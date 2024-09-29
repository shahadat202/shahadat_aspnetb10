// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification



//-- Item page content ---  
// Toggle modal on grid icon click
document.getElementById('gridIcon').addEventListener('click', function () {
    const modal = document.getElementById('layoutModal');
    if (modal.style.display === 'block') {
        modal.style.display = 'none';
    } else {
        modal.style.display = 'block';
    }
});
// Close modal when clicking outside of it
window.addEventListener('click', function (e) {
    const modal = document.getElementById('layoutModal');
    if (!modal.contains(e.target) && e.target.id !== 'gridIcon') {
        modal.style.display = 'none';
    }
});