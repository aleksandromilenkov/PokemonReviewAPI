﻿using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Interfaces {
    public interface IPokemonRepository {

        // Icollection is for list, feed of a objects..
        Task<ICollection<Pokemon>> GetAll();
        //The next 2 is going to be when the user clicks and goes into (poedinecen pokemon)
        Task<Pokemon> GetPokemon(int id);
        Task<Pokemon> GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        Task<bool> PokemonExists(int pokeId);
        Task<Pokemon> GetPokemonTrimToUpper(PokemonDTO pokemon);
        Task<bool> CreatePokemon(int onwerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemon(int onwerId, int categoryId, int pokeId, Pokemon pokemon);
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> Save();
    }
}
