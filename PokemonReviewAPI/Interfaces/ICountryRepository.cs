﻿using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface ICountryRepository {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int countryId);
        Task<IEnumerable<Owner>> GetOwnersByCountryId(int countryId);
        Task<Country> GetCountryByOwnerId(int ownerId);
        Task<bool> CountryExists(int countryId);
    }
}
