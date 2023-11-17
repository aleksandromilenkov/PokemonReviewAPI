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

        public async Task<Country> GetCountryByOwnerId(int ownerId) {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwners() {
            return await _context.Owners.ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersByPokemonId(int pokemonId) {
            return await _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).Select(po => po.Owner).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByOwnerId(int ownerId) {
            PokemonOwner pokemonOwner = _context.PokemonOwners.Where(po => po.OwnerId == ownerId).FirstOrDefault();
            return await _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId) {
            return await _context.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> CreateOwner(Owner owner) {
            await _context.AddAsync(owner);
            return await Save();
        }
        public async Task<bool> UpdateOwner(Owner owner) {
            _context.Update(owner);
            return await Save();
        }

        public async Task<bool> DeleteOwner(Owner owner) {
            _context.Remove(owner);
            return await Save();
        }
        public async Task<bool> Save() {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

    }
}
