document.addEventListener("DOMContentLoaded", () => {
    const heartButtons = document.querySelectorAll(".heart-button");
    let favcdartnumber = document.querySelector(".wishlist-count");
    heartButtons.forEach(button => {
        button.addEventListener("click", async () => {
            try {
                const product = JSON.parse(button.dataset.product); // حاول تحويل البيانات
                console.log(button.dataset.product);
                const response = await fetch('/Product/AddProductToFavoutits', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: JSON.stringify({
                        product,
                        userId: button.dataset.userid,
                        quntity: 1,
                    })
                });

                const data = await response.json();

                if (response.ok) {
                    button.dataset.clicked = "true";
                    button.classList.add("added");
                    toastr.success(data.message || "تمت الإضافة للمفضلة بنجاح!");
                    var number = favcdartnumber.textContent;
                    favcdartnumber.textContent = Number.parseInt(number) + 1;
                } else {
                    toastr.error(data.message || "حدث خطأ أثناء الإضافة.");
                }
            } catch (error) {
                console.error("Error:", error);
                toastr.error("حدث خطأ في الاتصال بالخادم أو في تنسيق البيانات.");
            }
        });

    });
});

document.addEventListener("DOMContentLoaded", function () {
    const filterHeaders = document.querySelectorAll(".filter-header");

    filterHeaders.forEach(header => {
        header.addEventListener("click", function () {
            let section = this.parentElement;
            let content = section.querySelector(".filter-content");

            section.classList.toggle("active");
            this.classList.toggle("active");

            if (section.classList.contains("active")) {
                content.style.maxHeight = content.scrollHeight+25 + "px";
                content.style.padding = "15px 0";
            } else {
                content.style.maxHeight = "0";
                content.style.padding = "0";
            }
        });
    });
});


document.addEventListener("DOMContentLoaded", () => {

    let btn = document.querySelector(".show-more-category");
    let parent = document.querySelector(".filter-content");
    let allcategories = document.querySelectorAll(".filter-content ul li");
    btn.addEventListener("click", () => {
        parent.style.maxHeight = "fit-content";
        allcategories.forEach(e => e.classList.remove("hidden"));
        btn.style.display = "none";
    })

   
})

//let allcards = document.querySelectorAll(".card");
//console.log("Enter")
//allcards.forEach(el => {
//    el.addEventListener("click", (e) => {
//        let id = el.dataset.productid?.trim();
//        if (!id) {
//            console.error("Product ID is missing!");
//            return;
//        }
//        location.href = `/Product/ProductDetails?productId=${id}`;
//    });

//})