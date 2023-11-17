using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class ReviewRepository : IReviewRepository {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context) {
            this._context = context;
        }
        public async Task<Review> GetReview(int id) {
            return await _context.Reviews.Where(r => r.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Reviewer> GetReviewerOfAReview(int reviewId) {
            return await _context.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviews() {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId) {
            return await _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();
        }

        public async Task<bool> ReviewExists(int id) {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> CreateReview(int reviewerId, int pokemonId, Review review) {
            Reviewer reviewer = await _context.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefaultAsync();
            Pokemon pokemon = await _context.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefaultAsync();
            review.Reviewer = reviewer;
            review.Pokemon = pokemon;
            await _context.AddAsync(review);
            return await Save();
        }
        public async Task<bool> Save() {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReview(Review review) {
            _context.Update(review);
            return await Save();
        }
    }
}
