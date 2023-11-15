using AutoMapper;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            CreateMap<Pokemon, PokemonDTO>();
        }
    }
}
