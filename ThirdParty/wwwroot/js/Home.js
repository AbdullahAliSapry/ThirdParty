console.log("Home.js3");

let checkinput = document?.getElementById("checkinputHome");


if (checkinput !== null) {
    const swiper = new Swiper('.second-swiper', {
        slidesPerView: 3,
        spaceBetween: 30,
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        autoplay: {
            delay: 2000,
            disableOnInteraction: false,
        },
        loop: true,
        breakpoints: {
            320: {
                slidesPerView: 1,
            },
            640: {
                slidesPerView: 2,
            },
            768: {
                slidesPerView: 3,
            },
            1024: {
                slidesPerView: 4,
            },
            1200: {
                slidesPerView: 5,
            },
        },
    });

}

const swiper1 = new Swiper('.first-swiper', {
    slidesPerView: 1,
    spaceBetween: 10,
    loop: true,
    navigation: {
        nextEl: '.swiper-container-first .swiper-button-next',
        prevEl: '.swiper-container-first .swiper-button-prev',
    },
    pagination: {
        el: '.swiper-container-first .swiper-pagination',
        clickable: true,
    },
});


let allcards = document.querySelectorAll(".card");
allcards.forEach(el => {
    el.addEventListener("click", (e) => {
        let id = el.dataset.productid?.trim();
        if (!id) {
            console.error("Product ID is missing!");
            return;
        }
        location.href = `/Product/ProductDetails?productId=${id}`;
    });

})