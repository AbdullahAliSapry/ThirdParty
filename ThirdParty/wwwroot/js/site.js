//// open slide bar
////const sidebar = document.getElementById("sidebar");
////const overlay = document.getElementById("overlay");
////const sidebarToggle = document.getElementById("sidebarToggle");
////const closeSidebar = document.getElementById("closeSidebar");

////function toggleSidebar() {
////    sidebar.classList.toggle("active");
////    overlay.style.display = sidebar.classList.contains("active") ? "block" : "none";
////}
////sidebarToggle.addEventListener("click", toggleSidebar);

////closeSidebar.addEventListener("click", toggleSidebar);
////overlay.addEventListener("click", toggleSidebar);

//// open sub slidebar

//document.querySelectorAll(".sidebar-item").forEach((item) => {
//    let timeout;
//    let closeTimeout;

//    const calculatePosition = (dropdownMenu) => {
//        const rect = item.getBoundingClientRect();
//        const topPosition = rect.top;
//        const windowHeight = window.innerHeight;
//        const menuHeight = dropdownMenu.offsetHeight || 400;
//        let finalTop = topPosition;

//        if (topPosition + menuHeight > windowHeight) {
//            finalTop = windowHeight - menuHeight;
//        }

//        return Math.max(0, finalTop);
//    };

//    item.addEventListener("mouseenter", () => {
//        clearTimeout(closeTimeout);
//        const dropdownMenu = item.querySelector(".dropdown-menu");

//        if (dropdownMenu) {
//            dropdownMenu.style.opacity = "0";
//            dropdownMenu.style.display = "block";
//            const finalTop = calculatePosition(dropdownMenu);
//            dropdownMenu.style.top = `${finalTop}px`;
//            dropdownMenu.style.opacity = "1";

//            timeout = setTimeout(() => {
//                if (!dropdownMenu.matches(":hover") && !item.matches(":hover")) {
//                    dropdownMenu.style.display = "none";
//                }
//            }, 300);
//        }
//    });

//    item.addEventListener("mouseleave", (e) => {
//        clearTimeout(timeout);
//        const dropdownMenu = item.querySelector(".dropdown-menu");

//        if (dropdownMenu && !dropdownMenu.contains(e.relatedTarget)) {
//            timeout = setTimeout(() => {
//                if (!dropdownMenu.matches(":hover")) {
//                    dropdownMenu.style.display = "none";
//                }
//            }, 300);
//        }
//    });

//    const dropdownMenu = item.querySelector(".dropdown-menu");
//    if (dropdownMenu) {
//        dropdownMenu.addEventListener("mouseenter", () => {
//            clearTimeout(timeout);
//        });

//        dropdownMenu.addEventListener("mouseleave", () => {
//            timeout = setTimeout(() => {
//                dropdownMenu.style.display = "none";
//            }, 300);
//        });
//    }
//});