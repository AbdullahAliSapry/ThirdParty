namespace ThirdParty.Utilities
{
    public class Country
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
    public class EmailRequest
    {
        public string Email { get; set; }
    }
    public static class CountryRepository
    {
        public static List<Country> Countries = new List<Country>
        {
            new Country { Code = "+966", Name = "المملكة العربية السعودية" },
            new Country { Code = "+971", Name = "الإمارات العربية المتحدة" },
            new Country { Code = "+974", Name = "قطر" },
            new Country { Code = "+973", Name = "البحرين" },
            new Country { Code = "+968", Name = "عمان" },
            new Country { Code = "+965", Name = "الكويت" },
            new Country { Code = "+20", Name = "مصر" },
            new Country { Code = "+962", Name = "الأردن" },
            new Country { Code = "+961", Name = "لبنان" },
            new Country { Code = "+963", Name = "سوريا" },
            new Country { Code = "+964", Name = "العراق" },
            new Country { Code = "+967", Name = "اليمن" },
            new Country { Code = "+970", Name = "فلسطين" },
            new Country { Code = "+216", Name = "تونس" },
            new Country { Code = "+213", Name = "الجزائر" },
            new Country { Code = "+212", Name = "المغرب" },
            new Country { Code = "+218", Name = "ليبيا" },
            new Country { Code = "+249", Name = "السودان" },
            new Country { Code = "+252", Name = "الصومال" },
            new Country { Code = "+253", Name = "جيبوتي" },
            new Country { Code = "+86", Name = "الصين" },
            new Country { Code = "+1", Name = "الولايات المتحدة" },
            new Country { Code = "+44", Name = "المملكة المتحدة" },
            new Country { Code = "+33", Name = "فرنسا" },
            new Country { Code = "+49", Name = "ألمانيا" },
            new Country { Code = "+39", Name = "إيطاليا" },
            new Country { Code = "+34", Name = "إسبانيا" },
            new Country { Code = "+7", Name = "روسيا" },
            new Country { Code = "+81", Name = "اليابان" },
            new Country { Code = "+82", Name = "كوريا الجنوبية" },
            new Country { Code = "+91", Name = "الهند" },
            new Country { Code = "+92", Name = "باكستان" },
            new Country { Code = "+90", Name = "تركيا" },
            new Country { Code = "+98", Name = "إيران" },
            new Country { Code = "+93", Name = "أفغانستان" },
            new Country { Code = "+60", Name = "ماليزيا" },
            new Country { Code = "+62", Name = "إندونيسيا" },
            new Country { Code = "+63", Name = "الفلبين" },
            new Country { Code = "+66", Name = "تايلاند" },
            new Country { Code = "+84", Name = "فيتنام" },
            new Country { Code = "+880", Name = "بنغلاديش" },
            new Country { Code = "+94", Name = "سريلانكا" },
            new Country { Code = "+95", Name = "ميانمار" },
            new Country { Code = "+977", Name = "نيبال" },
            new Country { Code = "+55", Name = "البرازيل" },
            new Country { Code = "+54", Name = "الأرجنتين" },
            new Country { Code = "+61", Name = "أستراليا" },
            new Country { Code = "+64", Name = "نيوزيلندا" }
        };
    }
}
