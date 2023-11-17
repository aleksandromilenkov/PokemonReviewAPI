using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper) {
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public async Task<IActionResult> GetReviews() {
            List<ReviewDTO> reviews = _mapper.Map<List<ReviewDTO>>(await _reviewRepository.GetReviews());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int reviewId) {
            if (!await _reviewRepository.ReviewExists(reviewId)) {
                return NotFound();
            }
            ReviewDTO review = _mapper.Map<ReviewDTO>(await _reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("reviewer/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewerByReview(int reviewId) {
            Reviewer reviewer = await _reviewRepository.GetReviewerOfAReview(reviewId);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("review/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsByPokemonId(int pokeId) {
            IEnumerable<ReviewDTO> reviews = _mapper.Map<List<ReviewDTO>>(await _reviewRepository.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDTO review) {
            if (review == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Review mappedReview = _mapper.Map<Review>(review);
            if (!await _reviewRepository.CreateReview(reviewerId, pokemonId, mappedReview)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewId, [FromBody] ReviewDTO review) {
            if (review == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (reviewId != review.Id) {
                return BadRequest(ModelState);
            }
            if (!await _reviewRepository.ReviewExists(reviewId)) {
                return NotFound();
            }

            var reviewMap = _mapper.Map<Review>(review);
            if (!await _reviewRepository.UpdateReview(reviewMap)) {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReview(int reviewId) {
            if (!await _reviewRepository.ReviewExists(reviewId)) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Review review = await _reviewRepository.GetReview(reviewId);
            if (!await _reviewRepository.DeleteReview(review)) {
                ModelState.AddModelError("", "Something went wrong with the deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
