const adminConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

// طلب إذن من المستخدم لعرض الإشعارات عند تحميل الصفحة
if (Notification.permission !== "granted") {
    Notification.requestPermission().then(permission => {
        if (permission === "granted") {
            console.log("تم منح الإذن للإشعارات.");
        } else {
            console.log("إذن الإشعار مرفوض.");
            alert("لم يتم منح إذن الإشعارات. لتفعيل الإشعارات، يجب عليك السماح بها في إعدادات المتصفح.");
        }
    });
} else {
    console.log("تم منح الإذن للإشعارات.");
}


// الاتصال كأدمن
async function connectAdmin() {
    await adminConnection.start();  // الاتصال بـ SignalR
    console.log("تم الاتصال بنجاح");
}

// دالة لعرض إشعار منبثق
function showNotification(sender, message) {
    if (Notification.permission === "granted") {
        const notification = new Notification(`رسالة جديدة من ${sender}`, {
            body: message,
            icon: 'your-icon-url',  // أيقونة للإشعار
        });

        notification.onclick = () => {
            window.focus();
        };
    }
}

// استلام الرسائل من أي مستخدم
adminConnection.on("ReceiveMessage", (senderEmail, message) => {
    if (document.hidden) {
        // إذا كانت الصفحة غير نشطة، اعرض إشعار
        showNotification(senderEmail, message);
    } else {
        console.log("Enter");
        toastr.success("تم استلام اشعار جديد");
        // إذا كانت الصفحة نشطة، اعرض الرسالة في واجهة المستخدم
        displayMessage(senderEmail, message);
    }
});

// عرض الرسائل في الصفحة
function displayMessage(sender, message) {
    const messagesList = document?.getElementById("messagesList");
    const listItem = document.createElement("li");
    listItem.textContent = `${sender}: ${message}`;
    messagesList?.appendChild(listItem);
}

// إرسال الرد
async function sendResponse() {
    const responseMessage = document.getElementById("responseMessage").value;
    const selectedUserEmail = document.getElementById("userSelect").value;

    if (responseMessage.trim() !== "" && selectedUserEmail !== "") {
        try {
            await adminConnection.invoke("SendMessageToUser", selectedUserEmail, responseMessage);
            console.log(`تم إرسال الرد إلى ${selectedUserEmail}: ${responseMessage}`);
        } catch (error) {
            console.error("حدث خطأ أثناء إرسال الرد:", error);
        }
    } else {
        alert("يرجى اختيار مستخدم وكتابة رد.");
    }
}

window.onload = async function () {
    await connectAdmin();  // الاتصال أولاً

    // جلب جميع الرسائل من السيرفر
    const messagesResponse = await fetch('/chat/getAllMessages');
    if (!messagesResponse.ok) {
        console.error("خطأ في جلب الرسائل");
        return;
    }

    const messages = await messagesResponse.json();
    messages.forEach(msg => {
        displayMessage(msg.senderEmail, msg.message);
    });

    // جلب قائمة بكل المستخدمين (يمكن تعديلها بناءً على النظام)
    const usersResponse = await fetch('/chat/getUsers');
    if (!usersResponse.ok) {
        console.error("خطأ في جلب المستخدمين");
        return;
    }

    const users = await usersResponse.json();
    const userSelect = document.getElementById("userSelect");

    if (users.length === 0) {
        console.log("لا يوجد مستخدمين في النظام.");
        return;
    }

    users.forEach(user => {
        const option = document.createElement("option");
        option.value = user.email;
        option.textContent = user.email;
        userSelect?.appendChild(option);
    });
};
