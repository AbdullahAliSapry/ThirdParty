using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAl.Models;
using Contracts.SharedDtos;
using Bll.Dtos;
using Bll.Dtos.ApiDto;
using Newtonsoft.Json;
namespace Bll.Mapping
{
    public class MappingApplication : Profile
    {

        public MappingApplication()
        {
            CreateMap<CategoryDto, Category>();

            CreateMap<Category, CategoryDto>();


            CreateMap<PersonalViewModel, ApplicationUser>()
                .ForMember(p => p.Email, o => o.MapFrom(src => src.Email))
                .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber + src.PhoneNumberCode))
                .ForMember(p => p.IsMarketer, opt => opt.MapFrom(src => src.IsMarketer));

            // application user
            CreateMap<CompanyViewModel, ApplicationUser>()
             .ForMember(p => p.Email, o => o.MapFrom(src => src.Email))
             .ForMember(p => p.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber + src.PhoneNumberCode))
             .ForMember(p => p.IsComapny, opt => opt.MapFrom(src => src.PhoneNumber + src.IsCompany));



            // applicationuser // 


            CreateMap<ApplicationUser, UserSettingVm>()
                .ForMember(p => p.Email, o => o.MapFrom(e => e.Email))
                .ForMember(p => p.Id, o => o.MapFrom(e => e.Id))
                .ForMember(p => p.Name, o => o.MapFrom(e => e.FullName))
                .ForMember(p => p.PhoneNumber, o => o.MapFrom(e => e.PhoneNumber));


            CreateMap<UserSettingVm, ApplicationUser>()
                         .ForMember(p => p.Email, o => o.MapFrom(e => e.Email))
                         .ForMember(p => p.Id, o => o.MapFrom(e => e.Id))
                         .ForMember(p => p.FullName, o => o.MapFrom(e => e.Name))
                         .ForMember(p => p.PhoneNumber, o => o.MapFrom(e => e.PhoneNumber));



            CreateMap<ProductDto, FavoriteItemDto>()
                .ForMember(p => p.ProductId, o => o.MapFrom(e => e.Id.ToString()))
                .ForMember(p => p.Name, o => o.MapFrom(e => e.Name ?? e.Title))
                .ForMember(p => p.Title, o => o.MapFrom(e => e.Title))
                .ForMember(p => p.Image, o => o.MapFrom(e => e.MainPictureUrl))
                .ForMember(p => p.Price, o => o.MapFrom(e => e.Price.ConvertedPriceList.Internal.Price))
                .ForMember(e => e.VendorId, o => o.MapFrom(e => e.VendorId))
                .ForMember(e => e.VendorName, o => o.MapFrom(e => e.VendorDisplayName))
                .ForMember(e => e.CategoryId, o => o.MapFrom(e => e.CategoryId))
                .ForMember(e => e.Id, o => o.Ignore());


            CreateMap<VendorDto, FavoriteSallerDto>()
                .ForMember(p => p.ProviderType, o => o.MapFrom(e => e.ProviderType))
                .ForMember(p => p.Name, o => o.MapFrom(e => e.Name ?? e.DisplayName))
                .ForMember(p => p.DisplayName, o => o.MapFrom(e => e.DisplayName))
                .ForMember(p => p.DisplayPictureUrl, o => o.MapFrom(e => e.DisplayPictureUrl))
                .ForMember(p => p.NumberOfYear,
                o => o.MapFrom(e => Convert.ToInt32(e.FeaturedValues.FirstOrDefault(l => l.Name == "years").Value ?? "0")))
                .ForMember(e => e.Stars, o => o.MapFrom(e => Convert.ToDouble(e.FeaturedValues.FirstOrDefault(l => l.Name == "start"))))
                .ForMember(e => e.ServiceScore, o => o.MapFrom(e => e.Scores.ServiceScore))
                .ForMember(e => e.DeliveryScore, o => o.MapFrom(e => e.Scores.DeliveryScore))
                .ForMember(e => e.ItemScore, o => o.MapFrom(e => e.Scores.ItemScore))
                .ForMember(e => e.VendorId, o => o.MapFrom(e => e.Id))
                .ForMember(e => e.Id, o => o.Ignore());




            CreateMap<FavoriteItemDto, FavoriteItem>();
            CreateMap<FavoriteItem, FavoriteItemDto>();


            CreateMap<FavoriteSaller, FavoriteSallerDto>();
            CreateMap<FavoriteSallerDto, FavoriteSaller>();



            CreateMap<ReferralCodeUsageDto, ReferralCodeUsage>();
            CreateMap<ReferralCodeUsage, ReferralCodeUsageDto>();


            CreateMap<Proplem, ProplemDto>();
            CreateMap<ProplemDto, Proplem>();


            CreateMap<Cart, CartDto>();
            CreateMap<CartDto, Cart>();

            CreateMap<CartItem, CartItemDto>()
                     .ForMember(p => p.Quntity, o => o.MapFrom(e => e.Quntity))
                     .ForMember(p => p.PhysicalParametersJson, o => o.MapFrom(e => e.PhysicalParametersJson));
            CreateMap<CartItemDto, CartItem>().ForMember(p => p.PhysicalParametersJson, o => o.MapFrom(e => e.PhysicalParametersJson));

            CreateMap<CommissionScheme, CommissionSchemeDto>()
                     .ForMember(p => p.UserType, o => o.MapFrom(e => e.UserType));  
            
            CreateMap<CommissionSchemeDto, CommissionScheme>()
                     .ForMember(p => p.UserType, o => o.MapFrom(e => e.UserType));


            CreateMap<AttributeItemDto, AttributeItem>();
            CreateMap<AttributeItem, AttributeItemDto>();


            CreateMap<MarketerCodeVm, CodesToMarketer>()
                    .ForMember(m => m.DiscountRate, o => o.MapFrom(e => e.DiscountRate))
                    .ForMember(m => m.IsActive, o => o.MapFrom(e => e.IsActive))
                    .ForMember(m => m.RevokedAt, o => o.MapFrom(e => e.RevokedAt));



            CreateMap<CodesToMarketer, MarketerCodeVm>();


            CreateMap<CodesToMarketer, CodesToMarketerDto>();
            CreateMap<CodesToMarketerDto, CodesToMarketer>();


            CreateMap<MarketerAccount, MarketerAccountDto>();
            CreateMap<MarketerAccountDto, MarketerAccount>();


            CreateMap<Marketer, MarketerVm>()

                .ForMember(m => m.CodesToMarketers, o => o.MapFrom(e => e.CodesToMarketers))
                .ForMember(m => m.Id, o => o.MapFrom(e => e.Id))
                .ForMember(m => m.CommissionEarned, o => o.MapFrom(e => e.CommissionEarned))
                .ForMember(m => m.TotalSales, o => o.MapFrom(e => e.TotalSales));



            CreateMap<MarketerVm, Marketer>();


            CreateMap<PayMentManoul, PayMentManoulDto>();
            CreateMap<PayMentManoulDto, PayMentManoul>();


            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            // create map in productdto with itemCart

            CreateMap<ProductDto, CartItemDto>()
                .ForMember(p => p.ProductId, o => o.MapFrom(e => e.Id.ToString()))
                .ForMember(p => p.Name, o => o.MapFrom(e => e.Name ?? e.Title))
                .ForMember(p => p.Title, o => o.MapFrom(e => e.Title))
                .ForMember(p => p.Image, o => o.MapFrom(e => e.MainPictureUrl))
                .ForMember(p => p.PricePerPiece, o => o.MapFrom(e => e.Price.ConvertedPriceList.Internal.Price))
                .ForMember(e => e.FinalPrice, o => o.Ignore())
                .ForMember(e => e.Quntity, o => o.Ignore())
                .ForMember(e => e.Id, o => o.Ignore())
                .ForMember(e => e.AttributeItems, o => o.Ignore())
                .ForMember(e => e.Comment, o => o.MapFrom(e => ""))
                .ForMember(e => e.Description, o => o.MapFrom(e => "Descripion not avlaibe"))
                .ForMember(e => e.PhysicalParameters, o => o.MapFrom(e => JsonConvert.SerializeObject(e.PhysicalParameters)))
                .ForMember(e => e.categoryId, o => o.MapFrom(e => e.CategoryId));





        }
    }
}

