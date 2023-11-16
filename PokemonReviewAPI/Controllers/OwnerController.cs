using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, IPokemonRepository pokemonRepository) {
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
            this._pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public async Task<IActionResult> GetOwners() {
            List<OwnerDTO> owners = _mapper.Map<List<OwnerDTO>>(await _ownerRepository.GetOwners());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwner(int ownerId) {
            if (!await _ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }
            OwnerDTO owner = _mapper.Map<OwnerDTO>(await _ownerRepository.GetOwnerById(ownerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(owner);
        }

        [HttpGet("pokemon/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonsByOwnerId(int ownerId) {
            if (!await _ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }
            List<PokemonDTO> pokemons = _mapper.Map<List<PokemonDTO>>(await _ownerRepository.GetPokemonsByOwnerId(ownerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("owner/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersByPokemonId(int pokemonId) {
            if (!await _pokemonRepository.PokemonExists(pokemonId)) {
                return NotFound();
            }
            List<OwnerDTO> owners = _mapper.Map<List<OwnerDTO>>(await _ownerRepository.GetOwnersByPokemonId(pokemonId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }


    }
}
