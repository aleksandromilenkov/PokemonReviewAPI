using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class OwnerRepository : IOwnerRepository {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context) {
            this._context = context;
        }
        public async Task<Owner> GetOwnerById(int ownerId) {
            return await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwners() {
            return await _context.Owners.ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByOwnerId(int ownerId) {
            return await _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId) {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }
    }
}
