using FluentAssertions;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestAPI
{
    public class AccountControllerTests : BasicTests
    {
        [Fact]
        public async Task RegisterWithCorrectDataShouldBeOK()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/api/account/register", new RegisterUserDto
            {
                Name = "test",
                Surname = "test",
                Email = "test@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("test", "test","test","User123@", "User123@")]
        [InlineData("test", "test", "test@example.com", "User123@", "User123")]
        [InlineData("test", "test", "test@example.com", "", "User123")]
        [InlineData("", "test", "test@example.com", "User123@", "User123")]
        [InlineData("test", "", "test@example.com", "User123@", "User123")]
        [InlineData("test", "test", "", "User123@", "User123")]
        public async Task RegisterWithIncorrectDataShouldBeBadRequest(string name, string surnName, string email, string password, string confirmPassword)
        {
            //Act
            var response = await _client.PostAsJsonAsync("/api/account/register", new RegisterUserDto
            {
                Name = name,
                Surname = surnName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            });
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginWithCorrectDataShoulbBeOk()
        {
            //Act
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
            {
                Email = "userTest@example.com",
                Password = "User123@"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "User123@")]
        [InlineData("userTest@example.com", "")]
        [InlineData("userTest", "User")]
        [InlineData("userTest@example.com", "User")]
        [InlineData("user@example.com", "User123@")]
        public async Task LoginWithIncorrectDataShouldBeBadRequest(string email, string password)
        {
            //Act
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
            {
                Email = email,
                Password = password
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


    }
}
