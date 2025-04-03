document.getElementById('bankForm').addEventListener('submit',async function (event) {
    event.preventDefault();

    let isValid = true;

    const amount = document.getElementById('Amount');
    if (amount.value.trim() === '' || parseFloat(amount.value) <= 0) {
        document.getElementById('error-amount').textContent = 'Amount must be greater than 0';
        isValid = false;
    } else {
        document.getElementById('error-amount').textContent = '';
    }

    const nameBank = document.getElementById('NameBank');
    if (!/^[A-Za-z\s]+$/.test(nameBank.value.trim())) {
        document.getElementById('error-namebank').textContent = 'Bank name must contain only letters and spaces';
        isValid = false;
    } else {
        document.getElementById('error-namebank').textContent = '';
    }

    const markterAccountId = document.getElementById('MarkterAccountId');

    const image = document.getElementById('Image');
    if (image.files.length === 0) {
        document.getElementById('error-image').textContent = 'Image is required';
        isValid = false;
    } else {
        document.getElementById('error-image').textContent = '';
    }

    if (isValid) {


        let imageDynamicData = new FormData();
        imageDynamicData.append("Image", image.files[0]);
        imageDynamicData.append("NameBank", nameBank.value.trim());
        imageDynamicData.append("MarkterAccountId", markterAccountId.value.trim());
        imageDynamicData.append("Amount", amount.value);
        console.log("🔍 محتوى `FormData` قبل الإرسال:", [...imageDynamicData.entries()]);

        try {
            const response = await fetch("/Admin/AddBillingToMarketer", {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: imageDynamicData,
            });

            let data = await response.json();
            if (response.ok) {
                console.log(data);
                toastr.success(data.message || "تم ارسال البيانات بنجاح!");



                setTimeout(() => {
                    location.reload();
                }, 1000);


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