//// open slide bar
//const sidebar = document.getElementById("sidebar");
//const overlay = document.getElementById("overlay");
//const sidebarToggle = document.getElementById("sidebarToggle");
//const closeSidebar = document.getElementById("closeSidebar");


//console.log("test2");
//function toggleSidebar() {
//    sidebar.classList.toggle("active");
//    overlay.style.display = sidebar.classList.contains("active") ? "block" : "none";
//}
//sidebarToggle.addEventListener("click", toggleSidebar);
//closeSidebar.addEventListener("click", toggleSidebar);
//overlay.addEventListener("click", toggleSidebar);

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


//const navbar = document.querySelector(".navbar-menu");

//window.addEventListener("scroll", () => {
//    if (window.scrollY > 140 || window.scrollX > 1140) {
//        navbar.classList.add("show");
//    } else {
//        navbar.classList.remove("show");
//    }
//});

//window.addEventListener("resize", () => {
//    if (window.innerWidth <= 1140) {
//        navbar.classList.add("hide");
//    } else {
//        navbar.classList.remove("hide");
//    }
//});

//document.querySelectorAll(".nav-item").forEach((item) => {
//    let timeout;
//    let closeTimeout;

//    const calculatePosition = (dropdown) => {
//        const rect = item.getBoundingClientRect();
//        dropdown.style.left = `${rect.left}px`;
//        return dropdown;
//    };

//    item.addEventListener("mouseenter", () => {
//        clearTimeout(closeTimeout);
//        const dropdown = item.querySelector(".nav-dropdown");

//        if (dropdown) {
//            dropdown.style.opacity = "0";
//            dropdown.style.display = "flex";
//            calculatePosition(dropdown);

//            setTimeout(() => {
//                dropdown.style.opacity = "1";
//            }, 50);

//            timeout = setTimeout(() => {
//                if (!dropdown.matches(":hover") && !item.matches(":hover")) {
//                    dropdown.style.display = "none";
//                }
//            }, 300);
//        }
//    });

//    item.addEventListener("mouseleave", (e) => {
//        clearTimeout(timeout);
//        const dropdown = item.querySelector(".nav-dropdown");

//        if (dropdown && e.relatedTarget && !dropdown.contains(e.relatedTarget)) {
//            timeout = setTimeout(() => {
//                if (!dropdown.matches(":hover")) {
//                    dropdown.style.display = "none";
//                }
//            }, 300);
//        }
//    });

//    const dropdown = item.querySelector(".nav-dropdown");
//    if (dropdown) {
//        dropdown.addEventListener("mouseenter", () => {
//            clearTimeout(timeout);
//        });

//        dropdown.addEventListener("mouseleave", () => {
//            timeout = setTimeout(() => {
//                dropdown.style.display = "none";
//            }, 300);
//        });
//    }
//});

//let resizeTimeout;
//window.addEventListener("resize", () => {
//    clearTimeout(resizeTimeout);
//    resizeTimeout = setTimeout(() => {
//        document.querySelectorAll(".nav-dropdown").forEach((dropdown) => {
//            if (dropdown.style.display === "flex") {
//                const item = dropdown.closest(".nav-item");
//                if (item) {
//                    const rect = item.getBoundingClientRect();
//                    dropdown.style.left = `${rect.left}px`;
//                }
//            }
//        });
//    }, 200);
//});

//function scrollNav(direction) {
//    const nav = document.querySelector(".nav-filter");
//    const scrollAmount = 200;

//    if (direction === "left") {
//        nav.scrollTo({
//            left: nav.scrollLeft + scrollAmount,
//            behavior: "smooth",
//        });
//    } else {
//        nav.scrollTo({
//            left: nav.scrollLeft - scrollAmount,
//            behavior: "smooth",
//        });
//    }
//}

//document.querySelectorAll(".nav-item").forEach((item) => {
//    item.addEventListener("click", (e) => {
//        e.preventDefault();
//        document.querySelectorAll(".nav-item").forEach((i) => i.classList.remove("active"));
//        item.classList.add("active");
//    });
//});
