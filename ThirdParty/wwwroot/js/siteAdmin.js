const toggleButton = document.querySelector('#sidebar-toggle');
const sidebar = document.querySelector('#sidebar');
const showSidebar = (e) => {
    sidebar.classList.contains('show') ? (sidebar.classList.remove('show'),
        toggleButton.classList.remove('position-fixed')) : (sidebar.classList.add('show'), toggleButton.classList.add('position-fixed'));
};
toggleButton.addEventListener("click", showSidebar, false);