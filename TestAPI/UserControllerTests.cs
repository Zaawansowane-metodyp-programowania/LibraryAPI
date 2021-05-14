using FluentAssertions;
using LibraryAPI.Dtos;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;



namespace TestAPI
{
    public class UserControllerTests : BasicTests
    {
        [Fact]
        public async Task CreateUserWithoutAuthorizeShouldBeUnauthorized()
        {

            //Act
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {

                Name = "test",
                Surname = "test",
                Email = "test@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 2
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateUserWithUserAuthorizeShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {

                Name = "test",
                Surname = "test",
                Email = "test@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 2
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateUserWithEmployeeAuthorizeShouldBeForbidden()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {

                Name = "test",
                Surname = "test",
                Email = "test@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 2
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateUserWithRoleWithAdminAuthorizeShouldBeCreated()
        {
            //Arrange
            await AdminAuthorize();

            //Arrange
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {

                Name = "test",
                Surname = "test",
                Email = "test@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 2
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("test", "test", "test", "User123@", "User123@",2)]
        [InlineData("test", "test", "test@example.com", "User123@", "User123",2)]
        [InlineData("test", "test", "test@example.com", "", "User123",2)]
        [InlineData("", "test", "test@example.com", "User123@", "User123",2)]
        [InlineData("test", "", "test@example.com", "User123@", "User123",2)]
        [InlineData("test", "test", "", "User123@", "User123",2)]
        [InlineData("test", "test", "test@example.com", "User123@", "User123@", 10)]
        public async Task CreateUserWithIncorrectDataShouldBeBadRequest(string name, string surnName, string email, string password, string confirmPassword, int role)
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {

                Name = name,
                Surname = surnName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                RoleId = role
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
