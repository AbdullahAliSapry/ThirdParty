using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class ProductDto: ProductBaseDto
    {
        public string ErrorCode { get; set; }
        public bool HasError { get; set; }
        public string ProviderType { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CategoryId { get; set; }
        public string ExternalCategoryId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorDisplayName { get; set; }
        public int VendorScore { get; set; }
        public string Description { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string TaobaoItemUrl { get; set; }
        public string ExternalItemUrl { get; set; }
        public string MainPictureUrl { get; set; }
        public string StuffStatus { get; set; }
        public int Volume { get; set; }
        public int MasterQuantity { get; set; }
        public List<PictureDto> Pictures { get; set; }
        public Location Location { get; set; }
        public List<FeaturedValue> FeaturedValues { get; set; }
        public bool IsSellAllowed { get; set; }
        public PhysicalParameters PhysicalParameters { get; set; }
        public bool IsFiltered { get; set; }
        public IEnumerable<Videos> Videos { get; set; }
    }


    public class ProductBaseDto
    {

        public string Name { get; set; }
        public PriceDto Price { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsTitleManuallyTranslated { get; set; }
        public string OriginalTitle { get; set; }

    }

    public class Videos
    {
        public string Url { get; set; }
        public string PreviewUrl { get; set; }
    }
    public class PhysicalParameters
    {
        public decimal Weight { get; set; }
    }

    public class FeaturedValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Location
    {
        public string State { get; set; }
    }
    public class PriceDto
    {
        public decimal OriginalPrice { get; set; }
        public decimal MarginPrice { get; set; }
        public string OriginalCurrencyCode { get; set; }
        public ConvertedPriceListDto ConvertedPriceList { get; set; }
        public string ConvertedPrice { get; set; }
        public string ConvertedPriceWithoutSign { get; set; }
        public string CurrencySign { get; set; }
        public string CurrencyName { get; set; }
        public bool IsDeliverable { get; set; }
        public DeliveryPriceDto DeliveryPrice { get; set; }
        public DeliveryPriceDto OneItemDeliveryPrice { get; set; }
        public PriceWithoutDeliveryDto PriceWithoutDelivery { get; set; }
        public PriceWithoutDeliveryDto OneItemPriceWithoutDelivery { get; set; }
    }

    public class ConvertedPriceListDto
    {
        public InternalPriceDto Internal { get; set; }
        public List<DisplayedMoneyDto> DisplayedMoneys { get; set; }
    }

    public class InternalPriceDto
    {
        public decimal Price { get; set; }
        public string Sign { get; set; }
        public string Code { get; set; }
    }

    public class DisplayedMoneyDto
    {
        public decimal Price { get; set; }
        public string Sign { get; set; }
        public string Code { get; set; }
    }

    public class DeliveryPriceDto
    {
        public decimal OriginalPrice { get; set; }
        public decimal MarginPrice { get; set; }
        public string OriginalCurrencyCode { get; set; }
        public ConvertedPriceListDto ConvertedPriceList { get; set; }
    }

    public class PriceWithoutDeliveryDto
    {
        public decimal OriginalPrice { get; set; }
        public decimal MarginPrice { get; set; }
        public string OriginalCurrencyCode { get; set; }
        public ConvertedPriceListDto ConvertedPriceList { get; set; }
    }

    public class PictureDto
    {
        public string Url { get; set; }
        public SmallPictureDto Small { get; set; }
        public MediumPictureDto Medium { get; set; }
        public LargePictureDto Large { get; set; }
        public bool IsMain { get; set; }
    }

    public class SmallPictureDto
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class MediumPictureDto
    {
        public string Url { get; set; }
    }

    public class LargePictureDto
    {
        public string Url { get; set; }
    }

    public class RelatedGroup
    {

        public string Type { get; set; }

        public string DispalyName { get; set; }

        public string OriginalName { get; set; }

        public IEnumerable<ProductDtoRelated> Items { get; set; }

    }


    public class ProductDtoRelated : ProductBaseDto
    {
        public PictureDto Image { get; set; }

    }
}
