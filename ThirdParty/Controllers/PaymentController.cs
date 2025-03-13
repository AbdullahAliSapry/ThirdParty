using AutoMapper;
using Bll.Dtos;
using CloudinaryDotNet.Actions;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.FileUploadService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Security.Claims;
using ThirdParty.ViewModels;
using Twilio.TwiML.Messaging;
using static Infrastructure.FileUploadService.FileUploadService;

namespace ThirdParty.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {

        private IUnitOfWork<PayMentManoul> _unitOfWork { get; set; }

        private readonly ILogger<PaymentController> _logger;
        private IFileUploadService _uploadService;

        private IMapper _mapper;
        public PaymentController(IUnitOfWork<PayMentManoul> unitOfWork, ILogger<PaymentController> logger, IFileUploadService uploadService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _uploadService = uploadService;
            _mapper = mapper;
        }
        [Authorize]
        public async Task<IActionResult> Index(string userId, string orderId)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return Forbid();

            if (!Guid.TryParse(orderId, out Guid OrderId))
                return View("Error", new ErrorViewModel
                {
                    Message = "Error In Order Id",
                    RequestId = "401"
                });

            var accounts = _unitOfWork.Accounts.GetItemsWithFunc(e => e.IsActive);

            ViewData["Accounts"] = accounts;
            var order = await _unitOfWork.Order.GetItemWithFunc(e => e.Id == OrderId && e.IsPrivewed);

            if (order == null)
                return View("Error", new ErrorViewModel
                {
                    Message = "Order Not Found",
                    RequestId = "404"
                });

            ViewData["Order"] = order;


            return View();
        }


        // add payments
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmManualPayment(PayMentManoulDto payMentManoulDto)
        {



            var accounts = _unitOfWork.Accounts.GetItemsWithFunc(e => e.IsActive);

            ViewData["Accounts"] = accounts;
            var order = await _unitOfWork.Order.GetItemWithFunc(e => e.Id == payMentManoulDto.OrderId && e.IsPrivewed);
            if (order == null)
                return View("Error", new ErrorViewModel
                {
                    Message = "Order Not Found",
                    RequestId = "404"
                });

            ViewData["Order"] = order;

            if (!ModelState.IsValid)
                return View("Index", ModelState);

            // 
            var checkpaymentidexist = await _unitOfWork.PayMentManoul.GetItemWithFunc(e => e.OrderId == order.Id);
            if (checkpaymentidexist != null)
            {
                TempData["SuccessMessage"] = "لقد تم دفع   هذا الطلب فعليا";
                return View("Index");
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != payMentManoulDto.UserId)
                return Forbid();

            var result = await _uploadService.UploadFile(payMentManoulDto.ReceiptImage, "PayMentImages", 10);

            if (result == null)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل الصورة.";
                return View("Index");
            }


            // create new file upload

            var fileupload = new FileUploads()
            {
                FileType = Path.GetExtension(payMentManoulDto.ReceiptImage.FileName),
                FileName = result.FileName,
                FilePath = result.FilePath,
            };


            _unitOfWork.FileUploads.Create(fileupload);


            _unitOfWork.SaveChanges();


            var payment = _mapper.Map<PayMentManoul>(payMentManoulDto);

            payment.FileUploads = fileupload;

            var resultpa = _unitOfWork.PayMentManoul.Create(payment);
            if (!resultpa.Status)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البينات .";
                return View("Index");
            }

            var resultsaving = _unitOfWork.SaveChanges();
            if (!resultsaving)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ البينات .";
                return View("Index");
            }

            TempData["SuccessMessage"] = "تم ارسال البيانات بنجاح";

            // update order  
            order.IsPaid = true;
            _unitOfWork.Order.UpdateItem(order);
            _unitOfWork.SaveChanges();

            // return to action
            return RedirectToAction("Index", "PayMent", new { userId = payMentManoulDto.UserId, orderId = payMentManoulDto.OrderId });

        }


        // review payment

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReviewPayMent(string userid, string paymentId)
        {


            if (!Guid.TryParse(paymentId, out Guid PayMentId))
                return View("Error", new ErrorViewModel() { Message = "Invalid Id", RequestId = "401" });

            var payment = await _unitOfWork.PayMentManoul.GetItemWithFunc(e => e.Id == PayMentId);
            if (payment.IsConfirmed)
            {
                payment.IsConfirmed = false;
                _unitOfWork.PayMentManoul.UpdateItem(payment);

                _unitOfWork.SaveChanges();
                TempData["SuccessMessage"] = "status changed successfully";
                return RedirectToAction("AllPaymnets", "Admin");

            }
            payment.IsConfirmed = true;

            _unitOfWork.PayMentManoul.UpdateItem(payment);

            _unitOfWork.SaveChanges();
            TempData["SuccessMessage"] = "status changed successfully";
            return RedirectToAction("AllPaymnets", "Admin");

        }



        // not use any where
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPatch("/PayMent/GetAllPayMentsToUser/{userId}")]
        public async Task<IActionResult> GetAllPayMentsToUser(string userId)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return Forbid();

            var payments = _unitOfWork.PayMentManoul
                .GetItemsWithFunc(e => e.UserId == userId && e.IsConfirmed, new string[] { "FileUploads" });


            return View(payments);
        }


    }
}
