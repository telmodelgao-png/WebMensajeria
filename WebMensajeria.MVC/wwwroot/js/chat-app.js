"use strict";

// Conexión del chat con el servidor: MVC (búsqueda, historial, lista) y SignalR (enviar y recibir).
(function () {
    if (location.search.includes("demo=1")) return;

    let abierto = null;               // chat que se está mostrando (para ignorar respuestas tardías)
    const otroDeChat = new Map();     // idChat -> idUsuario de la otra persona. También sirve para saber qué chats ya están en la lista.

    async function pedir(url, opciones) {
        const r = await fetch(url, opciones);

        if (!r.ok) {
            const detalle = await r.text();
            throw new Error(`HTTP ${r.status}${detalle ? ": " + detalle : ""}`);
        }

        return r.status === 204 ? null : r.json();
    }

    async function cargarConversaciones() {
        try {
            const lista = await pedir("/Mensajeria/Conversaciones");
            lista.forEach(c => otroDeChat.set(c.idChat, c.idOtro));
            Chat.setConversations(lista);
        } catch (e) {
            console.error("No se pudo cargar la lista de chats:", e);
        }
    }

    Chat.init({
        onSearch: async texto => {
            try {
                Chat.setSearchResults(
                    await pedir("/Mensajeria/BuscarUsuarios?texto=" + encodeURIComponent(texto))
                );
            } catch (e) {
                console.error("La búsqueda falló:", e);
            }
        },

        // Si ya hay chat con esa persona se abre con su historial; si no, queda vacío hasta el primer mensaje.
        onPickUser: async (idUsuario, nombre) => {
            idUsuario = Number(idUsuario);

            try {
                const r = await fetch("/Mensajeria/ChatPrivado?idOtro=" + idUsuario);
                if (r.ok) {
                    const { idChat } = await r.json();
                    otroDeChat.set(idChat, idUsuario);
                    Chat.openChat({ idChat, nombre });
                    return;
                }
            } catch (e) {
                console.error("No se pudo comprobar si ya existe el chat:", e);
            }

            abierto = null;
            Chat.openDraft({ idUsuario, nombre });
        },

        onOpen: async idChat => {
            abierto = idChat;

            try {
                const mensajes = await pedir("/Mensajeria/Mensajes?idChat=" + encodeURIComponent(idChat));
                if (abierto === idChat) Chat.setMessages(mensajes);
            } catch (e) {
                console.error("No se pudo abrir el chat:", e);
            }
        },

        onSend: async (idChat, texto, borrador) => {
            const contenido = texto.trim();
            if (!contenido) return;

            // A quién se le envía: en un borrador, la persona elegida; en un chat existente, la otra persona de ese chat.
            const destinatarioId = borrador ? Number(borrador.idUsuario) : otroDeChat.get(idChat);

            if (!destinatarioId) {
                alert("No se conoce el destinatario de este chat. Búscalo de nuevo e inténtalo otra vez.");
                return;
            }

            try {
                const resultado = await MensajeriaSignalR.enviarPrivado(destinatarioId, contenido);

                if (!resultado || !resultado.idChat) {
                    throw new Error("El servidor no devolvió el identificador del chat.");
                }

                otroDeChat.set(resultado.idChat, destinatarioId);

                if (idChat === null) {
                    // Primer mensaje: el chat se acaba de crear. Al abrirlo se carga el historial, que ya lo incluye.
                    Chat.openChat({ idChat: resultado.idChat, nombre: borrador.nombre });
                } else {
                    Chat.addMessage(resultado);   // si el Hub ya lo entregó antes, chat.js evita duplicarlo
                }
            } catch (e) {
                console.error("No se pudo enviar:", e);
                alert("No se pudo enviar el mensaje.\n" + e.message);
            }
        },

        onDelete: async idMensaje => {
            try {
                await pedir("/Mensajeria/EliminarMensaje?idMensaje=" + encodeURIComponent(idMensaje), { method: "POST" });
                Chat.markDeleted(idMensaje);
            } catch (e) {
                console.error("No se pudo eliminar:", e);
                alert("No se pudo eliminar el mensaje.\n" + e.message);
            }
        }
    });

    // Mensajes que llegan en vivo (de la otra persona, o tuyos desde otra pestaña).
    MensajeriaSignalR.alRecibirMensaje(async mensaje => {
        // Chat que aún no está en la lista (alguien te escribe por primera vez): se recarga la lista.
        if (!otroDeChat.has(mensaje.idChat)) {
            await cargarConversaciones();
        }
        Chat.addMessage(mensaje);   // si es el chat abierto lo muestra; si no, sube el chat y marca no leído
    });

    // Conectar al abrir la página. Sin esto, quien no ha abierto ningún chat no recibe nada en vivo.
    MensajeriaSignalR.conectar().catch(e => console.error("No se pudo conectar SignalR:", e));

    cargarConversaciones();
})();
