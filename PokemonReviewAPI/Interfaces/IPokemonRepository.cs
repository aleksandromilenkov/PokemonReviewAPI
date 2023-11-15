using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IPokemonRepository {
        Task<ICollection<Pokemon>> GetAll();

    }
}
