/*
  Frontend del chat. No habla con el servidor: tú le pasas los datos y él te avisa de las acciones.

  Datos que espera:
    conversación: { idChat, nombre, ultimoMensaje, fecha, noLeidos }
    mensaje:      { idChat, idUsuario, mensaje, fechaEnvio, nombre? }  (nombre es opcional; sirve para grupos)
    usuario:      { idUsuario, nombreUsuario }                      (resultados de búsqueda)

  Uso:
    Chat.init({ onSend, onOpen, onSearch, onPickUser })     onSend(idChat, texto, borrador): idChat es null si es el primer mensaje
    Chat.setConversations(lista)   Chat.openChat({ idChat, nombre })
    Chat.openDraft({ idUsuario, nombre })   abre el chat vacío con alguien con quien aún no hay chat
    Chat.setMessages(lista)        Chat.addMessage(mensaje)
    Chat.setSearchResults(lista)
*/
window.Chat = (function () {
    const app = document.getElementById('chatApp');
    const $ = id => document.getElementById(id);
    const yo = parseInt(app.dataset.yo, 10);

    let h = { onSend() {}, onOpen() {}, onSearch() {}, onPickUser() {} };
    let conversaciones = [];
    let activo = null;
    let ultimoDia = null;
    let borrador = null;   // { idUsuario, nombre } cuando aún no existe el chat
    let nombrePanel = '';

    /* ---------- utilidades ---------- */
    const el = (tag, cls, text) => {
        const e = document.createElement(tag);
        if (cls) e.className = cls;
        if (text !== undefined) e.textContent = text; // textContent evita inyección de HTML
        return e;
    };
    // El servidor guarda en UTC: si la fecha no trae zona, se interpreta como UTC.
    const fecha = f => {
        if (f instanceof Date) return f;
        const s = String(f);
        return new Date(/(Z|[+-]\d\d:?\d\d)$/.test(s) ? s : s + 'Z');
    };
    const hora = f => fecha(f).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    const dia = f => fecha(f).toLocaleDateString([], { weekday: 'long', day: 'numeric', month: 'long' });
    const claveDia = f => fecha(f).toDateString();

    function avatar(nombre) {
        const a = el('div', 'chat-avatar', (nombre || '?').trim().charAt(0).toUpperCase());
        let n = 0;
        for (const c of nombre || '') n = (n * 31 + c.charCodeAt(0)) % 360;
        a.style.background = `hsl(${n} 38% 40%)`;
        return a;
    }

    const bajar = () => { const m = $('mensajes'); m.scrollTop = m.scrollHeight; };

    /* ---------- lista de conversaciones ---------- */
    function renderList() {
        const ul = $('conversaciones');
        ul.replaceChildren();
        $('sinChats').hidden = conversaciones.length > 0;

        conversaciones.forEach(c => {
            const li = el('li', 'chat-item' + (c.idChat === activo ? ' is-active' : ''));
            li.tabIndex = 0;
            li.dataset.id = c.idChat;

            const top = el('div', 'chat-item__top');
            top.append(el('span', 'chat-item__name', c.nombre),
                       el('span', 'chat-item__time', c.fecha ? hora(c.fecha) : ''));

            const bottom = el('div', 'chat-item__bottom');
            bottom.append(el('span', 'chat-item__preview', c.ultimoMensaje || ''));
            if (c.noLeidos > 0) bottom.append(el('span', 'chat-badge', c.noLeidos));

            const body = el('div', 'chat-item__body');
            body.append(top, bottom);
            li.append(avatar(c.nombre), body);

            const abrir = () => api.openChat(c);
            li.addEventListener('click', abrir);
            li.addEventListener('keydown', e => { if (e.key === 'Enter') abrir(); });
            ul.append(li);
        });
    }

    /* ---------- mensajes ---------- */
    function mostrarVacio() {
        const v = el('div', 'chat-vacio');
        v.append(el('strong', null, 'Empieza la conversación'),
                 el('span', null, `Aún no hay mensajes con ${nombrePanel}. Escribe el primero abajo.`));
        $('mensajes').append(v);
    }

    function abrirPanel(nombre) {
        nombrePanel = nombre;
        ultimoDia = null;
        $('placeholder').hidden = true;
        $('panel').hidden = false;
        $('hNombre').textContent = nombre;
        $('hAvatar').replaceWith(Object.assign(avatar(nombre), { id: 'hAvatar' }));
        $('mensajes').replaceChildren();
        app.classList.add('is-chat-open');
        $('resultados').hidden = true;
        $('conversaciones').hidden = false;
    }

    function burbuja(m) {
        const vacio = $('mensajes').querySelector('.chat-vacio');
        if (vacio) vacio.remove();
        const mio = m.idUsuario === yo;
        const d = claveDia(m.fechaEnvio);
        if (d !== ultimoDia) {
            $('mensajes').append(el('div', 'chat-day', dia(m.fechaEnvio)));
            ultimoDia = d;
        }
        const box = el('div', 'chat-msg ' + (mio ? 'chat-msg--mine' : 'chat-msg--other'));
        if (!mio && m.nombre) box.append(el('div', 'chat-msg__author', m.nombre));
        box.append(el('div', 'chat-bubble', m.mensaje), el('div', 'chat-msg__time', hora(m.fechaEnvio)));
        $('mensajes').append(box);
    }

    function actualizarResumen(m, sumarNoLeido) {
        const c = conversaciones.find(x => x.idChat === m.idChat);
        if (!c) return;
        c.ultimoMensaje = m.mensaje;
        c.fecha = m.fechaEnvio;
        c.noLeidos = sumarNoLeido ? (c.noLeidos || 0) + 1 : 0;
        conversaciones = [c, ...conversaciones.filter(x => x !== c)]; // la más reciente arriba
        renderList();
    }

    /* ---------- API pública ---------- */
    const api = {
        init(handlers) { h = Object.assign(h, handlers); },

        setConversations(lista) { conversaciones = lista || []; renderList(); },

        openChat(c) {
            activo = c.idChat;
            borrador = null;
            abrirPanel(c.nombre);
            let conv = conversaciones.find(x => x.idChat === c.idChat);
            if (!conv) {   // chat recién creado: aparece en la lista
                conv = { idChat: c.idChat, nombre: c.nombre, ultimoMensaje: '', fecha: null, noLeidos: 0 };
                conversaciones = [conv, ...conversaciones];
            }
            conv.noLeidos = 0;
            renderList();
            h.onOpen(c.idChat);
            $('texto').focus();
        },

        // Chat con alguien con quien todavía no hay conversación: se muestra vacío y el chat se crea al enviar el primer mensaje.
        openDraft(u) {
            activo = null;
            borrador = u;
            abrirPanel(u.nombre);
            mostrarVacio();
            renderList();
            $('texto').focus();
        },

        setMessages(lista) {
            $('mensajes').replaceChildren();
            ultimoDia = null;
            if (!lista || lista.length === 0) mostrarVacio();
            else lista.forEach(burbuja);
            bajar();
        },

        addMessage(m) {
            if (m.idChat === activo) {
                burbuja(m);
                bajar();
                actualizarResumen(m, false);
            } else {
                actualizarResumen(m, m.idUsuario !== yo);
            }
        },

        setSearchResults(lista) {
            const ul = $('resultados');
            ul.replaceChildren();
            if (!lista || lista.length === 0) {
                const li = el('li', 'chat-empty', 'No se encontró a nadie con ese nombre.');
                ul.append(li);
            }
            (lista || []).forEach(u => {
                const li = el('li', 'chat-item');
                li.tabIndex = 0;
                const body = el('div', 'chat-item__body');
                body.append(el('div', 'chat-item__name', u.nombreUsuario));
                li.append(avatar(u.nombreUsuario), body);
                const elegir = () => {
                    $('buscar').value = '';
                    ul.hidden = true;
                    $('conversaciones').hidden = false;
                    h.onPickUser(u.idUsuario, u.nombreUsuario);
                };
                li.addEventListener('click', elegir);
                li.addEventListener('keydown', e => { if (e.key === 'Enter') elegir(); });
                ul.append(li);
            });
            ul.hidden = false;
            $('conversaciones').hidden = true;
        }
    };

    /* ---------- eventos de la pantalla ---------- */
    let t;
    $('buscar').addEventListener('input', e => {
        clearTimeout(t);
        const texto = e.target.value.trim();
        if (texto.length < 2) {
            $('resultados').hidden = true;
            $('conversaciones').hidden = false;
            return;
        }
        t = setTimeout(() => h.onSearch(texto), 250);
    });

    const caja = $('texto');
    caja.addEventListener('input', () => {
        caja.style.height = 'auto';
        caja.style.height = Math.min(caja.scrollHeight, 140) + 'px';
    });
    caja.addEventListener('keydown', e => {
        if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); $('formEnviar').requestSubmit(); }
    });

    $('formEnviar').addEventListener('submit', e => {
        e.preventDefault();
        const texto = caja.value.trim();
        if (!texto || (activo === null && !borrador)) return;
        h.onSend(activo, texto, borrador);
        caja.value = '';
        caja.style.height = 'auto';
    });

    $('volver').addEventListener('click', () => app.classList.remove('is-chat-open'));

    return api;
})();
