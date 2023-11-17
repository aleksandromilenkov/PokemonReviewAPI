using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller {
        public readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper) {
            _pokemonRepository = pokemonRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetPokemons() {
            List<PokemonDTO> pokemons = _mapper.Map<List<PokemonDTO>>(await _pokemonRepository.GetAll());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(int pokeId) {
            if (!await _pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }
            PokemonDTO pokemon = _mapper.Map<PokemonDTO>(await _pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRating(int pokeId) {
            if (!await _pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemon) {
            if (pokemon == null) {
                return BadRequest(ModelState);
            }
            var pokemons = await _pokemonRepository.GetAll();
            var existingPokemon = pokemons.Where(c => c.Name.Trim().ToUpper() == pokemon.Name.Trim().ToUpper()).FirstOrDefault();
            if (existingPokemon != null) {
                ModelState.AddModelError("", "This pokemon already exists");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Pokemon mappedPokemon = _mapper.Map<Pokemon>(pokemon);
            if (!await _pokemonRepository.CreatePokemon(ownerId, categoryId, mappedPokemon)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
    }
}
