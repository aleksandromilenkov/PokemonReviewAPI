using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using PokemonReviewAPI.Controllers;
using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Interfaces;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Tests.Controllers {

    public class PokemonCtonrollerTests {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonCtonrollerTests() {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnOK() {
            //Arrange
            var pokemons = A.Fake<ICollection<Pokemon>>();
            var pokemonList = A.Fake<List<PokemonDTO>>();
            A.CallTo(() => _mapper.Map<List<PokemonDTO>>(pokemons)).Returns(pokemonList);
            var controller = new PokemonController(_pokemonRepository, null, _reviewRepository, _mapper);
            //Act
            var result = controller.GetPokemons();
            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void PokemonController_CreatePokemon_ReturnOK() {

            // Arrange
            int ownerId = 1;
            int categoryId = 1;
            var pokemonDTO = A.Fake<PokemonDTO>();
            var pokemons = A.Fake<ICollection<PokemonDTO>>();
            var pokemon = A.Fake<Pokemon>();
            var pokemonList = A.Fake<List<PokemonDTO>>();
            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonDTO)).Returns(pokemon);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonDTO)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, null, _reviewRepository, _mapper);

            // Act
            var result = controller.CreatePokemon(ownerId, categoryId, pokemonDTO);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
