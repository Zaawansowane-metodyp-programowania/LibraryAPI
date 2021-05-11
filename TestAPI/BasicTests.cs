using LibraryAPI;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace TestAPI
{
    public class BasicTests
    {
        protected readonly HttpClient _client;


        public BasicTests()
        {
            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.Single(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<LibraryDBContext>));

                        services.Remove(descriptor);



                        services.AddDbContext<LibraryDBContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDatabaseForTesting");
                        });

                        services.AddScoped<SeederForTest>();
                        services.AddScoped<PasswordHasher<User>>();

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<LibraryDBContext>();
                            var seeder = scopedServices.GetRequiredService<SeederForTest>();
                            var logger = scopedServices
                                .GetRequiredService<ILogger<BasicTests>>();

                            db.Database.EnsureCreated();

                            try
                            {
                                seeder.Seed();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "An error occurred seeding the " +
                                                    "database with test messages. Error: {Message}", ex.Message);
                            }
                        }
                    });
                });

            _client = factory.CreateClient();
        }
        protected async Task AuthenticateAdmin()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAdmin());
        }
        protected async Task AuthenticateUser()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtUser());
        }
        protected async Task AuthenticateEmployee()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtEmployee());
        }

        private async Task<string> GetJwtAdmin()
        {
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
            {
                Email = "adminTest@example.com",
                Password = "User123@"
            });

            var loginVm = await response.Content.ReadAsAsync<LoginVm>();
            return loginVm.Token;
        }
        private async Task<string> GetJwtUser()
        {
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
            {
                Email = "userTest@example.com",
                Password = "User123@"
            });

            var loginVm = await response.Content.ReadAsAsync<LoginVm>();
            return loginVm.Token;
        }

        private async Task<string> GetJwtEmployee()
        {
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDto
            {
                Email = "employeeTest@example.com",
                Password = "User123@"
            });

            var loginVm = await response.Content.ReadAsAsync<LoginVm>();
            return loginVm.Token;
        }
    }

}
