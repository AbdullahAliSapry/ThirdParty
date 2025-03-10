document.addEventListener("DOMContentLoaded", function () {
    var toastEl = document.querySelector('.toast');
    if (toastEl) {
        var toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
});


/****  for local storage*///
// ============================================================
// // Select all sidebar links and sections
const sidebarLinks = document.querySelectorAll(".profile-links a");
const sections = document.querySelectorAll(".main-content .section");





// =====================================================

// Add event listeners to each sidebar link
sidebarLinks.forEach((link, index) => {
    link.addEventListener("click", (e) => {

        sidebarLinks.forEach((link) => link.classList.remove("active"));
        sections.forEach((section) => section.classList.remove("active"));

        link.classList.add("active");
        sections[index].classList.add("active");
    });
});


document.getElementById("update-btn").addEventListener("click", function () {
    toggleEditMode(true);
});

document.getElementById("cancel-btn").addEventListener("click", function () {
    restoreOriginalValues();
    toggleEditMode(false);
});

function toggleEditMode(enable) {
    const fields = ["Name", "email", "PhoneNumber"];
    fields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        field.readOnly = !enable;
    });

    document.getElementById("save-btn").style.display = enable ? "inline-block" : "none";
    document.getElementById("cancel-btn").style.display = enable ? "inline-block" : "none";
    document.getElementById("update-btn").style.display = enable ? "none" : "inline-block";
}

function restoreOriginalValues() {
    document.querySelectorAll("input[data-original-value]").forEach(input => {
        input.value = input.getAttribute("data-original-value");
    });
}

// تحديث عرض اسم المستخدم تلقائيًا
/*updateUsernameDisplay();*/

//saveBtn.addEventListener("click", function () {
//    const firstName = document.getElementById("first-name").value.trim();
//    const lastName = document.getElementById("last-name").value.trim();
//    const email = document.getElementById("email").value.trim();
//    const password = document.getElementById("password").value.trim();

//    let isValid = true;

//     إزالة رسائل الخطأ السابقة
//    document.querySelectorAll(".error-message").forEach((el) => el.remove());

//     التحقق من الاسم الأول والاسم الأخير
//    const nameRegex = /^[A-Za-z\s]+$/; // فقط الحروف والمسافات مسموح بها
//    if (!nameRegex.test(firstName)) {
//        showError("first-name", "First name must contain only letters and spaces.");
//        isValid = false;
//    }
//    if (!nameRegex.test(lastName)) {
//        showError("last-name", "Last name must contain only letters and spaces.");
//        isValid = false;
//    }

//    const emailRegex = /^[a-zA-Z0-9._%+-]+@gmail\.com$/; 
//    if (!emailRegex.test(email)) {
//        showError("email", "Please enter a valid Gmail address.");
//        isValid = false;
//    }

//     التحقق من كلمة المرور
//    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$/;
//    if (!passwordRegex.test(password)) {
//        showError("password", "Password must contain at least one uppercase letter, one lowercase letter, and one special character.");
//        isValid = false;
//    }




//     إذا كانت جميع البيانات صحيحة، يتم حفظها
//    if (isValid) {
//        const currentUser = JSON.parse(localStorage.getItem("currentUser")) || {};

//        const updatedUser = {
//            ...currentUser,
//            firstName: firstName || currentUser.firstName,
//            lastName: lastName || currentUser.lastName,
//            email: email || currentUser.email,
//            password: password || currentUser.password,

//        };

//        localStorage.setItem("currentUser", JSON.stringify(updatedUser));

//        const users = JSON.parse(localStorage.getItem("users")) || [];
//        const updatedUsers = users.map((user) => {
//            if (user.email === currentUser.email) {
//                return {
//                    ...user,
//                    firstName: firstName || user.firstName,
//                    lastName: lastName || user.lastName,
//                    email: email || user.email,
//                    password: password || user.password,
//                    phone: phone || user.phone,
//                    gender: gender || user.gender,
//                };
//            }
//            return user;
//        });

