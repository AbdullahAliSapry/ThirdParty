using AutoMapper;
using Bll.Dtos;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using DAl.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Polly;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using ThirdParty.Utilities;
using ThirdParty.ViewModels;
using Twilio.Rest.Trunking.V1;

namespace ThirdParty.Controllers
{

    public class UserSettingController : Controller
    {

        public IUnitOfWork<ApplicationUser> _unitOfWork { get; set; }
        public IAuthRepository _authRepository { get; set; }
        private IMapper _mapper { get; set; }

        public UserSettingController(IUnitOfWork<ApplicationUser> unitOfWork, IMapper mapper, IAuthRepository authRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authRepository = authRepository;
        }

        [Authorize]
        public async Task<IActionResult> SettingUser(string userid)
        {
            try
            {
                if (string.IsNullOrEmpty(userid) || !Guid.TryParse(userid, out Guid ValidId))
                {
                    var error = new ErrorViewModel
                    {
                        RequestId = "400",
                        Message = "Invalid User Id. Please ensure the Id is correct."
                    };
                    return View("Error", error);
                }


                var user = await _unitOfWork.User.GetItem(userid);



                var uservm = _mapper.Map<ApplicationUser, UserSettingVm>(user);



                return View(uservm);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = "500", Message = ex.Message });
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserSettingVm userSettingVm)
        {
            if (!ModelState.IsValid)
            {
                return View("SettingUser", userSettingVm);
            }

            var user = await _unitOfWork.User.GetItem(userSettingVm.Id);

            if (user == null)
            {
                return View("Error", new ErrorViewModel { RequestId = "404", Message = "User not found" });
            }

            bool isDataChanged =
                user.Email != userSettingVm.Email ||
                user.FullName != userSettingVm.Name ||
                user.PhoneNumber != userSettingVm.PhoneNumber;


            if (!isDataChanged)
            {
                TempData["ErrorUpdate"] = "لم يتم اي تغيير في البينات";
                return RedirectToAction("SettingUser", new { userid = userSettingVm.Id });
            }


            if (user.Email != userSettingVm.Email)
            {
                bool emailExist = _unitOfWork.User.GetItems().Any(x => x.Email == userSettingVm.Email && x.Id != userSettingVm.Id);

                if (emailExist)
                {
                    userSettingVm.Email = user.Email;
                    ModelState.AddModelError("Email", "Email already exists");
                    return View("SettingUser", userSettingVm);
                }
            }

            // ✅ Map new values to the existing user entity
            _mapper.Map(userSettingVm, user);

            var resultUpdating = _unitOfWork.User.UpdateItem(user);
            if (!resultUpdating)
            {
                return View("Error", new ErrorViewModel { RequestId = "500", Message = "An error occurred while updating user" });
            }

            var result = await _unitOfWork.SaveChangesAsync();

            if (!result)
            {
                TempData["ErrorUpdate"] = "An error occurred while updating user";
            }
            else
            {



                var resultLogin = await _authRepository.UpdateUserNameClaimAsync(user, userSettingVm.Name, ClaimTypes.Name);

                if (!resultLogin)
                {
                    TempData["ErrorUpdate"] = "An error occurred while updating user";
                    return RedirectToAction("SettingUser", new { userid = userSettingVm.Id });
                }

                TempData["SuccessUpdate"] = "User updated successfully";
            }

            return RedirectToAction("SettingUser", new { userid = userSettingVm.Id });
        }



