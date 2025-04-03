using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Web;

namespace ThirdParty.Controllers
{

    public class LanguageController : Controller
    {
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            // التحقق من أن الثقافة مدعومة
            var supportedCultures = new[] { "en-US", "ar" };
            if (!supportedCultures.Contains(culture))
            {
                culture = "en-US";
            }

            // تعيين الثقافة
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // حفظ التفضيل في الكوكيز
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                }
            );

            // إصلاح مشكلة إعادة التوجيه
            if (!string.IsNullOrEmpty(returnUrl))
            {
                try
                {
                    var decodedUrl = Uri.UnescapeDataString(returnUrl);

                    if (Url.IsLocalUrl(decodedUrl))
                    {
                        return LocalRedirect(decodedUrl);
                    }
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
