(function ($) {
    "use strict"; 

    var $sidebar = $(".sidebar");
    var sidebarBaseWidth = "15rem"; // SCSS
    var isSidebarVisible = true;

    // Función para alternar la visibilidad del sidebar
    function toggleSidebar() {
        if (isSidebarVisible) {
            $sidebar.css("width", "0");
            $sidebar.css("overflow", "hidden");
            $("body").addClass("sidebar-toggled"); // Opcional: mantener la clase para otros estilos
            $sidebar.addClass("toggled");       // Opcional: mantener la clase para otros estilos
        } else {
            $sidebar.css("width", sidebarBaseWidth);
            $sidebar.css("overflow", "visible"); 
            $("body").removeClass("sidebar-toggled"); // Opcional
            $sidebar.removeClass("toggled");      // Opcional
        }
        isSidebarVisible = !isSidebarVisible;
    }

    // Evento de clic para los botones de toggle
    $("#sidebarToggle, #sidebarToggleTop").on('click', function (e) {
        toggleSidebar();
    });

    $(window).resize(function () {
        if ($(window).width() < 768 && isSidebarVisible) {
            $sidebar.css("width", "0").css("overflow", "hidden");
            isSidebarVisible = false;
            $("body").addClass("sidebar-toggled");
            $sidebar.addClass("toggled");
        } else if ($(window).width() >= 768 && !isSidebarVisible) {
            $sidebar.css("width", sidebarBaseWidth).css("overflow", "visible");
            isSidebarVisible = true;
            $("body").removeClass("sidebar-toggled");
            $sidebar.removeClass("toggled");
        }
    });


})(jQuery); 