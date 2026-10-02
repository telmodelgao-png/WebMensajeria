/*
  Frontend del chat. No habla con el servidor: tú le pasas los datos y él te avisa de las acciones.

  Datos que espera:
    conversación: { idChat, nombre, ultimoMensaje, fecha, noLeidos }
    mensaje:      { idMensaje, idChat, idUsuario, mensaje, fechaEnvio, estado, eliminado, nombre? }
                  estado: "enviado" | "entregado" | "leido"   (solo en mensajes propios)
    usuario:      { idUsuario, nombreUsuario }                 (resultados de búsqueda)

  Uso:
    Chat.init({ onSend, onOpen, onSearch, onPickUser, onDelete })
        onSend(idChat, texto, borrador)  idChat es null en el primer mensaje (borrador = { idUsuario, nombre })
        onDelete(idMensaje)              el usuario pidió eliminar uno de sus mensajes
    Chat.setConversations(lista)    Chat.openChat({ idChat, nombre })
    Chat.openDraft({ idUsuario, nombre })   chat vacío con alguien con quien aún no hay chat
    Chat.setMessages(lista)         Chat.addMessage(mensaje)
    Chat.updateMessage({ idMensaje, ...campos })   Chat.markDeleted(idMensaje)
    Chat.setSearchResults(lista)
*/
window.Chat = (function () {
    const app = document.getElementById('chatApp');
    const $ = id => document.getElementById(id);
    const yo = parseInt(app.dataset.yo, 10);

    let h = { onSend() {}, onOpen() {}, onSearch() {}, onPickUser() {}, onDelete() {} };
    let conversaciones = [];
    let activo = null;
    let borrador = null;      // { idUsuario, nombre } cuando aún no existe el chat
    let nombrePanel = '';
    let ultimoDia = null;
    const burbujas = new Map();   // idMensaje -> { m, box }

    const ETIQUETA = { enviado: 'Enviado', entregado: 'Entregado', leido: 'Leído' };

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

    /* ---------- panel de conversación ---------- */
    function mostrarVacio() {
        const v = el('div', 'chat-vacio');
        v.append(el('strong', null, 'Empieza la conversación'),
                 el('span', null, `Aún no hay mensajes con ${nombrePanel}. Escribe el primero abajo.`));
        $('mensajes').append(v);
    }

    function abrirPanel(nombre) {
        nombrePanel = nombre;
        ultimoDia = null;
        burbujas.clear();
        $('placeholder').hidden = true;
        $('panel').hidden = false;
        $('hNombre').textContent = nombre;
        $('hAvatar').replaceWith(Object.assign(avatar(nombre), { id: 'hAvatar' }));
        $('mensajes').replaceChildren();
        app.classList.add('is-chat-open');
        $('resultados').hidden = true;
        $('conversaciones').hidden = false;
    }

    /* ---------- mensajes ---------- */
    function construir(m) {
        const mio = m.idUsuario === yo;
        const box = el('div', 'chat-msg ' + (mio ? 'chat-msg--mine' : 'chat-msg--other'));

        if (!mio && m.nombre && !m.eliminado) box.append(el('div', 'chat-msg__author', m.nombre));

        box.append(el('div', 'chat-bubble' + (m.eliminado ? ' chat-bubble--eliminado' : ''),
                      m.eliminado ? 'Se eliminó este mensaje' : m.mensaje));

        const meta = el('div', 'chat-msg__meta');
        meta.append(el('span', 'chat-msg__time', hora(m.fechaEnvio)));

        if (mio && !m.eliminado && m.estado) {
            meta.append(el('span', 'chat-msg__estado chat-msg__estado--' + m.estado, ETIQUETA[m.estado] || m.estado));
        }
        if (mio && !m.eliminado && m.idMensaje != null) {
            const b = el('button', 'chat-msg__eliminar', 'Eliminar');
            b.type = 'button';
            b.addEventListener('click', () => {
                if (confirm('¿Eliminar este mensaje? Tampoco se verá para la otra persona.')) h.onDelete(m.idMensaje);
            });
            meta.append(b);
        }
        box.append(meta);
        return box;
    }

    function burbuja(m) {
        const vacio = $('mensajes').querySelector('.chat-vacio');
        if (vacio) vacio.remove();

        const d = claveDia(m.fechaEnvio);
        if (d !== ultimoDia) {
            $('mensajes').append(el('div', 'chat-day', dia(m.fechaEnvio)));
            ultimoDia = d;
        }
        const box = construir(m);
        $('mensajes').append(box);
        if (m.idMensaje != null) burbujas.set(m.idMensaje, { m, box });
    }

    function actualizarResumen(m, sumarNoLeido) {
        const c = conversaciones.find(x => x.idChat === m.idChat);
        if (!c) return;
        c.ultimoMensaje = m.eliminado ? 'Mensaje eliminado' : m.mensaje;
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
            burbujas.clear();
            ultimoDia = null;
            if (!lista || lista.length === 0) mostrarVacio();
            else lista.forEach(burbuja);
            bajar();
        },

        addMessage(m) {
            if (m.idMensaje != null && burbujas.has(m.idMensaje)) { api.updateMessage(m); return; } // evita duplicados
            if (m.idChat === activo) {
                burbuja(m);
                bajar();
                actualizarResumen(m, false);
            } else {
                actualizarResumen(m, m.idUsuario !== yo);
            }
        },

        // Cambia campos de un mensaje ya pintado (por ejemplo estado: "leido").
        updateMessage(parcial) {
            const r = burbujas.get(parcial.idMensaje);
            if (!r) return;
            Object.assign(r.m, parcial);
            const nuevo = construir(r.m);
            r.box.replaceWith(nuevo);
            r.box = nuevo;
        },

        markDeleted(idMensaje) { api.updateMessage({ idMensaje, eliminado: true }); },

        setSearchResults(lista) {
            const ul = $('resultados');
            ul.replaceChildren();
            if (!lista || lista.length === 0) {
                ul.append(el('li', 'chat-empty', 'No se encontró a nadie con ese nombre.'));
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
