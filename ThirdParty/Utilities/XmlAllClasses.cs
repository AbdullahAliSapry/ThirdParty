namespace ThirdParty.Utilities
{
    public class XmlAllClasses
    {
    }

    public class RatingListItemSearchParameters
    {
        public int CategoryId { get; set; }
        public string ItemRatingType { get; set; }
        public string StuffStatus { get; set; }
    }



    public class SearchItemsParameters
    {

        public string CategoryId { get; set; }
        public string VendorName { get; set; }
        public string VendorId { get; set; }
        public string ItemTitle { get; set; } 
        public string LanguageOfQuery { get; set; } 
        public string MinPrice { get; set; } 
        public string MaxPrice { get; set; }
        public string MinVolume { get; set; } 
        public string MaxVolume { get; set; } 
        public string MinVendorRating { get; set; } 
        public string MaxVendorRating { get; set; } 
        public string BrandId { get; set; } 
        public List<ConfiguratorDto> Configurators { get; set; }
        public string OrderBy { get; set; } 
        public string OutputMode { get; set; } 
        public string StuffStatus { get; set; } 
        public string CategoryMode { get; set; } 
        public string IsTmall { get; set; }

        public string? ImageUrl { get; set; } 
    }

    public class ConfiguratorDto
    {
        public string Pid { get; set; } 
        public string Vid { get; set; }  
    }


}
