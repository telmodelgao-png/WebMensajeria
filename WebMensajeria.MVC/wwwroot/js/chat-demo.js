// Solo para ver el diseño. Abre /Mensajeria?demo=1. Elimina este archivo y su <script> al conectar tu lógica real.
(function () {
    if (!location.search.includes('demo=1')) return;

    const yo = parseInt(document.getElementById('chatApp').dataset.yo, 10) || 1;
    const ahora = Date.now();
    const min = n => new Date(ahora - n * 60000).toISOString();

    const conversaciones = [
        { idChat: 1, nombre: 'Marcela', ultimoMensaje: 'Nos vemos mañana entonces', fecha: min(5), noLeidos: 2 },
        { idChat: 2, nombre: 'Andrés', ultimoMensaje: 'Ya te mandé el archivo', fecha: min(95), noLeidos: 0 }
    ];

    const historial = {
        1: [
            { idChat: 1, idUsuario: yo + 1, nombre: 'Marcela', mensaje: 'Hola, ¿cómo vas con el proyecto?', fechaEnvio: min(40) },
            { idChat: 1, idUsuario: yo, nombre: 'Yo', mensaje: 'Bien, ya tengo el login y la base de datos.', fechaEnvio: min(38) },
            { idChat: 1, idUsuario: yo + 1, nombre: 'Marcela', mensaje: 'Nos vemos mañana entonces', fechaEnvio: min(5) }
        ],
        2: [{ idChat: 2, idUsuario: yo + 2, nombre: 'Andrés', mensaje: 'Ya te mandé el archivo', fechaEnvio: min(95) }]
    };

    Chat.init({
        onOpen: id => Chat.setMessages(historial[id] || []),
        onSend: (idChat, texto) => Chat.addMessage({ idChat, idUsuario: yo, nombre: 'Yo', mensaje: texto, fechaEnvio: new Date().toISOString() }),
        onSearch: texto => Chat.setSearchResults(
            ['Marcela', 'Andrés', 'Lucía', 'Pablo'].filter(n => n.toLowerCase().includes(texto.toLowerCase()))
                .map((n, i) => ({ idUsuario: 100 + i, nombreUsuario: n }))),
        onPickUser: (id, nombre) => Chat.openChat({ idChat: 1000 + id, nombre })
    });

    Chat.setConversations(conversaciones);
})();
