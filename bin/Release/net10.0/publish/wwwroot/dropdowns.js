document.addEventListener('click', event => {
    document.querySelectorAll('details.ticket-column-menu[open]').forEach(menu => {
        if (!menu.contains(event.target)) menu.removeAttribute('open');
    });
});
