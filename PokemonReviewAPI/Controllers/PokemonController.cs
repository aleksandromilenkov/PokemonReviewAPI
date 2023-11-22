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
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IOwnerRepository ownerRepository, IReviewRepository reviewRepository, IMapper mapper) {
            _pokemonRepository = pokemonRepository;
            this._ownerRepository = ownerRepository;
            this._reviewRepository = reviewRepository;
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
            var existingPokemon = _pokemonRepository.GetPokemonTrimToUpper(pokemon);
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

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemon) {
            if (pokemon == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (pokeId != pokemon.Id) {
                return BadRequest(ModelState);
            }
            if (!await _pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!await _pokemonRepository.UpdatePokemon(ownerId, categoryId, pokeId, pokemonMap)) {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokeId) {
            if (!await _pokemonRepository.PokemonExists(pokeId)) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var reviews = await _reviewRepository.GetReviewsOfAPokemon(pokeId);
            Pokemon pokemon = await _pokemonRepository.GetPokemon(pokeId);
            if (!await _reviewRepository.DeleteReviews(reviews.ToList())) {
                ModelState.AddModelError("", "Something went wrong with the deleting");
                return StatusCode(500, ModelState);
            }
            if (!await _pokemonRepository.DeletePokemon(pokemon)) {
                ModelState.AddModelError("", "Something went wrong with the deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
