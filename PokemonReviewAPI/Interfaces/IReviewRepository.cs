using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IReviewRepository {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int id);
        Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<Reviewer> GetReviewerOfAReview(int reviewId);
        Task<bool> ReviewExists(int id);

    }
}
