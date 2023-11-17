using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class CategoryRepository : ICategoryRepository {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) {
            this._context = context;
        }

        public async Task<bool> CategoryExists(int id) {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }


        public async Task<ICollection<Category>> GetCategories() {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category> GetCategory(int id) {
            return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId) {
            return await _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(c => c.Pokemon).ToListAsync();
        }
        public async Task<bool> CreateCategory(Category category) {
            //Change Tracker
            await _context.AddAsync(category);
            return await Save();
        }

        public async Task<bool> Save() {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
