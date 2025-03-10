

let input_check_All = document.querySelector(".check-all-parent");
let buttons = document.querySelectorAll(".header-to-cart button");
let allInputsCheck = document.querySelectorAll(".cart-item .form-check input");
let orderSummary = document.querySelector(".order-summary");
let userId = document.querySelector('input[name="userId"]').value;
let cartid = document.querySelector('input[name="cartId"]').value;
function updateButtons() {
    let checkedInputs = document.querySelectorAll(".cart-item .form-check input:checked");
    let count = checkedInputs.length;

    buttons.forEach(btn => {
        let spanText = btn.querySelector("span");
        if (spanText) {
            spanText.textContent = count;
        }

        if (count > 0) {
            btn.classList.remove("disabled");
        } else {
            btn.classList.add("disabled");
        }
    });

    input_check_All.checked = count === allInputsCheck.length;
}

function updateOrderSummary() {
    let checkedItems = document.querySelectorAll(".cart-item .form-check input:checked").length;

    if (checkedItems > 0) {
        orderSummary.style.display = "block";
    } else {
        orderSummary.style.display = "none";
    }
}

input_check_All.addEventListener("change", () => {
    allInputsCheck.forEach(input => input.checked = input_check_All.checked);
    updateButtons();
    updateOrderSummary();
});

allInputsCheck.forEach(input => {
    input.addEventListener("change", () => {
        updateButtons();
        updateOrderSummary();
    });
});

document.addEventListener("DOMContentLoaded", () => {
    input_check_All.checked = true;
    allInputsCheck.forEach(input => input.checked = true);

    updateButtons();
});

// update qunity
document.addEventListener("DOMContentLoaded", function () {
    function updateTotalPrice(quantityInput, priceElement, pricePerPiece) {
        let quantity = parseInt(quantityInput.value);
        if (isNaN(quantity) || quantity < 1) {
            quantity = 1;
            quantityInput.value = quantity;
        }
        priceElement.innerText = (quantity * pricePerPiece).toFixed(2) + " $";
    }

    function updateMainQuantity(cartItem, value) {
        let mainQuantityInput = cartItem.querySelector(".quantity-input");
        let attributeQuantities = cartItem.querySelectorAll(".quantity-input-attrbute");
        let totalQuantity = 0;

        attributeQuantities.forEach(input => {
            totalQuantity += parseInt(input.value) || 0;
        });

        mainQuantityInput.value = totalQuantity;
        mainQuantityInput.dispatchEvent(new Event("input"));
        let priceElement = cartItem.querySelector(".price-total[data-price]");
        let pricePerPiece = parseFloat(priceElement.getAttribute("data-price"));
        updateTotalPrice(mainQuantityInput, priceElement, pricePerPiece);
    }

    document.querySelectorAll(".cart-item").forEach(cartItem => {

        const priceElement = cartItem.querySelector(".price-total[data-price]");

        let pricePerPiece = parseFloat(priceElement.getAttribute("data-price"));

    });

    document.querySelectorAll(".cart-item").forEach(cartItem => {
        const cartitemid = cartItem.dataset.cartitemid;
        cartItem.querySelectorAll(".quantity-control-attrbute").forEach(control => {
            const minusBtn = control.querySelector(".quantity-btn-attrbute:first-child");
            const plusBtn = control.querySelector(".quantity-btn-attrbute:last-child");
            const quantityInput = control.querySelector(".quantity-input-attrbute");

            minusBtn.addEventListener("click", async function () {
                if (quantityInput.value > 0) {
                    quantityInput.value--;

                    let result = await UpdateQuntityDatabase(quantityInput.value, true, cartitemid, quantityInput.dataset.itematbuteid);
                    console.log(result);
                    if (result) {
                        updateMainQuantity(cartItem, quantityInput.value);
                    }
                    else {
                        toastr.error("حدث خطأ اثناء تحديث الكميه!");

                    }
                }
            });

            plusBtn.addEventListener("click", async function () {
                quantityInput.value++;
                let result = await UpdateQuntityDatabase(quantityInput.value, true, cartitemid, quantityInput.dataset.itematbuteid);
                console.log(result);
                if (result) {
                    updateMainQuantity(cartItem);
                }
                else {
                    toastr.error("حدث خطأ اثناء تحديث الكميه!");

                }
            });
        });
    });
});
const UpdateQuntityDatabase = async (newqunity, isquntityAttrbute, CartitemId, ItemAttrbuteId = 0) => {
    const datasend = {
        newquntity: newqunity,
        isquntityAttrbute: isquntityAttrbute,
        CartId: cartid,
        CartitemId: CartitemId,
    }
    if (isquntityAttrbute) {
        datasend["ItemAttrbuteId"] = ItemAttrbuteId;
    }

    try {
        const response = await fetch(`/Cart/UpdateQuntity/${userId}`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(datasend),
        });

        let data = await response.json();

        if (response.ok) {
            console.log(data.message);
            return true;
        } else {
            console.log(data.message);
        }
    } catch (error) {
        console.error(error.message || error);
    }
}


document.addEventListener("DOMContentLoaded", function () {
    const quantityInputs = document.querySelectorAll(".quantity-input");
    const checkboxes = document.querySelectorAll(".cart-item .form-check-input");
    const totalPriceElement = document.getElementById("totalPrice");

    // دالة لحساب السعر الإجمالي
    function calculateTotal() {
        let total = 0;

        checkboxes.forEach(checkbox => {
            if (checkbox.checked) {
                const cartItem = checkbox.closest('.cart-item');
                const quantityInput = cartItem.querySelector('.quantity-input');

                const pricePerItem = parseFloat(quantityInput.dataset.price) || 0;
                const quantity = parseInt(quantityInput.value) || 0;

                total += quantity * pricePerItem;
            }
        });

        totalPriceElement.textContent = `SAR: ${total.toFixed(2)}`;
    }

    quantityInputs.forEach(input => {
        input.addEventListener("input", function () {
            calculateTotal();
        });
    });

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener("change", function () {
            calculateTotal();
        });
    });

    calculateTotal();
});


