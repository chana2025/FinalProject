using AutoMapper;
using Common.Dto;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
namespace Service
{
    public class MyMapper : Profile
    {
        string imagesFolderPath = Path.Combine(Environment.CurrentDirectory, "Images");

        public MyMapper()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
                    File.Exists(Path.Combine(imagesFolderPath, src.ImageUrl))
                        ? File.ReadAllBytes(Path.Combine(imagesFolderPath, src.ImageUrl))
                        : null));

            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.FileImage != null ? src.FileImage.FileName : null));

            CreateMap<DietType, DietDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<WeeklyTracking, WeeklyTrackingDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CustomerFoodPreference, FoodPreferencesDto>().ReverseMap();
        }
    }

}
