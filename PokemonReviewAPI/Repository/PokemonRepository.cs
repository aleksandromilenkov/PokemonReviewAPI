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
    }
}
