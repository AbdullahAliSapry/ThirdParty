// gallory of product 
const thumbnails = document.querySelectorAll('.thumbnail');
const mainMedia = document.getElementById('mainMedia');
const prevButton = document.getElementById('prev');
const nextButton = document.getElementById('next');
let currentIndex = 0;

function updateMainMedia(index) {
    // Remove active class from all thumbnails
    thumbnails.forEach((thumbnail, i) => {
        thumbnail.classList.toggle('active', i === index);
    });

    const activeThumbnail = thumbnails[index];

    if (activeThumbnail.tagName === 'VIDEO') {
        // If the thumbnail is a video, set it as the main media
        mainMedia.innerHTML = `
            <video 
                src="${activeThumbnail.src}" 
                controls 
                autoplay 
                muted 
                loop 
                style="width: 100%; height: 100%; object-fit: cover;">
            </video>
        `;
    } else {
        // If the thumbnail is an image, set it as the main media
        mainMedia.innerHTML = `
            <img 
                src="${activeThumbnail.src}" 
                alt="Main Media" 
                style="width: 100%; height: 100%; object-fit: cover;">
            </img>
        `;
    }

    ensureVisible(index);
}

function ensureVisible(index) {
    const activeThumbnail = thumbnails[index];
    activeThumbnail.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
    // const containerRect = thumbnailsContainer.getBoundingClientRect();
    const thumbnailRect = activeThumbnail.getBoundingClientRect();
    // if (thumbnailRect.left < containerRect.left || thumbnailRect.right > containerRect.right) {
    //     activeThumbnail.scrollIntoView({ behavior: 'smooth', inline: 'center' });
    // }
}

thumbnails.forEach((thumbnail, index) => {
    thumbnail.addEventListener('click', () => {
        currentIndex = index;
        updateMainMedia(index);
    });
});

prevButton.addEventListener('click', () => {
    currentIndex = (currentIndex - 1 + thumbnails.length) % thumbnails.length;
    updateMainMedia(currentIndex);
});

nextButton.addEventListener('click', () => {
    currentIndex = (currentIndex + 1) % thumbnails.length;
    updateMainMedia(currentIndex);
});

// recomdation of the prodcut of slider
function scrollNav(direction) {
    const nav = document.querySelector(".rec-product");
    const scrollAmount = 200;

    if (direction === "left") {
        nav.scrollTo({
            left: nav.scrollLeft + scrollAmount,
            behavior: "smooth",
        });
    } else {
        nav.scrollTo({
            left: nav.scrollLeft - scrollAmount,
            behavior: "smooth",
        });
    }
}

// lemit letter in the title of the product
document.addEventListener("DOMContentLoaded", () => {
    const productTitles = document.querySelectorAll(".product-card h5");

    productTitles.forEach((title) => {
        if (title.textContent.length > 60) {
            title.textContent = title.textContent.slice(0, 60) + "...";
        }
    });
});
// ==========================
// Tab Switching Logic
const tabs = document.querySelectorAll('.tab');
const tabContents = document.querySelectorAll('.tab-content');

tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        // Remove active class from all tabs and contents
        tabs.forEach(t => t.classList.remove('active'));
        tabContents.forEach(content => content.classList.remove('active'));

        // Add active class to clicked tab and corresponding content
        tab.classList.add('active');
        const tabId = tab.getAttribute('data-tab');
        document.getElementById(tabId).classList.add('active');
    });
});


// ==============================================================
// swiper

