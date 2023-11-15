using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller {
        public readonly IPokemonRepository _pokemonRepository;
        public PokemonController(IPokemonRepository pokemonRepository) {
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetPokemons() {
            var pokemons = await _pokemonRepository.GetAll();
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
    }
}
