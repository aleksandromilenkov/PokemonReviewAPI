using AutoMapper;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            CreateMap<Pokemon, PokemonDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Reviewer, ReviewerDTO>().ReverseMap();
        }
    }
}
