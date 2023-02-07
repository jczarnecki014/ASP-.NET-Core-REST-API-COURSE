using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMapingProfile : Profile
    {
        public RestaurantMapingProfile()
        {
            CreateMap<Restaurant,RestaurantDTO>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.address.PostalCode));

            CreateMap<Dish,DishDTO>();

            CreateMap<CreateRestaurantDBO,Restaurant>()
                .ForMember(m=>m.address,c=>c.MapFrom(dbo=> new Address() { City = dbo.City, Street= dbo.Street, PostalCode = dbo.PostalCode}));

            CreateMap<CreateDishDto,Dish>();
            CreateMap<Dish,CreateDishDto>();


            /*CreateMap<UpdateRestaurantDTO,Restaurant>()
                .ForMember(m=>m.name,c=>c.MapFrom(dbo => dbo.name))
                .ForMember(m=>m.description,c=>c.MapFrom(dbo => dbo.description))
                .ForMember(m=>m.hasDelivery,c=>c.MapFrom(dbo => dbo.hasDelivery));*/
        }
    }
}
