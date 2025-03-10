
window.addEventListener('load', function () {
    document.querySelector('.parent-spinner').style.display = 'none';
});

let profile = document.querySelector('.profile');
let menu = document.querySelector('.menu');

profile?.addEventListener("click", () => {
    menu.classList.toggle('active');
})

document.addEventListener("DOMContentLoaded", function () {
    const iconContainer = document.querySelector(".icon-container");
    const menuList = document.querySelector(".menu-list-category");

    iconContainer.addEventListener("click", function (event) {
        event.stopPropagation();
        menuList.style.display = menuList.style.display === "block" ? "none" : "block";
    });

    document.addEventListener("click", function (event) {
        if (!iconContainer.contains(event.target)) {
            menuList.style.display = "none";
        }
    });
});


// handle upload file

document.getElementById("fileSearch").addEventListener("change", async function (event) {
    const file = event.target.files[0];
    if (file) {
        let formData = new FormData();
        formData.append("file", file);

        try {
            const response = await fetch("Product/UploadPhotoToSearch", {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: formData,
            });

            let data = await response.json();

            if (response.ok) {
                toastr.success(data.message || "تم رفع الصوره بنجاح!");
                document.getElementById("fileUrl").value = data.url;    
            } else {
                toastr.error(data.message || "حدث خطأ أثناء . رفع الصوره");
            }



        } catch (error) {
            console.error(error);
            toastr.error(error.message || "حدث خطأ في الاتصال.");
        }
    }
});