// checkout btn

let checkoutBtn = document.querySelector(".checkout-btn");
console.log(checkoutBtn);
checkoutBtn.addEventListener("click", (e) => {

    const checkboxes = document.querySelectorAll(".cart-item .form-check-input");
    let totalweight = 0.0;
    let totalpricee = 0.0;
    let totaltax = 0.0;
    checkboxes.forEach(checkbox => {

        if (checkbox.checked) {
            let parentcart = checkbox.parentElement.parentElement;
            let parentcartphysicalparameters = JSON.parse(parentcart.dataset.physicalparameters);
            let qunity = Number.parseInt(parentcart.querySelector(".quantity-input").value);
            totalweight += Number.parseFloat(parentcartphysicalparameters.Weight) * qunity;
            totalpricee += parentcart.querySelector(".price-total").dataset.price * qunity;
        }

    });
    if (totalpricee < 100) {
        console.log("Error");
        e.preventDefault();
    }
    let totalttaxparent = document.querySelector(".total-tax");
    let totalweightcon = document.querySelector(".total-weight p span");
    let totalttaxtSpan = document.querySelector(".total-tax p span");
    let totoalpricemodelcon = document.querySelector(".total-price-final p span");
    let totoalpricemodelconwithtax = document.querySelector(".total-price-withTax p span");
    let arrOftaxes = JSON.parse(totalttaxparent.dataset.totaltaxlist);

    arrOftaxes.forEach(tax => {
        if (tax.UpperLimit > totalpricee && tax.LowerLimit < totalpricee) {
            totaltax = totalpricee * tax.CommissionRate
        }
    });


    totalweightcon.textContent = totalweight.toFixed(2);
    totalttaxtSpan.textContent = totaltax.toFixed(3);
    totoalpricemodelcon.textContent = totalpricee.toFixed(3);
    totoalpricemodelconwithtax.textContent = ((+totalpricee) + (+totaltax)).toFixed(3)
});



Submitcode = document.querySelector(".total-price-shipping form");
btnSubmitcode = document.querySelector(".total-price-shipping form .btn");


Submitcode.addEventListener("submit", async (e) => {

    e.preventDefault();

    // input
    let code = document.getElementById("codeMarketer").value;
    let errorcontainermessage = document.querySelector(".error-code");
    if (code === null || code.length === 0) {
        errorcontainermessage.textContent = "Please Enter Valid Code";
        return;
    }

    let totoalpricemodelconwithtax = document.querySelector(".total-price-withTax p span");
    try {
        const response = await fetch(`/Marketer/GetDateToCode/${userId}?code=${code}&totalPrice=${parseFloat(totoalpricemodelconwithtax.textContent)}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('.cart-container input[name="__RequestVerificationToken"]').value
            },
        });

        let data = await response.json();

        if (response.ok) {
            toastr.success(data.message || "تم قبول الكود بنجاح يرجي المتابعه في العمليه");
            totoalpricemodelconwithtax.textContent = parseFloat(totoalpricemodelconwithtax.textContent) - data.discount;
            btnSubmitcode.style.display="none"
            console.log(data)

        } else {
            toastr.error(data.message || "حدث خطأ اثناء طلب الكود يرجي التواصل مع الدعم!");
        }
    } catch (error) {
        console.log(error);
        console.error(error.message || error);
        toastr.error("حدث خطأ في الاتصال!");

    }

});


            //public string UserId { get; set; } = null!;
            //public List < int > IdsCartItems { get; set; } = null!;
            //public string ? Code { get; set; }
            //public double TotoalPrice { get; set; }
            //public double TotolaTax { get; set; }
            //public int TotalQunity { get; set; }
            //public double TotoalPriceWithTax { get; set; }

// add submit


let btnSubmitData = document.querySelector(".submit-ietm-to-order");

btnSubmitData.addEventListener("click", async (e) => {
    e.preventDefault();

    const checkboxes = document.querySelectorAll(".cart-item .form-check-input");
    let IdsCartItems = [];
    let totalQunity = 0;

    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            let parentcart = checkbox.parentElement.parentElement;
            IdsCartItems.push(parentcart.dataset.cartitemid);
            totalQunity += Number.parseInt(parentcart.querySelector(".quantity-input").value) || 0;
        }
    });

    let addOrderData = {
        userId: userId,
        idscartitems: IdsCartItems,
        totoalPrice: parseFloat(document.querySelector(".total-price-final p span").textContent) || 0,
        totolatax: parseFloat(document.querySelector(".total-tax p span").textContent) || 0,
        totoalpricewithTax: parseFloat(document.querySelector(".total-price-withTax p span").textContent) || 0,
        code: document.getElementById("codeMarketer").value || "",
        totalQunity: totalQunity
    };
    console.log(addOrderData);
    try {
        const response = await fetch(`/Order/AddOrder`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(addOrderData) || "{}",
        });

        let data = await response.json();

        if (response.ok) {
            toastr.success(data.message || "تم طلب الاوردر بنجاح، وهو الآن في فترة المراجعة.");
            console.log("Order Response:", data);
        } else {
            toastr.error(data.message || "حدث خطأ أثناء طلب الأوردر. يرجى التواصل مع الدعم!");
            console.error("Order Error:", data);
        }
    } catch (error) {
        console.error("Fetch Error:", error);
        toastr.error("حدث خطأ في الاتصال! حاول مرة أخرى.");
    }
});
