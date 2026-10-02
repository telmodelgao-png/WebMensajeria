
"use strict";

const conexionPrueba = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7215/chatHub", {
        withCredentials: true
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function probarSignalR() {
    try {
        console.log("Conectando a /chatHub...");

        await conexionPrueba.start();

        console.log("WebSocket/SignalR conectado.");

        const respuesta = await conexionPrueba.invoke(
            "ProbarConexion"
        );

        console.log(respuesta);
    } catch (error) {
        console.error("Falló la prueba de SignalR:", error);
    }
}

conexionPrueba.onclose(error => {
    console.error("Conexión cerrada:", error);
});

probarSignalR();