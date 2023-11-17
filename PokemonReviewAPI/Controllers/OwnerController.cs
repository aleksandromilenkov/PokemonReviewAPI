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
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, IPokemonRepository pokemonRepository, ICountryRepository countryRepository) {
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
            this._pokemonRepository = pokemonRepository;
            this._countryRepository = countryRepository;
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO owner) {
            if (owner == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Owner mappedOwner = _mapper.Map<Owner>(owner);
            mappedOwner.Country = await _countryRepository.GetCountry(countryId);
            if (!await _ownerRepository.CreateOwner(mappedOwner)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDTO owner) {
            if (owner == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (ownerId != owner.Id) {
                return BadRequest(ModelState);
            }
            if (!await _ownerRepository.OwnerExists(ownerId)) {
                return NotFound();
            }

            var ownerMap = _mapper.Map<Owner>(owner);
            if (!await _ownerRepository.UpdateOwner(ownerMap)) {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
    }
}
