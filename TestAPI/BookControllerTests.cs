using FluentAssertions;
using LibraryAPI.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestAPI
{
    public class BookControllerTests : BasicTests
    {
        [Fact]
        public async Task GetAllBooksWithoutAuthorizeShouldBeUnauthorized()
        {
            //Act
            var response = await _client.GetAsync("/api/books");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        private static Dictionary<string, string> Dictionary(string searchPhrase, int pageNumber, int pageSize, string sortBy,
         string sortDirection)
        {
            return new Dictionary<string, string>
            {
                ["SearchPhrase"] = searchPhrase,
                ["PageNumber"] = pageNumber.ToString(),
                ["PageSize"] = pageSize.ToString(),
                ["SortBy"] = sortBy,
                ["SortDirection"] = sortDirection
            };
        }

        [Theory]
        [InlineData("python", 1, 5, "BookName", "asc")]
        [InlineData("", 1, 15, "Category", "desc")]
        public async Task GetAllBooksWithAuthorize(string searchPhrase,
            int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            //Arrange
            await UserAuthorize();
            var query = Dictionary(searchPhrase, pageNumber, pageSize, sortBy, sortDirection);

            //Act
            var response = await _client.GetAsync(QueryHelpers.AddQueryString("/api/books", query));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("python", 1, 5, "BookName", "")]
        [InlineData("", 5, 30, "Category", "desc")]
        [InlineData("", 1, 15, "Category", "dqd")]
        [InlineData("", 1, 15, "dqq", "asc")]
        public async Task GetAllBooksWithIncorretQueryShouldBeBadRequest(string searchPhrase,
            int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            //Arrange
            await UserAuthorize();
            var query = Dictionary(searchPhrase, pageNumber, pageSize, sortBy, sortDirection);

            //Act
            var response = await _client.GetAsync(QueryHelpers.AddQueryString("/api/books", query));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
