
const urlParams = new URLSearchParams(window.location.search);

const userId = urlParams.get('userid');




// Open Modal
function openModal() {
    document.getElementById('accountModal').classList.add('active');
    document.querySelector('.modal-overlay').style.display = 'block';
}

// Close Modal
function closeModal() {
    document.getElementById('accountModal').classList.remove('active');
    document.querySelector('.modal-overlay').style.display = 'none';
}

//// Save Account and Add Row to Table
//function saveAccount() {
//    const accountNumber = document.getElementById('accountNumber').value.trim();
//    const iban = document.getElementById('iban').value.trim();
//    const name = document.getElementById('name').value.trim();

//    if (accountNumber && iban && name) {
//        const table = document.getElementById('accountTable');
//        const tbody = table.querySelector('tbody');

//        const row = document.createElement('tr');
//        row.innerHTML = `
//                    <td>${accountNumber}</td>
//                    <td>${iban}</td>
//                    <td>${name}</td>
//                    <td><button class="delete-btn" onclick="deleteRow(this)">Delete</button></td>
//                `;
//        tbody.appendChild(row);

//        closeModal();
//        clearInputs();
//    } else {
//        alert('Please fill in all fields.');
//    }
//}

//// Delete Row from Table
//function deleteRow(button) {
//    const row = button.closest('tr'); // Get the parent row of the button
//    row.remove(); // Remove the row
//}

//// Clear Input Fields
//function clearInputs() {
//    document.getElementById('accountNumber').value = '';
//    document.getElementById('iban').value = '';
//    document.getElementById('name').value = '';
//}

document.addEventListener("DOMContentLoaded",  function () {
    document.getElementById("accountForm").addEventListener("submit", async function (event) {
        event.preventDefault();

        let isValid = true;

        // مسح الأخطاء السابقة
        document.getElementById("accountNumberError").textContent = "";
        document.getElementById("nameAccountError").textContent = "";
        document.getElementById("nameBankError").textContent = "";

        // التحقق من رقم الحساب
        let accountNumber = document.getElementById("accountNumber").value.trim();
        if (accountNumber === "" || !/^\d{5,20}$/.test(accountNumber)) {
            document.getElementById("accountNumberError").textContent = "Account number must be 5-20 digits.";
            isValid = false;
        }

        // التحقق من اسم الحساب
        let nameAccount = document.getElementById("nameAccount").value.trim();
        if (nameAccount === "" || nameAccount.length < 3) {
            document.getElementById("nameAccountError").textContent = "Name must be at least 3 characters.";
            isValid = false;
        }

        // التحقق من اسم البنك
        let nameBank = document.getElementById("nameBank").value.trim();
        if (nameBank === "" || nameBank.length < 3) {
            document.getElementById("nameBankError").textContent = "Bank name must be at least 3 characters.";
            isValid = false;
        }

        // منع الإرسال إذا كان هناك خطأ
        if (isValid) {


            let dataSend = {
                nameOfBank: nameBank,
                nameOfAccount: nameAccount,
                numberOfAccount: accountNumber,
                marketerId: parseInt(document.getElementById("marketerId").value) || 0
            };
            try {
                const response = await fetch(`/Marketer/AddAccount/${userId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },

                    body: JSON.stringify(dataSend)
                });

                let data = await response.json();

                if (response.ok) {
                    toastr.success(data.message);
                    const newAccount = data.account;
                    const accountTableBody = document.querySelector("#accountTable tbody");
                    const row = document.createElement('tr');
                    row.innerHTML = `
                                <td>${newAccount.numberOfAccount}</td>
                                <td>${newAccount.nameOfAccount}</td>
                                <td>${newAccount.nameOfBank}</td>
                                <td>${newAccount.isActived?"yes":"no"}</td>
                                <td>
                                    <button class="btn btn-primary" onclick="viewAccount(${newAccount.Id})">Revoke</button>
                                    <button class="btn btn-danger" onclick="deleteAccount(${newAccount.Id})">Delete</button>
                                </td>
                            `;
                    accountTableBody.appendChild(row);
                    console.log(data)

                } else {
                    toastr.error(data.message || "حدث خطأ اثناء طلب الكود يرجي التواصل مع الدعم!");
                    console.log(data)
                }
            } catch (error) {
                console.error(error.message || error);
                console.log(error)
                toastr.error("حدث خطأ في الاتصال!");

            }
        }
    });
});
