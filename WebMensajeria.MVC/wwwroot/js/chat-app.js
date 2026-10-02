
"use strict";

(function () {
    if (location.search.includes("demo=1")) return;

    let abierto = null;
    let destinatarioActual = null;
    let nombreActual = "";

    async function pedir(url, opciones) {
        const r = await fetch(url, opciones);

        if (!r.ok) {
            const detalle = await r.text();
            throw new Error(
                `HTTP ${r.status}${detalle ? ": " + detalle : ""}`
            );
        }

        return r.status === 204 ? null : r.json();
    }

    Chat.init({
        onSearch: async texto => {
            try {
                const usuarios = await pedir(
                    "/Mensajeria/BuscarUsuarios?texto=" +
                    encodeURIComponent(texto)
                );

                Chat.setSearchResults(usuarios);
            } catch (e) {
                console.error("La búsqueda falló:", e);
            }
        },

        onPickUser: async (idUsuario, nombre) => {
            destinatarioActual = Number(idUsuario);
            nombreActual = nombre;

            try {
                const r = await pedir(
                    "/Mensajeria/ChatPrivado?idOtro=" +
                    encodeURIComponent(idUsuario)
                );

                if (r && r.idChat) {
                    Chat.openChat({
                        idChat: r.idChat,
                        nombre
                    });
                    return;
                }
            } catch (e) {
                console.error(
                    "No se pudo comprobar si ya existe el chat:",
                    e
                );
            }

            Chat.openDraft({
                idUsuario: destinatarioActual,
                nombre
            });
        },

        onOpen: async idChat => {
            abierto = idChat;

            try {
                await MensajeriaSignalR.conectar();

                const mensajes = await pedir(
                    "/Mensajeria/Mensajes?idChat=" +
                    encodeURIComponent(idChat)
                );

                if (abierto === idChat) {
                    Chat.setMessages(mensajes);
                }
            } catch (e) {
                console.error("No se pudo abrir el chat:", e);
            }
        },

        onSend: async (idChat, texto, borrador) => {
            try {
                const contenido = texto.trim();

                if (!contenido) return;

                // En un borrador, usar el destinatario seleccionado.
                const destinatarioId = borrador
                    ? Number(borrador.idUsuario)
                    : destinatarioActual;

                if (!destinatarioId) {
                    throw new Error(
                        "No se conoce el destinatario. Selecciona al usuario nuevamente."
                    );
                }

                const resultado =
                    await MensajeriaSignalR.enviarPrivado(
                        destinatarioId,
                        contenido
                    );

                // El Hub debe devolver idChat y los datos del mensaje.
                if (!resultado || !resultado.idChat) {
                    throw new Error(
                        "El servidor no devolvió el identificador del chat."
                    );
                }

                if (idChat === null) {
                    Chat.openChat({
                        idChat: resultado.idChat,
                        nombre: borrador.nombre
                    });
                } else {
                    Chat.addMessage(resultado);
                }
            } catch (e) {
                console.error("No se pudo enviar:", e);
                alert("No se pudo enviar el mensaje.\n" + e.message);
            }
        },

        onDelete: async idMensaje => {
            try {
                await pedir(
                    "/Mensajeria/EliminarMensaje?idMensaje=" +
                    encodeURIComponent(idMensaje),
                    { method: "POST" }
                );

                Chat.markDeleted(idMensaje);
            } catch (e) {
                console.error("No se pudo eliminar:", e);
                alert("No se pudo eliminar el mensaje.\n" + e.message);
            }
        }
    });

    // Recibir mensajes enviados por otros usuarios.
    MensajeriaSignalR.alRecibirMensaje(mensaje => {
        if (abierto === mensaje.idChat) {
            // Evitar añadir dos veces el mensaje propio si ya lo agregó onSend.
            if (
                !mensaje.idUsuario ||
                Number(mensaje.idUsuario) !== Number(
                    document.querySelector("#chatApp")?.dataset.yo
                )
            ) {
                Chat.addMessage(mensaje);
            }
        }
    });
})();