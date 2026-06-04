// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// ===== NAVBAR DROPDOWN - DANH MỤC =====
(function () {
    document.addEventListener('DOMContentLoaded', function () {

        // Desktop: click toggle mở/đóng
        var toggle = document.querySelector('.nav-dropdown-toggle');
        var menu   = document.querySelector('.nav-dropdown-menu');

        if (toggle && menu) {
            toggle.addEventListener('click', function (e) {
                e.stopPropagation();
                var isOpen = toggle.getAttribute('aria-expanded') === 'true';
                toggle.setAttribute('aria-expanded', String(!isOpen));
                menu.classList.toggle('is-open', !isOpen);
            });

            // Click ngoài thì đóng
            document.addEventListener('click', function () {
                toggle.setAttribute('aria-expanded', 'false');
                menu.classList.remove('is-open');
            });

            // Click bên trong menu không đóng
            menu.addEventListener('click', function (e) {
                e.stopPropagation();
            });
        }

        // Mobile: accordion danh mục
        var mobileToggle = document.querySelector('.mobile-dropdown-toggle');
        if (mobileToggle) {
            mobileToggle.addEventListener('click', function () {
                var group = mobileToggle.closest('.mobile-dropdown-group');
                if (group) group.classList.toggle('open');
            });
        }
    });
})();
