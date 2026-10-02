
"use strict";

window.MensajeriaSignalR = (() => {
    const conexion = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7215/chatHub", {
            withCredentials: true
        })
        .withAutomaticReconnect()
        .build();

    let inicio = null;
    const chatsUnidos = new Set();
    const callbacksMensaje = new Set();

    // Distribuir los mensajes recibidos a la interfaz.
    conexion.on("MensajeRecibido", mensaje => {
        callbacksMensaje.forEach(callback => {
            try {
                callback(mensaje);
            } catch (error) {
                console.error("Error al mostrar el mensaje:", error);
            }
        });
    });

    // Volver a unirse a los chats después de reconectar.
    conexion.onreconnected(async () => {
        console.log("SignalR reconectado.");

        for (const chatId of chatsUnidos) {
            try {
                await conexion.invoke("UnirseAlChat", chatId);
            } catch (error) {
                console.error(
                    `No se pudo volver a unir al chat ${chatId}:`,
                    error
                );
            }
        }
    });

    conexion.onclose(error => {
        if (error) {
            console.error("SignalR se desconectó:", error);
        } else {
            console.warn("SignalR desconectado.");
        }
    });

    async function conectar() {
        if (conexion.state === signalR.HubConnectionState.Connected) {
            return;
        }

        if (inicio) {
            return inicio;
        }

        inicio = (async () => {
            try {
                if (conexion.state === signalR.HubConnectionState.Disconnected) {
                    await conexion.start();
                    console.log("SignalR conectado.");
                }

                if (conexion.state !== signalR.HubConnectionState.Connected) {
                    throw new Error(
                        "SignalR todavía no está conectado. Inténtalo de nuevo."
                    );
                }
            } finally {
                inicio = null;
            }
        })();

        return inicio;
    }

    function alRecibirMensaje(callback) {
        callbacksMensaje.add(callback);

        // Permite retirar el callback cuando ya no se necesite.
        return () => callbacksMensaje.delete(callback);
    }

    async function enviarPrivado(destinatarioId, contenido) {
        await conectar();

        return conexion.invoke(
            "EnviarMensajePrivado",
            Number(destinatarioId),
            contenido
        );
    }

    async function enviarGrupo(chatId, contenido) {
        await conectar();

        return conexion.invoke(
            "EnviarMensajeGrupo",
            Number(chatId),
            contenido
        );
    }

    async function unirse(chatId) {
        await conectar();

        chatId = Number(chatId);

        await conexion.invoke("UnirseAlChat", chatId);
        chatsUnidos.add(chatId);
    }

    async function salir(chatId) {
        chatId = Number(chatId);

        if (conexion.state === signalR.HubConnectionState.Connected) {
            await conexion.invoke("SalirDelChat", chatId);
        }

        chatsUnidos.delete(chatId);
    }

    return {
        conexion,
        conectar,
        alRecibirMensaje,
        enviarPrivado,
        enviarGrupo,
        unirse,
        salir
    };
})();