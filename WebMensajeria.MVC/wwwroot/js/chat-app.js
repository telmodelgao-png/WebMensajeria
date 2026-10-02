// Conexión real del chat con el servidor. Por ahora solo la búsqueda de usuarios.
(function () {
    if (location.search.includes('demo=1')) return; // en modo demo manda chat-demo.js

    Chat.init({
        onSearch: async texto => {
            try {
                const r = await fetch('/Mensajeria/BuscarUsuarios?texto=' + encodeURIComponent(texto));
                if (!r.ok) throw new Error('HTTP ' + r.status);
                Chat.setSearchResults(await r.json());
            } catch (e) {
                console.error('La búsqueda falló:', e);
            }
        },

        // Abre el chat vacío con ese usuario. Si ya existiera un chat con él, aquí se abrirá ese (siguiente paso).
        onPickUser: (idUsuario, nombre) => Chat.openDraft({ idUsuario, nombre }),

        // idChat es null en el primer mensaje: aquí hay que crear el chat y guardar el mensaje (siguiente paso).
        onSend: (idChat, texto, borrador) => {
            console.log('Falta guardar:', { idChat, texto, borrador });
        }
    });
})();
