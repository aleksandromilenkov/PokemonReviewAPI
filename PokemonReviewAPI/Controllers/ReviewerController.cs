using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper) {
            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public async Task<IActionResult> GetReviewers() {
            List<ReviewerDTO> reviewers = _mapper.Map<List<ReviewerDTO>>(await _reviewerRepository.GetReviewers());
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int reviewerId) {
            if (!await _reviewerRepository.ReviewExists(reviewerId)) {
                return NotFound();
            }
            ReviewerDTO reviewer = _mapper.Map<ReviewerDTO>(await _reviewerRepository.GetReviewer(reviewerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsByReviewerId(int reviewerId) {
            if (!await _reviewerRepository.ReviewExists(reviewerId)) {
                return NotFound();
            }
            IEnumerable<ReviewDTO> reviews = _mapper.Map<List<ReviewDTO>>(await _reviewerRepository.GetReviewsByReviewer(reviewerId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDTO reviewer) {
            if (reviewer == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Reviewer mappedReviewer = _mapper.Map<Reviewer>(reviewer);
            if (!await _reviewerRepository.CreateReviewer(mappedReviewer)) {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewer) {
            if (reviewer == null) {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (reviewerId != reviewer.Id) {
                return BadRequest(ModelState);
            }
            if (!await _reviewerRepository.ReviewExists(reviewerId)) {
                return NotFound();
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewer);
            if (!await _reviewerRepository.UpdateReviewer(reviewerMap)) {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId) {
            if (!await _reviewerRepository.ReviewExists(reviewerId)) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            Reviewer reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            if (!await _reviewerRepository.DeleteReviewer(reviewer)) {
                ModelState.AddModelError("", "Something went wrong with the deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