        [Authorize]
        public async Task<IActionResult> GetFavoutits(string userId)
        {


            try
            {


                if (!Guid.TryParse(userId, out Guid Id))
                {
                    return View("Error",
                        new ErrorViewModel
                        {
                            RequestId = "400",
                            Message = "Invalid User Id. Please ensure the Id is correct."
                        });
                }

                if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userId)
                {
                    return View("AccessDenied",
                        new AccessDeniedViewModel
                        {
                            StatusMessage = "403",
                            Message = "You are not authorized to view this page"
                        });

                }

                var user = await _unitOfWork.
                    User.GetItem(userId, new string[] { "Favorite" });
                if (user.Favorite == null)
                {
                    user.Favorite = new Favorite();
                    _unitOfWork.User.UpdateItem(user);
                    _unitOfWork.SaveChanges();
                }

                var favorite = await _unitOfWork.Favorite
                        .GetItemWithFunc(e => e.Id == user.Favorite.Id, new string[] { "FavoriteItems", "FavoriteSallers" });

                var favouritsitems = _mapper.Map<List<FavoriteItemDto>>(favorite.FavoriteItems);
                ViewData["NumberOfProduct"]= favouritsitems.Count;
                ViewData["NumberOfSaller"]= favorite.FavoriteSallers.Count;

                var model = new FavvouritViewTypes();

                model.FavoriteItems = favouritsitems;

                return View(model);
            }
            catch (Exception ex)
            {

                return View("Error", new ErrorViewModel { RequestId = "500", Message = "Error in get Data" });
            }
        }


        public class UpdateFavoriteRequest
        {
            public List<UpdateFavorite> UpdatedFavoriteItems { get; set; }

            public string UserId { get; set; }
        }


        public class UpdateFavorite
        {
            public int Id { get; set; }


            public int newQuntity { get; set; }


        }




        [Authorize]
        [HttpPost("/UserSetting/UpdateFavoriteCart")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFavoriteCart([FromBody] UpdateFavoriteRequest updateRequest)
        {
            Console.WriteLine("Enter in method new");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != updateRequest.UserId)
            {
                return Json(new { success = false, message = "Unauthorized request" });
            }

            var favorite = await _unitOfWork.Favorite.GetItemWithFunc(e => e.UserId == userId, new string[] { "FavoriteItems" });

            if (favorite == null)
            {
                return Json(new { success = false, message = "Favorites not found" });
            }

            try
            {
                foreach (var item in favorite.FavoriteItems)
                {
                    var updatedItem = updateRequest.UpdatedFavoriteItems.FirstOrDefault(i => i.Id == item.Id);
                    if (updatedItem != null)
                    {
                        item.Quntity = updatedItem.newQuntity;
                    }
                }

                bool result = _unitOfWork.Favorite.UpdateItem(favorite);

                if (result)
                {
                    _unitOfWork.SaveChanges();
                    return Json(new { success = true, message = "تم حفظ البيانات بنجاح" });
                }
                else
                {
                    return Json(new { success = false, message = "لم يتم حفظ البيانات" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء الحفظ: " + ex.Message });
            }
        }



        public class DeleteFavClass
        {
            public List<int> Ids { get; set; }
            public bool DeleteAll { get; set; }

            public string UserId { get; set; }
        }

        [Authorize]
        [HttpPost("/UserSetting/DeleteFromFav")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFromFav([FromBody] DeleteFavClass deleteFavClass)
        {
            Console.WriteLine("Enter in delete method");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(deleteFavClass.UserId);
            if (userId != deleteFavClass.UserId)
            {


                return StatusCode(403, new
                {
                    Success = false,
                    Message = "acess denied please insure your dataa",
                });
            }

            var favouriteCart = await _unitOfWork.Favorite.GetItemWithFunc(e => e.UserId == userId);

            if (favouriteCart == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Not Found Cart. Please ensure your data is correct.",
                    RequestId = "401"
                });
            }

            bool deleteResult;

            if (deleteFavClass.DeleteAll)
            {
                deleteResult = await _unitOfWork.FavoriteItems.DeleteRangeAsync(e => e.FavoriteId == favouriteCart.Id);
            }
            else
            {
                deleteResult = await _unitOfWork.FavoriteItems.DeleteRangeAsync(e => deleteFavClass.Ids.Contains(e.Id));
            }

            if (deleteResult)
            {
                await _unitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Items deleted successfully.",
                    UserId = userId
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Failed to delete items. Please try again later.",
                    RequestId = "401"
                });
            }
        }


        [Authorize]
        public async Task<IActionResult> DeleleteSaller(string userId, int sallerId, string VendorId)
        {

            try
            {
                var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                if (UserId != userId)
                {

                    return Forbid();

                }
                if (sallerId == 0 || string.IsNullOrEmpty(VendorId))
                {
                    return RedirectToAction("GetFavoutits", new { userId });
                }

                var resultdelet = await _unitOfWork.FavoriteSallers.DeleteItemWithFunc(e => e.Id == sallerId && e.VendorId == VendorId);


                if (resultdelet)
                {
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("GetFavoutits", new { userId });

                }
                else
                {
                    return RedirectToAction("GetFavoutits", new { userId });
                }

            }
            catch (Exception ex)
            {


                Console.WriteLine(ex.Message);
                return View("Error", new ErrorViewModel { Message = "Error In Delete", RequestId = "401" });
            }


        }



        [Authorize]
        public IActionResult GetAllPaymentstouser(string userid)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userid)
                return Forbid();

            var payments = _unitOfWork.PayMentManoul
                .GetItemsWithFunc(e => e.UserId == userid, new string[] { "FileUploads" });

            var allpaymetAwait = payments.Where(e=>!e.IsConfirmed).Sum(e=>e.Amount);
            ViewBag.allpaymetwait = allpaymetAwait;
            var allpaymentconfirmed = payments.Where(e => e.IsConfirmed).Sum(e=>e.Amount);
            ViewBag.paymentconfirmed = allpaymentconfirmed;

            return View(payments);
        }




 



    }
}
