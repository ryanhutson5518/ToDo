document.addEventListener('htmx:beforeSwap', event => {
    // Close currently open modals if it has this attribute and request succeeded
    if (event.target.hasAttribute('hx-hide-modals-before-swap') && event.detail.isError == false) {
        for (const modalElement of document.getElementsByClassName('modal show')) {
            const modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();
        }
    }

    // All requests will swap html, even on errors
    event.detail.shouldSwap = true;
});

document.addEventListener('shown.bs.modal', event => {
    let element = event.target.querySelector('[autofocus-after-modal-shown]');
    element.focus();
});
