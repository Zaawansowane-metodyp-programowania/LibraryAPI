﻿using FluentAssertions;
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
        public async Task GetBookWithoutAuthorizeShouldBeUnauthorized()
        {
            //Act
            var response = await _client.GetAsync("/api/books/1");

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

        [Fact]
        public async Task GetBookByCorrrectIdShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task GetBookByIncorrrectIdShouldBeNotFound(int id)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync($"/api/books/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllBooksBorrowedByUserWithEmployeeAuthorizeShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/7");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task GetAllBooksBorrowedByUserWithIncorrectUserIdShouldBeNotFound(int id)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync($"/api/books/user/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task UserCanGetAllBooksBorrowedByYourSelfSoStatusShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UserCantGetAllBooksBorrowedByOtherUserSoStatusShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAllBooksReservedByUserWithEmployeeAuthorizeShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/reservation/4");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task GetAllBooksReservedByUserWithIncorrectUserIdShouldBeNotFound(int id)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync($"/api/books/user/reservation/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UserCantGetAllBooksReservedByOtherUserSoStatusShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/reservation/7");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UserCanGetAllReservedBooksForYourSelfSoStatusShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/books/user/reservation/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }



        [Fact]
        public async Task CreateBookWithEmployeeAuthorizeAndCorrectDataShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PostAsJsonAsync("/api/books/", new CreateBookDto
            {
                ISBN = "921-144-313-12",
                BookName = "otherSurname",
                AuthorName = "test test",
                PublisherName = "tester",
                PublishDate = 2015,
                Category = "Comedy",
                Language = "",
                BookDescription = ""
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("", "test", "test", "test", 5, "test", "test", "test")]
        [InlineData("test", "", "test", "test", 5, "test", "test", "test")]
        [InlineData("test", "test", "", "test", 5, "test", "test", "test")]
        [InlineData("test", "test", "test", "test", 5, "", "test", "test")]
        public async Task CreateBookWithEmptyRequiredFieldShouldBeBadRequest(string isbn, string bookName, string authorName,
            string publisherName, int publishDate, string category, string language, string bookDescription)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PostAsJsonAsync("/api/books/", new CreateBookDto
            {
                ISBN = isbn,
                BookName = bookName,
                AuthorName = authorName,
                PublisherName = publisherName,
                PublishDate = publishDate,
                Category = category,
                Language = language,
                BookDescription = bookDescription

            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateBookWithEmployeeAuthorizeAndCorrectDataShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/books/1", new UpdateBookDto
            {
                ISBN = "921-144-313-12",
                BookName = "otherSurname",
                AuthorName = "test test",
                PublisherName = "tester",
                PublishDate = 2015,
                Category = "Comedy",
                Language = "",
                BookDescription = ""
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("","test","test","test",5,"test","test","test")]
        [InlineData("test", "", "test", "test", 5, "test", "test", "test")]
        [InlineData("test", "test", "", "test", 5, "test", "test", "test")]
        [InlineData("test", "test", "test", "test", 5, "", "test", "test")]
        public async Task UpdateBookWithEmptyRequiredFieldShouldBeBadRequest(string isbn, string bookName, string authorName,
            string publisherName, int publishDate, string category, string language, string bookDescription)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/books/1", new UpdateBookDto
            {
                ISBN = isbn,
                BookName = bookName,
                AuthorName = authorName,
                PublisherName = publisherName,
                PublishDate = publishDate,
                Category = category,
                Language = language,
                BookDescription = bookDescription

            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
