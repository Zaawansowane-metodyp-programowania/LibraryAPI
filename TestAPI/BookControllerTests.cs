using FluentAssertions;
using LibraryAPI.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
        [InlineData("", "test", "test", "test", 5, "test", "test", "test")]
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

        [Fact]
        public async Task BorrowBookWithEmployeeAuthorizeShouldBeOK()
        {   //Arrange
            await EmployeeAuthorize();

            //Act
            var borrow = new BorrowBookDto()
            {
                UserId = 7
            };
            var borrowJson = JsonConvert.SerializeObject(borrow);
            var response = await _client.PatchAsync(
                "/api/books/borrow/2",
                new StringContent(borrowJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task BorrowBorrowedBookShouldBeBadRequest()
        {   //Arrange
            await EmployeeAuthorize();

            //Act
            var borrow = new BorrowBookDto()
            {
                UserId = 7
            };
            var borrowJson = JsonConvert.SerializeObject(borrow);
            var response = await _client.PatchAsync(
                "/api/books/borrow/1",
                new StringContent(borrowJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BorrowBookForUserWhoIsntFirstOnReservationListShouldBeBadRequest()
        {   //Arrange
            await EmployeeAuthorize();

            //Act
            var borrow = new BorrowBookDto()
            {
                UserId = 3
            };
            var borrowJson = JsonConvert.SerializeObject(borrow);
            var response = await _client.PatchAsync(
                "/api/books/borrow/8",
                new StringContent(borrowJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task BorrowBookWithIncorrectBookIdShouldBeNotFound(int id)
        {   //Arrange
            await EmployeeAuthorize();

            //Act
            var borrow = new BorrowBookDto()
            {
                UserId = 7
            };
            var borrowJson = JsonConvert.SerializeObject(borrow);
            var response = await _client.PatchAsync(
                $"/api/books/borrow/{id}",
                new StringContent(borrowJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task BorrowBookWithIncorrectUserIdShouldBeNotFound()
        {   //Arrange
            await EmployeeAuthorize();

            //Act
            var borrow = new BorrowBookDto()
            {
                UserId = 0
            };
            var borrowJson = JsonConvert.SerializeObject(borrow);
            var response = await _client.PatchAsync(
                "/api/books/borrow/2",
                new StringContent(borrowJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ReturnBookFromUserWithEmployeeAuthorizeShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();
            
            //Act
            var response = await _client.PatchAsync("/api/books/return/1", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task ReturnBookFromOtherUserWithUserAuthorizeShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.PatchAsync("/api/books/return/1", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task ReturnBookNotOnLoanShouldBeBadrequest()
        {

            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PatchAsync("/api/books/return/2", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async Task ReturnBookWithIncorrectIdShouldBeNotFound(int id)
        {

            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PatchAsync($"/api/books/return/{id}", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ReserveBookShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.PatchAsync("/api/books/reservation/1", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        public async Task ReserveBookWithIncorrectBookIdShouldBeNotFound(int id)
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.PatchAsync($"/api/books/reservation/{id}", null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteBookWithEmployeeAuthorizheShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/books/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteBookWithUserAuthorizheShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/books/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task DeleteBookWithIncorrectIdShouldbBeNoFound(int id)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.DeleteAsync($"/api/books/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        public async Task DeleteReserveForBookWithIncorrectBookId(int id)
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.DeleteAsync($"/api/books/reservation/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteReserveForBookShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/books/reservation/4");
                
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteReserveForBookForWhichWeDontHaveReservationShouldBeNotFound()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/books/reservation/2");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


    }

}

