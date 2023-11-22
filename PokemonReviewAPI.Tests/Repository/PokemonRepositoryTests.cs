using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;
using PokemonReviewAPI.Repository;

namespace PokemonReviewAPI.Tests.Repository {
    public class PokemonRepositoryTests {
        private async Task<DataContext> GetDatabaseContext() {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "PokemonsInMemory")
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Pokemons.CountAsync() <= 0) {
                for (int i = 0; i < 10; i++) {
                    databaseContext.Pokemons.Add(
                      new Pokemon() {
                          Name = "Charizard",
                          BirthDate = new DateTime(1903, 1, 1),
                          PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Fire"}}
                            },
                          Reviews = new List<Review>()
                            {
                                new Review { Title= "Charizard", Text = "Charizard is the best pokemon, because it is Fire", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title= "Charizard",Text = "Charizard is the best a killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title= "Charizard", Text = "Charizard, squirtle, squirtle", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void PokemonRepository_GetPokemon_ReturnsPokemon() {
            //Arrange
            var name = "Charizard";
            var dbContext = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(dbContext);

            //Act
            var result = pokemonRepository.GetPokemon(name);

            // Assert
            result.Should().BeOfType<Task<Pokemon>>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void PokemonRepository_GetPokemonRating_ReturnDecimal() {
            //Arrange
            var id = 1;
            var dbContext = await GetDatabaseContext();
            var pokemonRepository = new PokemonRepository(dbContext);

            //Act
            var result = pokemonRepository.GetPokemonRating(id);

            //Assert
            result.Should().NotBe(0);
            result.Should().BeInRange(1, 5);
        }
    }
}
