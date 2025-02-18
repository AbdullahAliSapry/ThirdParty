//const form = document.getElementById("feedbackForm");
//const submitButton = document.getElementById("submitButton");
//const descriptionArea = document.getElementById("description");
//const charCount = document.getElementById("charCount");
//const fileInput = document.getElementById("fileInput");
//const imagePreviewContainer = document.getElementById("imagePreviewContainer");

//// تحديث عداد الأحرف
//descriptionArea.addEventListener("input", function () {
//    charCount.textContent = this.value.length;
//});

//// التحقق من صحة النموذج
//function validateForm() {
//    const problemSelected = form.querySelector('input[name="problem"]:checked');

//    if (problemSelected) {
//        submitButton.classList.add("enabled");
//        submitButton.disabled = false;
//    } else {
//        submitButton.classList.remove("enabled");
//        submitButton.disabled = true;
//    }
//}

//// إضافة مستمعي الأحداث للتحقق
//form.querySelectorAll('input[name="problem"]').forEach((radio) => {
//    radio.addEventListener("change", validateForm);
//});

//// معالجة رفع الصور
//fileInput.addEventListener("change", function (e) {
//    const files = Array.from(e.target.files);

//    files.forEach((file) => {
//        if (file.type.startsWith("image/")) {
//            const reader = new FileReader();
//            reader.onload = function (e) {
//                const previewDiv = document.createElement("div");
//                previewDiv.className = "image-preview";

//                const img = document.createElement("img");
//                img.src = e.target.result;

//                const deleteButton = document.createElement("button");
//                deleteButton.className = "delete-button";
//                deleteButton.innerHTML = "×";
//                deleteButton.onclick = function () {
//                    previewDiv.remove();
//                };

//                previewDiv.appendChild(img);
//                previewDiv.appendChild(deleteButton);
//                imagePreviewContainer.appendChild(previewDiv);
//            };
//            reader.readAsDataURL(file);
//        }
//    });
//});

///* معالجة تقديم النموذج*/
//function handleSubmit(event) {
//    event.preventDefault();
//    alert("تم إرسال الملاحظات بنجاح");
//    return false;
//}
