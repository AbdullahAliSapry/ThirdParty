const userConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

const userEmail = document.getElementById("userIdChat").value;

// الاتصال كمستخدم

async function connectUser() {
    await userConnection.start();  // Connect to SignalR
    console.log("تم الاتصال بنجاح");
    await userConnection.invoke("ConnectUser", userEmail);  // Pass the user email
}


// استلام الرسائل من الأدمن
userConnection.on("ReceiveMessage", (sender, message) => {
    console.log(`💬 رسالة جديدة من ${sender}: ${message}`);
    displayMessage(sender, message);
})

// عرض الرسائل في صفحة المستخدم
function displayMessage(sender, message) {
    const messagesList = document.getElementById("userMessagesList");
    const listItem = document.createElement("li");
    listItem.textContent = `${sender.trim() === userEmail.trim() ? 'You' : 'Admin'}: ${message}`;
    messagesList.appendChild(listItem);
}

// إرسال رسالة إلى الأدمن
async function sendUserMessage() {
    const userMessage = document.getElementById("userMessage").value;

    if (userMessage.trim() !== "") {
        try {
            await userConnection.invoke("SendMessageToAdmin", userEmail, userMessage);
            console.log(`تم إرسال الرسالة إلى الأدمن: ${userMessage}`);
        } catch (error) {
            console.error("حدث خطأ أثناء إرسال الرسالة:", error);
        }
    } else {
        alert("يرجى كتابة رسالة.");
    }
}

// جلب الرسائل السابقة من الأدمن عند تحميل الصفحة
window.onload = async function () {
    await connectUser();  // الاتصال أولاً

    // جلب الرسائل القديمة الخاصة بالمستخدم
    const response = await fetch(`/chat/getMessages?senderEmail=${userEmail}`);
    const messages = await response.json();
    messages.forEach(msg => {
        displayMessage(msg.senderEmail, msg.message);
    });
};