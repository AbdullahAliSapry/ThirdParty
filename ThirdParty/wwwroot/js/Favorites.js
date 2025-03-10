document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tab-custom");
    const favoritesContainer = document.querySelector(".favorites-container");
    const sellersContainer = document.querySelector(".favorites-container-saller");

    tabs.forEach(tab => {
        tab.addEventListener("click", function () {
            document.querySelector(".tab-custom.active").classList.remove("active");
            this.classList.add("active");

            const isItemsTab = this.dataset.type === "items";
            const isItemsEmpty = favoritesContainer.dataset.isempety === "True";
            const isSellersEmpty = sellersContainer.dataset.isempety === "True";

            if (isItemsTab) {
                if (isItemsEmpty) {
                    favoritesContainer.innerHTML = "<div class='empty-favorites'>You have no favorite items</div>";
                }
                favoritesContainer.classList.add("active");
                sellersContainer.classList.remove("active");
            } else {
                if (isSellersEmpty) {
                    sellersContainer.innerHTML = "<div class='empty-favorites'>You have no favorite sellers</div>";
                }
                sellersContainer.classList.add("active");
                favoritesContainer.classList.remove("active");
            }
        });
    });

    window.removeFavorite = function (button) {
        const card = button.closest(".favorite-card");
        card.remove();
    };
});

document.addEventListener("DOMContentLoaded", function () {
    const cartContainer = document.querySelector(".cart-container");

    function updateTotalPrice(input) {
        let quantity = parseInt(input.value);
        if (isNaN(quantity) || quantity < 1) {
            input.value = 1;
            quantity = 1;
        }
        let pricePerItem = parseFloat(input.closest(".cart-item").querySelector(".price").textContent);
        let totalElement = input.closest(".cart-item").querySelector(".total-price");
        totalElement.textContent = (quantity * pricePerItem).toFixed(2) + " SAR";
    }

    cartContainer?.addEventListener("click", function (event) {
        let target = event.target;

        // Handle decrease button
        if (target.closest(".decrease")) {
            let input = target.closest(".cart-item").querySelector(".quantity-input");
            if (input.value > 1) {
                input.value--;
                updateTotalPrice(input);
            }
        }

        // Handle increase button
        if (target.closest(".increase")) {
            let input = target.closest(".cart-item").querySelector(".quantity-input");
            input.value++;
            updateTotalPrice(input);
        }

        // Handle delete button
        if (target.closest(".delete-item")) {
            target.closest(".cart-item").remove();
        }
    });

    cartContainer?.addEventListener("input", function (event) {
        let target = event.target;
        if (target.classList.contains("quantity-input")) {
            updateTotalPrice(target);
        }
    });
});

let btnSubmit = document.querySelector(".Save-btn");
let userId = document.querySelector(".userId-input")?.value;

btnSubmit?.addEventListener("click", async () => {

    let allcartitems = document.querySelectorAll(".cart-item");

    let allIdsAndQuntities = [];




    allcartitems.forEach(cart => {

        let id = cart.querySelector(".ProductId").value;
        let newquntity = cart.querySelector(".quantity-input").value;

        allIdsAndQuntities.push({ Id: id, newQuntity: newquntity });
    });

    try {
        const response = await fetch('/UserSetting/UpdateFavoriteCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({
                updatedFavoriteItems: allIdsAndQuntities,
                userId: userId,
            })
        });

        const data = await response.json();

        if (response.ok) {
            toastr.success(data.message || "تمت حفظ البينات بنجاح!");
        } else {
            toastr.error(data.message || "حدث خطأ أثناء التحديث.");
        }


    }
    catch (error) {
        console.log(error);

    }

});

let inputCheck = document.querySelector(".check-all-parent");
let btndelete = document.querySelector(".delete-btn");
let btnCart = document.querySelector(".cart-btn");
inputCheck?.addEventListener("change", () => {
    let allItemsChecked = document.querySelectorAll(".parent-products .form-check-input");

    let isChecked = inputCheck.checked;

    allItemsChecked.forEach(input => {
        input.checked = isChecked;
    });

    btnCart.classList.toggle("disabled");
    btndelete.classList.toggle("disabled");
});



let btnDeleteALl = document.querySelector(".delete-all-btn");

btnDeleteALl.addEventListener("click", async () => {
    try {
        const response = await fetch("/UserSetting/DeleteFromFav", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({
                DeleteAll: true,
                Ids: [],
                userId: userId
            }),
        });

        let data = await response.json();

        if (response.ok) {
            toastr.success(data.message || "تم حذف البيانات بنجاح!");

            setTimeout(() => {
                location.reload();
            }, 1000);

        } else {
            toastr.error(data.message || "حدث خطأ أثناء الحذف.");
        }
    } catch (error) {
        console.error(error);
        toastr.error(error.message || "حدث خطأ في الاتصال.");
    }
});



let allbtndeleteoneitem = document.querySelectorAll(".delete-one-item-btn");

allbtndeleteoneitem.forEach(btn => {
    btn.addEventListener("click", async () => {


        let id = btn.querySelector('input[name="Id"]')?.value;

        try {


            const response = await fetch("/UserSetting/DeleteFromFav", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value

                }
                ,
                body: JSON.stringify({
                    DeleteAll: false,
                    Ids: [id],
                    userId: userId

                }),

            });
            let data = await response.json();

            if (response.ok) {
                toastr.success(data.message || "تم حذف البيانات بنجاح!");

                //setTimeout(() => {
                //    location.reload();
                //}, 1000);

            } else {
                toastr.error(data.message || "حدث خطأ أثناء الحذف.");
            }

        }
        catch (error) {
            console.log(error);

        }


    });


});