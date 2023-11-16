using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class ReviewerRepository : IReviewerRepository {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context) {
            this._context = context;
        }

        public async Task<Reviewer> GetReviewer(int reviewerId) {
            return await _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Reviewer>> GetReviewers() {
            return await _context.Reviewers.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId) {
            return await _context.Reviewers.Where(r => r.Id == reviewerId).SelectMany(r => r.Reviews).ToListAsync();
        }

        public Task<bool> ReviewExists(int reviewId) {
            return _context.Reviewers.AnyAsync(r => r.Id == reviewId);
        }
    }
}
