using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.DTO;
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

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon) {
            var ownerEntity = await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
            var pokemonOwner = new PokemonOwner() { Owner = ownerEntity, Pokemon = pokemon };
            _context.AddAsync(pokemonOwner);

            var categoryEntity = await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
            var pokemonCategory = new PokemonCategory() { Category = categoryEntity, Pokemon = pokemon };
            _context.AddAsync(pokemonCategory);

            await _context.AddAsync(pokemon);
            return await Save();
        }
        public async Task<bool> UpdatePokemon(int ownerId, int categoryId, int pokeId, Pokemon pokemon) {
            _context.Update(pokemon);
            return await Save();
        }

        public async Task<bool> DeletePokemon(Pokemon pokemon) {
            _context.Remove(pokemon);
            return await Save();
        }
        public async Task<bool> Save() {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Pokemon> GetPokemonTrimToUpper(PokemonDTO pokemon) {
            return await _context.Pokemons.Where(p => p.Name.Trim().ToUpper() == pokemon.Name.Trim().ToUpper()).FirstOrDefaultAsync();
        }
    }
}
