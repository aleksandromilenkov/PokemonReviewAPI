﻿using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface ICategoryRepository {
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        Task<bool> CategoryExists(int id);
    }
}