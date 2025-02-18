using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAl.Models;
using Contracts.SharedDtos;
namespace Bll.Mapping
{
    public class MappingApplication: Profile
    {

        public MappingApplication()
        {
            CreateMap<CategoryDto,Category>();

            CreateMap<Category, CategoryDto>();


            CreateMap<PersonalViewModel, ApplicationUser>()
                .ForMember(p=>p.Email,o=>o.MapFrom(src=>src.Email))
                .ForMember(p=>p.PhoneNumber,opt=>opt.MapFrom(src=>src.PhoneNumber + src.PhoneNumberCode))
                .ForMember(p=>p.IsMarketer,opt=>opt.MapFrom(src=>src.IsMarketer));



        }
    }
}

