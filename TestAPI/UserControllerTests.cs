﻿using FluentAssertions;
using LibraryAPI.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;



namespace TestAPI
{
    public class UserControllerTests : BasicTests
    {

        [Fact]
        public async Task GetAllUsersWithEmployeeAuthorizeShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetAllUsersWithUserAuthorizeShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAllUsersWithAdminAuthorizeShouldBeOK()
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserByCorrectIdWithEmployeeAuthorizeShouldBeOK()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        public async Task GetUserByInCorrectIdShouldBeNotFound(int id)
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.GetAsync($"/api/users/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UserCantGetOtherUserByIdSoStatusShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UserCanGetYourselveSoStatusShouldBeOK()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.GetAsync("/api/users/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

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
                RoleId = 1
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

                Name = "tester",
                Surname = "tester",
                Email = "tester@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 1
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateUserWithAdminAuthorizeShouldBeCreated()
        {
            //Arrange
            await AdminAuthorize();

            //Arrange
            var response = await _client.PostAsJsonAsync("/api/users", new CreateUserDto
            {
                Name = "testt",
                Surname = "testt",
                Email = "testt@example.com",
                Password = "User123@",
                ConfirmPassword = "User123@",
                RoleId = 2
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("test", "test", "test", "User123@", "User123@", 2)]
        [InlineData("test", "test", "test@example.com", "User123@", "User123", 2)]
        [InlineData("test", "test", "test@example.com", "", "User123", 2)]
        [InlineData("", "test", "test@example.com", "User123@", "User123", 2)]
        [InlineData("test", "", "test@example.com", "User123@", "User123", 2)]
        [InlineData("test", "test", "", "User123@", "User123", 2)]
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

        [Fact]
        public async Task UpdateOtherUserWithAdminAuthorizeAndCorrectDataShouldBeOK()
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/users/4", new UpdateUserDto
            {
                Name = "otherName",
                Surname = "otherSurname",
                Email = "otherEmail@example.com"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateOtherUserWithEmployeeAuthorizeShouldBeForbidden()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/users/4", new UpdateUserDto
            {
                Name = "otherName",
                Surname = "otherSurname",
                Email = "otherEmail@example.com"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        [Fact]
        public async Task UpdateYourselfShouldBeOK()
        {
            //Arrange
            await User3Authorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/users/7", new UpdateUserDto
            {
                Name = "otherName",
                Surname = "otherSurname",
                Email = "otherEmail@example.com"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateOtherUserWithUserAuthorizeShouldBeForbidden()
        {
            //Arrange
            await UserAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/users/1", new UpdateUserDto
            {
                Name = "otherName",
                Surname = "otherSurname",
                Email = "otherEmail@example.com"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData("TestName", "TestSurname", "testexample.com")]
        public async Task UpdateUserWithIncorrectEmailShouldBeBadRequest(string name, string surname, string email)
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.PutAsJsonAsync("/api/users/1", new UpdateUserDto
            {
                Name = name,
                Surname = surname,
                Email = email
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task UpdateRoleIdWithAdminAuthorizeShouldBeOK()
        {   //Arrange
            await AdminAuthorize();

            //Act
            var role = new UpdateUserRoleDto()
            {
                RoleId = 2
            };
            var roleJson = JsonConvert.SerializeObject(role);
            var response = await _client.PatchAsync(
                "/api/users/role/7",
                new StringContent(roleJson, Encoding.UTF8, "application/json")); ;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUserWithAdminAuthorizeShouldBeNoContent()
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/users/5");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteYourselfShouldBeNoContent()
        {
            //Arrange
            await User2Authorize();

            //Act
            var response = await _client.DeleteAsync("/api/users/6");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUserWithEmployeeAuthorizeShouldBeForbidden()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/users/5");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteUserWithUserAuthorizeShouldBeForbidden()
        {
            //Arrange
            await EmployeeAuthorize();

            //Act
            var response = await _client.DeleteAsync("/api/users/5");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task DeleteUserWithIncorrectIdShouldBeNotFound(int id)
        {
            //Arrange
            await AdminAuthorize();

            //Act
            var response = await _client.DeleteAsync($"/api/users/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
