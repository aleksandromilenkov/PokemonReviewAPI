﻿
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository {
    public class CountryRepository : ICountryRepository {
        private readonly DataContext _context;
        public CountryRepository(DataContext context) {
            _context = context;
        }

        public async Task<bool> CountryExists(int countryId) {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }


        public async Task<ICollection<Country>> GetCountries() {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountry(int countryId) {
            return await _context.Countries.Where(c => c.Id == countryId).FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByOwnerId(int ownerId) {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Owner>> GetOwnersByCountryId(int countryId) {
            return await _context.Countries.Where(c => c.Id == countryId).SelectMany(c => c.Owners).ToListAsync();
        }

        public async Task<bool> CreateCountry(Country country) {
            await _context.AddAsync(country);
            return await Save();
        }

        public async Task<bool> UpdateCountry(Country country) {
            _context.Update(country);
            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country) {
            _context.Remove(country);
            return await Save();
        }
        public async Task<bool> Save() {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
