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
    }
}
