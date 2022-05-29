using AutoMapper;
using ExamenProject3.Models.Product;

namespace ExamenProject3.Profiles
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductEntity, Product>()
                .ForMember(d => d.ArticleNumber, option => option.MapFrom(s => s.ArticleNr))
                .ForMember(d => d.CategoryName, option => option.MapFrom(s => s.Category.CategoryName));

            CreateMap<Product, ProductEntity>()
                .ForMember(d => d.ArticleNr, option => option.MapFrom(s => s.ArticleNumber));

            CreateMap<ProductUpdate, ProductEntity>();
            CreateMap< ProductEntity,ProductUpdate>();




        }


    }
}
