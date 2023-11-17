using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IOwnerRepository {
        Task<Owner> GetOwnerById(int ownerId);
        Task<Country> GetCountryByOwnerId(int ownerId);
        Task<ICollection<Owner>> GetOwners();

        Task<ICollection<Pokemon>> GetPokemonsByOwnerId(int ownerId);
        Task<ICollection<Owner>> GetOwnersByPokemonId(int pokemonId);
        Task<bool> OwnerExists(int ownerId);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> UpdateOwner(Owner owner);
        Task<bool> Save();

    }
}
