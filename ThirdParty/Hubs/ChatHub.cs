using DAl;
using DAl.IRepository;
using DAl.Models;
using DAl.Repository;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ThirdParty.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> ConnectedAdmins = new();

        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

        private IUnitOfWork<ChatMessage> _untiofWork { get; set; }

        public ChatHub(IUnitOfWork<ChatMessage> untiofWork)
        {
            _untiofWork = untiofWork;
        }



        // المستخدم العادي يتصل بالشات
        public async Task ConnectUser(string email)
        {
            // Store the connection ID for the user
            ConnectedUsers[email] = Context.ConnectionId;
            Console.WriteLine($"User connected: {email}, ConnectionId: {Context.ConnectionId}");
            await Clients.Caller.SendAsync("Connected", "تم الاتصال بالسيرفر!");
        }

        // الأدمن يدخل الداشبورد
        public async Task ConnectAdmin(string adminId)
        {
            // حفظ الـ ConnectionId للأدمن
            if (!string.IsNullOrEmpty(adminId))
            {
                ConnectedAdmins[adminId] = Context.ConnectionId;
            }
            await Clients.Caller.SendAsync("AdminConnected", "تم الاتصال كأدمن!");
        }

        // عند فصل المستخدم أو الأدمن
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // إزالة الـ ConnectionId عند فصل المستخدم أو الأدمن
            var user = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(user))
            {
                ConnectedUsers.TryRemove(user, out _);
            }

            var admin = ConnectedAdmins.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(admin))
            {
                ConnectedAdmins.TryRemove(admin, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAdmin(string senderEmail, string message)
        {
            // حفظ الرسالة في قاعدة البيانات
            var newMessage = new ChatMessage
            {
                 SenderEmail= senderEmail,
                Message = message,
                ReceiverEmail = "Admin",
                Timestamp= DateTime.UtcNow
            };
            _untiofWork.Message.Create(newMessage);
            await _untiofWork.SaveChangesAsync();

            // إرسال الرسالة للأدمن
            await Clients.All.SendAsync("ReceiveMessage", senderEmail, message);
        }
      
        
        
        public async Task<IEnumerable<ChatMessage>> GetMessages(string senderEmail)
        {
            // جلب الرسائل من قاعدة البيانات
            return  _untiofWork.Message.GetItems()
                .Where(m => m.SenderEmail == senderEmail)
                .OrderBy(m => m.Timestamp)
                .ToList();
        }


        // استرجاع كل الرسائل للأدمن
        // جلب جميع الرسائل
        public async Task<IEnumerable<ChatMessage>> GetAllMessages()
        {
            // جلب جميع الرسائل من قاعدة البيانات
            return  _untiofWork.Message.GetItems().OrderBy(m => m.Timestamp).ToList();
        }

        public async Task SendMessageToUser(string senderEmail, string message)
        {
            Console.WriteLine($"إرسال الرسالة إلى: {senderEmail}");

            // تحقق من وجود المستخدم في الـ ConnectedUsers
            if (ConnectedUsers.ContainsKey(senderEmail))
            {
                await Clients.Client(ConnectedUsers[senderEmail]).SendAsync("ReceiveMessage", "Admin", message);
            }
            else
            {
                Console.WriteLine($"not{senderEmail} Connection");
            }
        }


    }
}
