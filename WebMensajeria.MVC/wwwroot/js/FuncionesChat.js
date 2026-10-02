
import {
    conectarChat,
    unirseAlChat,
    enviarMensajePrivado,
    enviarMensajeGrupo
} from "./signalr.js";

// Preparar la conexión cuando se cargue el chat.
document.addEventListener("DOMContentLoaded", async () => {
    try {
        await conectarChat();
        console.log("Chat preparado.");
    } catch (error) {
        console.error("No se pudo iniciar el chat:", error);
    }
});

// Funciones que llamará la interfaz.
async function abrirChat(chatId) {
    await unirseAlChat(chatId);
}

async function enviarPrivado(destinatarioId, contenido) {
    await enviarMensajePrivado(destinatarioId, contenido);
}

async function enviarGrupo(chatId, contenido) {
    await enviarMensajeGrupo(chatId, contenido);
}

// Exponer las funciones para utilizarlas desde los eventos HTML.
window.abrirChat = abrirChat;
window.enviarPrivado = enviarPrivado;
window.enviarGrupo = enviarGrupo;