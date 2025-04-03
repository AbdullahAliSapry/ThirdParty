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