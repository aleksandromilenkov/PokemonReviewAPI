using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IReviewerRepository {
        Task<Reviewer> GetReviewer(int reviewerId);
        Task<ICollection<Reviewer>> GetReviewers();
        Task<bool> ReviewExists(int reviewId);
        Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> CreateReviewer(Reviewer reviewer);
        Task<bool> UpdateReviewer(Reviewer reviewer);
        Task<bool> Save();
    }
}
