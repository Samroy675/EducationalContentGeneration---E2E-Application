window.edugen = {
    showToast: function (message) {
        let toast = document.getElementById('edugen-toast');
        if (!toast) {
            toast = document.createElement('div');
            toast.id = 'edugen-toast';
            toast.style.cssText = [
                'position:fixed', 'bottom:80px', 'right:22px', 'z-index:9999',
                'background:var(--surface)', 'border:1px solid var(--accent)',
                'border-radius:10px', 'padding:9px 15px',
                'font-size:13px', 'color:var(--accent)',
                'display:flex', 'align-items:center', 'gap:8px',
                'box-shadow:0 4px 18px rgba(0,0,0,0.25)',
                'font-family:"DM Sans",sans-serif',
                'opacity:0', 'transform:translateY(8px)',
                'transition:opacity 0.25s,transform 0.25s',
                'pointer-events:none'
            ].join(';');
            document.body.appendChild(toast);
        }
        toast.textContent = '\u2713 ' + message;
        toast.style.opacity = '1';
        toast.style.transform = 'translateY(0)';
        clearTimeout(window._edugenToastTimer);
        window._edugenToastTimer = setTimeout(function () {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(8px)';
        }, 2500);
    },

    scrollToBottom: function (elementId) {
        var el = document.getElementById(elementId);
        if (el) el.scrollTop = el.scrollHeight;
    },

    setTheme: function (theme) {
        document.documentElement.setAttribute('data-theme', theme);
        localStorage.setItem('edugen-theme', theme);
    },

    initTheme: function () {
        var saved = localStorage.getItem('edugen-theme') || 'dark';
        document.documentElement.setAttribute('data-theme', saved);
        return saved;
    },

    addHistoryItem: function (itemJson) {
        let history = localStorage.getItem("edugen-history");
        let items = history ? JSON.parse(history) : [];
        items.unshift(JSON.parse(itemJson));
        items = items.slice(0, 20);
        localStorage.setItem("edugen-history", JSON.stringify(items));
    },

     getHistory: function () {
         return localStorage.getItem("edugen-history");
    },

    removeHistoryitem: function (id) {
        let history = JSON.parse(localStorage.getItem("edugen-history") || "[]");
        history = history.filter(x => x.id !== id);
        localStorage.setItem("edugen-history", JSON.stringify(history));
    }
};

// Apply saved theme immediately on page load
(function () {
    var saved = localStorage.getItem('edugen-theme') || 'dark';
    document.documentElement.setAttribute('data-theme', saved);
})();
