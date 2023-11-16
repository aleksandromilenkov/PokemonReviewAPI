using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller {
        public readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper) {
            _countryRepository = countryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public async Task<IActionResult> GetCountries() {
            List<CountryDTO> countries = _mapper.Map<List<CountryDTO>>(await _countryRepository.GetCountries());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId) {
            if (!await _countryRepository.CountryExists(countryId)) {
                return NotFound();
            }
            CountryDTO country = _mapper.Map<CountryDTO>(await _countryRepository.GetCountry(countryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("owner/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersByCountryId(int countryId) {
            if (!await _countryRepository.CountryExists(countryId)) {
                return NotFound();
            }
            List<Owner> owners = _mapper.Map<List<Owner>>(await _countryRepository.GetOwnersByCountryId(countryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryByOwnderId(int ownerId) {
            CountryDTO country = _mapper.Map<CountryDTO>(await _countryRepository.GetCountryByOwnerId(ownerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }
    }
}
