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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId) {
            IEnumerable<PokemonDTO> pokemons = _mapper.Map<List<PokemonDTO>>(await _categoryRepository.GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryCreate) {
            if (categoryCreate == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var categories = await _categoryRepository.GetCategories();
            var category = categories.Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (category != null) {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!await _categoryRepository.CreateCategory(categoryMap)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory([FromQuery] int categoryId, [FromBody] CategoryDTO category) {
            if (category == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (categoryId != category.Id) {
                return BadRequest(ModelState);
            }
            if (!await _categoryRepository.CategoryExists(categoryId)) {
                return NotFound();
            }

            var categoryMap = _mapper.Map<Category>(category);
            if (!await _categoryRepository.UpdateCategory(categoryMap)) {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
    }
}