//        localStorage.setItem("users", JSON.stringify(updatedUsers));

//        document.getElementById("first-name").readOnly = true;
//        document.getElementById("last-name").readOnly = true;
//        document.getElementById("email").readOnly = true;
//        document.getElementById("password").readOnly = true;

//        saveBtn.style.display = "none";
//        cancelBtn.style.display = "none";
//        updateBtn.style.display = "inline-block";
//    }
//     تحديث عرض اسم المستخدم تلقائيًا
//    updateUsernameDisplay();
//});

//window.addEventListener("storage", function () {
//    updateUsernameDisplay(); // يتم تحديث اسم المستخدم تلقائيًا عند تغيير البيانات في LocalStorage
//});

//cancelBtn.addEventListener("click", function () {
//    // استرجاع البيانات القديمة من localStorage
//    const currentUser = JSON.parse(localStorage.getItem("currentUser")) || {};

//    // إعادة القيم القديمة للحقول
//    document.getElementById("first-name").value = currentUser.firstName || "";
//    document.getElementById("email").value = currentUser.email || "";
//    //document.getElementById("password").value = currentUser.password || "";

//    // إلغاء التعديل على الحقول وجعلها للقراءة فقط
//    document.getElementById("first-name").readOnly = true;
//    document.getElementById("email").readOnly = true;
//    //document.getElementById("password").readOnly = true;

//    // إخفاء زر "حفظ" و "إلغاء" وعرض زر "تحديث" مرة أخرى
//    saveBtn.style.display = "none";
//    cancelBtn.style.display = "none";
//    updateBtn.style.display = "inline-block";
//});

//function showError(fieldId, message) {
//    const field = document.getElementById(fieldId);
//    const errorMessage = document.createElement("span");
//    errorMessage.className = "error-message";
//    errorMessage.style.color = "red";
//    errorMessage.textContent = message;
//    field.parentNode.appendChild(errorMessage);
//}

//document.getElementById("logout").addEventListener("click", function () {
//    localStorage.removeItem("currentUser"); // Remove user data
//    window.location.href = "../pages/home.html"; // Redirect to home page
//});


 //==================================================================
 //show the password 
//const togglePassword = document.getElementById("toggle-password");
//const passwordField = document.getElementById("password");

//togglePassword.addEventListener("click", function () {
//    // التبديل بين نوع الحقل (password و text)
//    if (passwordField.type === "password") {
//        passwordField.type = "text"; // عرض كلمة المرور
//        togglePassword.classList.remove("fa-eye"); // إزالة الأيقونة الحالية
//        togglePassword.classList.add("fa-eye-slash"); // إضافة الأيقونة الخاصة بالإخفاء
//    } else {
//        passwordField.type = "password"; // إخفاء كلمة المرور
//        togglePassword.classList.remove("fa-eye-slash"); // إزالة الأيقونة الحالية
//        togglePassword.classList.add("fa-eye"); // إضافة الأيقونة الخاصة بالعرض
//    }
//});

// fill the table 
// Sample data (manual data filling)
//const orderHistory = [
//    {
//        title: "Laptop",
//        type: "Electronics",
//        price: 999.99,
//        quantity: 1,
//        totalPrice: 999.99,
//        date: "2023-10-01"
//    },
//    {
//        title: "Smartphone",
//        type: "Electronics",
//        price: 699.99,
//        quantity: 2,
//        totalPrice: 1399.98,
//        date: "2023-10-02"
//    },
//    {
//        title: "Headphones",
//        type: "Accessories",
//        price: 149.99,
//        quantity: 3,
//        totalPrice: 449.97,
//        date: "2023-10-03"
//    },
//    {
//        title: "Book",
//        type: "Books",
//        price: 19.99,
//        quantity: 5,
//        totalPrice: 99.95,
//        date: "2023-10-04"
//    }
//];

 //Function to populate the table with data
