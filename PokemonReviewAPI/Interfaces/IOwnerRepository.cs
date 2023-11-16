using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IOwnerRepository {
        Task<Owner> GetOwnerById(int ownerId);
        Task<ICollection<Owner>> GetOwners();

        Task<ICollection<Pokemon>> GetPokemonsByOwnerId(int ownerId);
        Task<ICollection<Owner>> GetOwnersByPokemonId(int pokemonId);
        Task<bool> OwnerExists(int ownerId);

    }
}
