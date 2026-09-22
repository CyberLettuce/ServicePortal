window.servicePortal = window.servicePortal || {};

window.servicePortal.setReadOnlyMode = function (enabled) {
    const content = document.querySelector("article.content");
    if (!content) return;

    document.body.classList.toggle("service-portal-read-only", enabled);

    const apply = () => {
        content.querySelectorAll("input, select, textarea, button").forEach(control => {
            control.disabled = enabled;
        });
    };

    apply();

    if (enabled && !window.servicePortal.readOnlyObserver) {
        window.servicePortal.readOnlyObserver = new MutationObserver(apply);
        window.servicePortal.readOnlyObserver.observe(content, { childList: true, subtree: true });
    }
};
