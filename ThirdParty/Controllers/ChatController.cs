using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThirdParty.Controllers
{

    [Authorize]
    public class ChatController: Controller
    {
        private IUnitOfWork<ChatMessage> _untiofWork { get; set; }


        public ChatController(IUnitOfWork<ChatMessage> untiofWork)
        {
            _untiofWork = untiofWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin,SubAdmin")]
        // to save in database
        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages =  _untiofWork.Message.GetItems();
            return Json(messages);
        }

        // API لجلب جميع المستخدمين
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users =  _untiofWork.User.GetItems();
            var userEmails = users.Select(u => new { u.Email }).ToList();
            return Json(userEmails);
        }


        [HttpGet("chat/getMessages")]
        public async Task<IActionResult> GetMessages([FromQuery] string senderEmail)
        {
            // البحث عن الرسائل بناءً على الـ senderEmail
            var messages =  _untiofWork.Message.GetItems()
                .Where(m => m.SenderEmail == senderEmail)  // تأكد أن الحقل SenderEmail موجود في جدول الرسائل
                .ToList();

            // إرجاع الرسائل
            return Json(messages);
        }
    }
}
