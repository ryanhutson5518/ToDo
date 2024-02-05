document.addEventListener('htmx:beforeSwap', event => {
    // Close any open modals
    if (event.detail.isError == false) {
        for (const modalElement of document.getElementsByClassName('modal show')) {
            const modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();
        }
    }

    // All requests will swap html, even on errors
    event.detail.shouldSwap = true;
});