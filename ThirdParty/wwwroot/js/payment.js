// Handle account selection
let inputaccount = document.querySelector(".AccountId");
function selectAccount(element,idAccount) {
    // Remove selected class from all cards
    document.querySelectorAll('.account-card').forEach(card => {
        card.classList.remove('selected');
    });

    // Add selected class to clicked card
    element.classList.add('selected');
    inputaccount.value = idAccount;
}


// Handle file upload
//const uploadArea = document.getElementById('uploadArea');
//const fileInput = document.getElementById('fileInput');
//const previewContainer = document.getElementById('previewContainer');

//uploadArea.addEventListener('click', () => {
//    fileInput.click();
//});

//uploadArea.addEventListener('dragover', (e) => {
//    e.preventDefault();
//    uploadArea.style.borderColor = '#36c7f6';
//    uploadArea.style.backgroundColor = '#d8eff860';
//});

//uploadArea.addEventListener('dragleave', () => {
//    uploadArea.style.borderColor = '#ccc';
//    uploadArea.style.backgroundColor = '';
//});

//uploadArea.addEventListener('drop', (e) => {
//    e.preventDefault();
//    uploadArea.style.borderColor = '#ccc';
//    uploadArea.style.backgroundColor = '';

//    if (e.dataTransfer.files.length) {
//        handleFiles(e.dataTransfer.files);
//    }
//});

//fileInput.addEventListener('change', () => {
//    if (fileInput.files.length) {
//        handleFiles(fileInput.files);
//    }
//});

//function handleFiles(files) {
//    for (const file of files) {
//        if (file.type.startsWith('image/')) {
//            const reader = new FileReader();

//            reader.onload = (e) => {
//                createImagePreview(e.target.result);
//            };

//            reader.readAsDataURL(file);
//        }
//    }
//}

//function createImagePreview(src) {
//    const previewElement = document.createElement('div');
//    previewElement.className = 'image-preview';

//    const img = document.createElement('img');
//    img.src = src;

//    const deleteBtn = document.createElement('div');
//    deleteBtn.className = 'delete-btn';
//    deleteBtn.textContent = '×';
//    deleteBtn.addEventListener('click', (e) => {
//        e.stopPropagation();
//        previewContainer.removeChild(previewElement);
//    });

//    previewElement.appendChild(img);
//    previewElement.appendChild(deleteBtn);
//    previewContainer.appendChild(previewElement);
//}

// Copy account number to clipboard
function copyToClipboard(elementId, event) {
    event.stopPropagation();
    const text = document.getElementById(elementId).textContent;
    navigator.clipboard.writeText(text).then(() => {
        // Show success message
        const successElement = event.target.nextElementSibling;
        successElement.style.display = 'inline';

        // Hide after 2 seconds
        setTimeout(() => {
            successElement.style.display = 'none';
        }, 2000);
    });
}

function validateForm(event) {
    // منع إرسال النموذج قبل التحقق
    event.preventDefault();



    // تحقق إذا كان الحقل فارغًا
    if (!inputaccount || !inputaccount.value.trim()) {
        Swal.fire({
            title: "Error",
            text: "من فضلك اختر الحساب الذي قمت بتحويل عليه",
            icon: "error",
            confirmButtonText: "OK"
        });

        return false; // منع الإرسال
    }

    // إذا كانت جميع الحقول صحيحة، السماح بالإرسال
    event.target.submit();  // إرسال النموذج بعد التحقق
    return true;
}
