using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class PokemonRepository : IPokemonRepository {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context) {
            _context = context;
        }

        public async Task<ICollection<Pokemon>> GetAll() {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> GetPokemon(int id) {
            return await _context.Pokemons.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pokemon> GetPokemon(string name) {
            Pokemon pokemon = await _context.Pokemons.Where(p => p.Name == name).FirstOrDefaultAsync();
            return pokemon;
        }

        public decimal GetPokemonRating(int id) {
            IEnumerable<Review> review = _context.Reviews.Where(p => p.Pokemon.Id == id);
            if (review.Count() <= 0) {
                return 0;
            }
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public Task<bool> PokemonExists(int pokeId) {
            return _context.Pokemons.AnyAsync(p => p.Id == pokeId);
        }
    }
}
