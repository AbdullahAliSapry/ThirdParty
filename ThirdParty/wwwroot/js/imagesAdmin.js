document.getElementById("imageForm").addEventListener("submit", async function (event) {
    event.preventDefault();
    let isValid = true;

    // التحقق من رفع الصورة
    let imageInput = document.getElementById("imageUpload");
    let imageError = document.getElementById("imageError");
    if (!imageInput.files.length) {
        imageError.textContent = "Please upload a photo.";
        isValid = false;
    } else {
        imageError.textContent = "";
    }

    let typeSelect = document.getElementById("typeimage");
    let typeError = document.getElementById("typeError");
    if (typeSelect.value === "#" || typeSelect.value === "") {
        typeError.textContent = "Please select an image type.";
        isValid = false;
    } else {
        typeError.textContent = "";
    }

    if (isValid) {
        let imageDynamicData = new FormData();
        imageDynamicData.append("file", imageInput.files[0]);
        imageDynamicData.append("typeImageUpload", typeSelect.value?.trim());
        console.log("🔍 محتوى `FormData` قبل الإرسال:", [...imageDynamicData.entries()]);

        try {
            const response = await fetch("/Admin/AddImageDynamic", {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: imageDynamicData,
            });

            let data = await response.json();
            if (response.ok) {

                toastr.success(data.message || "تم ارسال البيانات بنجاح!");

                let con = document.querySelector("table tbody ");
                //IsActive = true,
                //    PublicId = result.PublicId,
                //    Url = result.Url,
                //    typeImageUpload
                console.log(data);
                let newimagw = `<tr>
                    <td>#${data.image.id +1}</td>
                    <td>${data.image.typeImageUpload ===0 ? "logo" : "advertisement"}</td>
                    <td>${data.image.isActive ? "Active" : "Not Active"}</td>
                    <td>
                        <a href="${data.image.url}" target="_blank" class="btn">
                                View_Image
                            </a>
                    </td>

                    <td>
                        <a href="/Admin/ChangeStatusImage?imageid=${data.image.id}"
                        asp-action=""
                           asp-route-imageid="@image.value.Id" class="btn">Change Status</a>
                    </td>
                </tr>`

                con.innerHTML += newimagw;
           

            } else {
                console.log(data)

                toastr.error(data.message || "حدث خطأ أثناء ارسال البيانات.");
            }
        } catch (error) {
            console.error(error);
            toastr.error(error.message || "حدث خطأ في الاتصال.");
        }
    }

});