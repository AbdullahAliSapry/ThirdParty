using Contracts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class ProductDto : ProductBaseDto
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

        public ICollection<AttributeProduct> Attributes { get; set; }

        public ICollection<RelatedGroup> RelatedGroups { get; set; } 

        public List<ConfiguredItems> ConfiguredItems { get; set; }


    }

              
    public class ConfiguredItems
    {

        public string Id { get; set; }

        public string Quantity { get; set; }

        public List<ConfiguratorItem> Configurators { get; set; }

        public PriceDto Price { get; set; }


    }
    public class ConfiguratorItem
    {
        public string Pid { get; set; }

        public string Vid { get; set; }
    }


    public class AttributeProduct
    {

        public string Pid { get; set; }

        public string Vid { get; set; }

        public string PropertyName { get; set; }

        public string Value { get; set; }

        public string OriginalPropertyName { get; set; }

        public string OriginalValue { get; set; }


        public bool IsConfigurator {  get; set; }

        public string? ImageUrl { get; set; }


    }


    public class VendorDto
    {
        public string Id { get; set; }
        public string ProviderType { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ShopName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public string DisplayPictureUrl { get; set; }
        public Location Location { get; set; }
        public CreditDto Credit { get; set; }
        public ScoresDto Scores { get; set; }
        public List<FeaturedValue> FeaturedValues { get; set; }
    }


    public class ProviderReviews
    {



        public List<ProductReviewItemDto> Content { get; set; }

    }

    public class ProductReviewItemDto
    {
        public string ExternalId { get; set; }
        public string ItemId { get; set; }
        public string ConfigurationId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserNick { get; set; }
        public int Rating { get; set; }
        public List<string> Images { get; set; } = new();
        public List<FeaturedValue> FeaturedValues { get; set; } = new();
    }

    public class VendorItemDto
    {
        public List<ProductDto> Content { get; set; }
        public int TotalCount { get; set; }
    }

    public class CreditDto
    {
        public int Level { get; set; }
        public int Score { get; set; }
        public int TotalFeedbacks { get; set; }
        public int PositiveFeedbacks { get; set; }
    }

    public class ScoresDto
    {
        public double DeliveryScore { get; set; }
        public double ItemScore { get; set; }
        public double ServiceScore { get; set; }
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