//function populateTable(data) {
//    const tbody = document.querySelector("#order-history-table tbody");

//    // Clear existing rows
//    tbody.innerHTML = "";

//    // Add new rows based on the data
//    data.forEach(order => {
//        const row = document.createElement("tr");

//        row.innerHTML = `
//            <td>${order.title}</td>
//            <td>${order.type}</td>
//            <td>$${order.price.toFixed(2)}</td>
//            <td>${order.quantity}</td>
//            <td>$${order.totalPrice.toFixed(2)}</td>
//            <td>${order.date}</td>
//        `;

//        tbody.appendChild(row);
//    });
//}

 //Call the function to populate the table
/*populateTable(orderHistory);*/


// // ==================================================================

//*** */ form database

// CREATE TABLE users (
//     id INT AUTO_INCREMENT PRIMARY KEY,
//     firstName VARCHAR(50),
//     lastName VARCHAR(50),
//     email VARCHAR(100) UNIQUE,
//     password VARCHAR(100)
// );

// const express = require('express');
// const mysql = require('mysql2');
// const bodyParser = require('body-parser');
// const cors = require('cors');

// const app = express();
// app.use(bodyParser.json());
// app.use(cors());

// // إعداد قاعدة البيانات
// const db = mysql.createConnection({
//     host: 'localhost',
//     user: 'root',
//     password: '', // إذا كان لديك كلمة مرور، أضفها هنا
//     database: 'your_database_name' // اسم قاعدة البيانات الخاصة بك
// });

// db.connect((err) => {
//     if (err) throw err;
//     console.log('Connected to the database!');
// });

// // نقطة النهاية لتسجيل المستخدم
// app.post('/api/register', (req, res) => {
//     const { firstName, lastName, email, password } = req.body;

//     const sql = 'INSERT INTO users (firstName, lastName, email, password) VALUES (?, ?, ?, ?)';
//     db.query(sql, [firstName, lastName, email, password], (err, result) => {
//         if (err) return res.status(500).json({ error: err.message });
//         res.status(201).json({ message: 'User registered successfully!' });
//     });
// });

// // نقطة النهاية للحصول على جميع المستخدمين
// app.get('/api/users', (req, res) => {
//     const sql = 'SELECT * FROM users';
//     db.query(sql, (err, results) => {
//         if (err) return res.status(500).json({ error: err.message });
//         res.status(200).json(results);
//     });
// });

// // تشغيل الخادم
// const PORT = 5000;
// app.listen(PORT, () => {
//     console.log(`Server running on port ${PORT}`);
// });

// document.getElementById("register-btn").addEventListener("click", function () {
//     const firstName = document.getElementById("first-name").value.trim();
//     const lastName = document.getElementById("last-name").value.trim();
//     const email = document.getElementById("email").value.trim();
//     const password = document.getElementById("password").value.trim();

//     fetch('http://localhost:5000/api/register', {
//         method: 'POST',
//         headers: {
//             'Content-Type': 'application/json'
//         },
//         body: JSON.stringify({ firstName, lastName, email, password })
//     })
//     .then(response => response.json())
//     .then(data => {
//         alert(data.message); // رسالة نجاح التسجيل
//     })
//     .catch(error => {
//         console.error('Error:', error);
//     });
// });
// function fetchUsers() {
//     fetch('http://localhost:5000/api/users')
//         .then(response => response.json())
//         .then(users => {
//             const userList = document.getElementById("user-list");
//             userList.innerHTML = ''; // تفريغ القائمة

//             users.forEach(user => {
//                 const li = document.createElement("li");
//                 li.textContent = `${user.firstName} ${user.lastName} - ${user.email}`;
//                 userList.appendChild(li);
//             });
//         })
//         .catch(error => {
//             console.error('Error fetching users:', error);
//         });
// }

// // استدعاء الدالة عند تحميل الصفحة
// window.onload = fetchUsers;