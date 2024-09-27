// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

//-- Fixed header and content scrolling
//document.addEventListener("DOMContentLoaded", function () {
//    const fixedHeader = document.querySelector('.fixed-header');
//    const itemContent = document.querySelector('.col-md-9'); // Adjust as needed

//    itemContent.addEventListener('scroll', function () {
//        if (itemContent.scrollTop > 0) {
//            fixedHeader.classList.add('scrolled');
//        } else {
//            fixedHeader.classList.remove('scrolled');
//        }
//    });
//});

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