const swiper = new Swiper('.swiper-container', {
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
        delay: 2000, // تأخير بين كل انتقال (3 ثوانٍ)
        disableOnInteraction: false, // استمر في التشغيل التلقائي حتى عند التفاعل
    },
    loop: true, // تشغيل اللوب حتى لا يتوقف عند آخر سلايد
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

document.addEventListener("DOMContentLoaded", function () {
    let rows = document.querySelectorAll(".size-row"); // جميع الصفوف
    let showMoreBtn = document.getElementById("showMoreBtn");
    let increment = 10; // عدد الصفوف التي ستظهر عند كل ضغطة

    showMoreBtn.addEventListener("click", function () {
        let hiddenRows = Array.from(rows).filter(row => row.classList.contains("hidden"));

        for (let i = 0; i < increment; i++) {
            if (hiddenRows[i]) {
                hiddenRows[i].classList.remove("hidden");

                setTimeout(() => {
                    hiddenRows[i].classList.add("show-custom");
                }, i * 100);
            }
        }

        // إذا لم يتبقَ صفوف مخفية، قم بإخفاء الزر
        if (Array.from(rows).every(row => !row.classList.contains("hidden"))) {
            showMoreBtn.parentElement.parentElement.style.display = "none";
        }
    });
});




let co_To_attrbure = document.querySelector(".Container_To_Attrbutes");


let status = co_To_attrbure.dataset.ismorerelation;



let attributes = [];

if (status === "true") {

    let cards = document.querySelectorAll(".colors_cards .cards");

    cards.forEach(card => {

        card.addEventListener("click", () => {

            cards.forEach(card2 => card2.classList.remove("active-border"))

            card.classList.add("active-border");
        })

    });

}
else {
    console.log("False")
}

function updateTotalPrice() {
    let totalPrice = 0;

    document.querySelectorAll(".quantity-value").forEach(quantityElement => {
        let itemId = quantityElement.id.replace('quantity_', '');
        let price = parseFloat(document.querySelector(`[data-item-id='${itemId}']`).getAttribute("data-price")) || 0;
        let quantity = parseInt(quantityElement.textContent) || 0;

        totalPrice += quantity * price;
    });

    document.getElementById("totalPrice").textContent = totalPrice.toFixed(2);
}

function adjustQuantity(nameAttribute, value, pid, vid, itemId, change) {
    let co_To_attrbure = document.querySelector(".Container_To_Attrbutes");

    let quantityElement = document.getElementById(`quantity_${itemId}`);
    if (!quantityElement) {
        console.error("❌ لم يتم العثور على عنصر الكمية.");
        return;
    }

    let currentQuantity = parseInt(quantityElement.textContent) || 0;
    let newQuantity = currentQuantity + change;

    if (newQuantity < 0) {
        console.warn("⚠️ الكمية لا يمكن أن تكون سالبة.");
        return;
    }

    quantityElement.textContent = newQuantity;

    let jsonDataRelated;

    if (status === "true") {
        let activeCard = document.querySelector(".cards.active-border");
        if (!activeCard) {
            console.error("❌ لم يتم العثور على أي بطاقة نشطة.");
            return;
        }
        jsonDataRelated = JSON.parse(activeCard.dataset.valuetoactivecard);
    } else {
        jsonDataRelated = { "key": nameAttribute, "Value": value, "Pid": pid, "Vid": vid };
    }

    // إنشاء الكائن الجديد بشكل صحيح
    let newObject = {
        [jsonDataRelated["key"]]: {
            value: jsonDataRelated["Value"],
            pid: jsonDataRelated["Pid"],
            vid: jsonDataRelated["Vid"]
        },
        [nameAttribute]: {
            value: value,
            pid: pid,
            vid: vid
        },
        Quantity: 1
    };

    // دالة مساعدة لمقارنة الكائنات
    function isEqual(obj1, obj2) {
        const keys1 = Object.keys(obj1).filter(key => key !== "Quantity"); // تجاهل خاصية الكمية
        const keys2 = Object.keys(obj2).filter(key => key !== "Quantity");

        if (keys1.length !== keys2.length) {
            return false;
        }

        for (let key of keys1) {
            if (
                obj1[key].value !== obj2[key].value ||
                obj1[key].pid !== obj2[key].pid ||
                obj1[key].vid !== obj2[key].vid
            ) {
                return false;
            }
        }

        return true;
    }

    // البحث عن الكائن الموجود
    let existingObject = attributes.find(obj => isEqual(obj, newObject));

    if (existingObject) {
        existingObject["Quantity"] += change; // تحديث الكمية إذا وجد الكائن
    } else {
        attributes.push(newObject); // إضافة الكائن الجديد إذا لم يتم العثور عليه
    }

    console.log("✅ الحالة الحالية للـ attributes:", attributes);

    updateTotalPrice();
}



// submit

let btnAddToCart = document.getElementById("add-to-cart-btn");

btnAddToCart.addEventListener("click", async () => {
    let data = JSON.parse(btnAddToCart.dataset.itemcart);

    if (attributes.length === 0) {
        toastr.error("Please select attributes or quantity");
        return;
    }

    let allQuantity = 0;
    data["AttributeItems"] = attributes.map(attr => {
        let quantity = attr.Quantity;
        allQuantity += quantity;
        let attrCopy = { ...attr };
        delete attrCopy.Quantity;

        return { AttributesJson: JSON.stringify(attrCopy), Quantity: quantity };
    });

    data["Quntity"] = allQuantity;
    data["FinalPrice"] = allQuantity * data["PricePerPiece"];

    try {
        let userId = document.querySelector(`input[name="userId"]`).value;


        let response = await fetch(`/Cart/AddItemInCart/${userId}`, {

            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        });


        const returndata = await response.json();

        if (response.ok) {
            toastr.success(returndata.message || "تمت الإضافة للمفضلة بنجاح!");
        } else {
            console.log(returndata)
            toastr.error(returndata.message || "حدث خطأ أثناء الإضافة.");
        }

    } catch (error) {
        console.error("Error:", error);
        toastr.error("حدث خطأ في الاتصال بالخادم أو في تنسيق البيانات.");
    }

});
