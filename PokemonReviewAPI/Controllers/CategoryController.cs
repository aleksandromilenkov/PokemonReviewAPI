using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller {
        public readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) {
            _categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public async Task<IActionResult> GetCategories() {
            List<CategoryDTO> categories = _mapper.Map<List<CategoryDTO>>(await _categoryRepository.GetCategories());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }


        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId) {
            if (!await _categoryRepository.CategoryExists(categoryId)) {
                return NotFound();
            }
            CategoryDTO category = _mapper.Map<CategoryDTO>(await _categoryRepository.GetCategory(categoryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId) {
            IEnumerable<PokemonDTO> pokemons = _mapper.Map<List<PokemonDTO>>(await _categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
    }
}
