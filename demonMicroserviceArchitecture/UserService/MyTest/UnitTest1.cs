using System.Linq.Expressions;
using Azure.Core;
using Moq;
using MockQueryable.Moq; 
using UserService.Models.DTOs;
using UserService.Models.Enities;
using UserService.Repositories.Interfaces;
using UserService.Services.Implements;
using UserService.Services.Interfaces;
using MockQueryable;
using System.Data.Entity;
using UserService.Models.Requests;
using Microsoft.Extensions.Configuration;

using NUnit;
using Microsoft.AspNetCore.Identity.Data;

namespace MyTest
{
    public class UserService_Test
    {
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<ITokenRepository> _mockTokenRepo;
        private Mock<IConfiguration> _mockConfig;
        private UserServices _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockTokenRepo = new Mock<ITokenRepository>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(config => config["Auth:Key"]).Returns("c906743b-161a-42b3-a8d0-b18cb0e0ae5f");
            _mockConfig.Setup(config => config["Auth:Issuer"]).Returns("http://localhost:5190");
            _mockConfig.Setup(config => config["Auth:Audience"]).Returns("gateway_api");
            _mockConfig.Setup(config => config["Auth:ExpiredAccessToken"]).Returns("10800");
            _mockConfig.Setup(config => config["Auth:ExpiredRefreshToken"]).Returns("30600");
            _userService = new UserServices(
                _mockUserRepo.Object,
                _mockConfig.Object,
                _mockTokenRepo.Object
            );
        }


        // test login
        [Test]
        public async Task Login_ReturnsSuccess()
        {
            var fakeData = new User
            {
                Id = Guid.NewGuid(),
                Name = "baobao",
                Email = "baobao@email.com",
                Password = "12345"
            };
            var users = new List<User>
            {
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "aliceadffasffasfd@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1afdsfasds@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alicefasdfsf2@email.com",
                    Password = "12345"
                },  new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "alice@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alice2@email.com",
                    Password = "12345"
                }, fakeData
            };


            SignInRequest loginForm = new SignInRequest
            {
                Email = fakeData.Email,
                Password = fakeData.Password
            };

            var mockUserQueryable = users.BuildMock();
            _mockUserRepo
                .Setup(r => r.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(
                   ( Expression<Func<User, bool>> predicate) => mockUserQueryable.Where(predicate)
                );


            _mockTokenRepo.Setup(r => r.Insert(It.IsAny<Token>()));

            var result = await _userService.Login(loginForm);


            Assert.IsNotNull(result, "Response should not be null");
            Assert.AreEqual(200, result.StatusCode, "Status code should be 200");
            Assert.AreEqual("Success", result.Message, "Message should be 'Success'");
            Assert.IsNotNull(result.Data, "LoginResponse data should not be null");
            Assert.AreEqual(fakeData.Name, result.Data.Name, "User name should match");
            Assert.IsNotEmpty(result.Data.AccessToken, "Access token should not be empty");
            Assert.IsNotEmpty(result.Data.RefreshToken, "Refresh token should not be empty");
        }

        [Test]
        public async Task LoginWithManyRequests_ReturnsSuccess()
        {
            var fakeData = new User
            {
                Id = Guid.NewGuid(),
                Name = "baobao",
                Email = "baobao@email.com",
                Password = "12345"
            };
            var users = new List<User>
            {
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "aliceadffasffasfd@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1afdsfasds@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alicefasdfsf2@email.com",
                    Password = "12345"
                },  new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "alice@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alice2@email.com",
                    Password = "12345"
                }, fakeData
            };


            SignInRequest loginForm = new SignInRequest
            {
                Email = fakeData.Email,
                Password = fakeData.Password
            };

            var mockUserQueryable = users.BuildMock();
            _mockUserRepo
                .Setup(r => r.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(
                   (Expression<Func<User, bool>> predicate) => mockUserQueryable.Where(predicate)
                );


            _mockTokenRepo.Setup(r => r.Insert(It.IsAny<Token>()));
            var tasks = Enumerable.Range(0, 1000)
            .Select(_ => _userService.Login(loginForm))
            .ToArray();
            var results = await Task.WhenAll(tasks);
            foreach (var result in results)
            {
                Assert.AreEqual(200, result.StatusCode);
                Assert.AreEqual("Success", result.Message);
                Assert.IsNotNull(result.Data);
                Assert.AreEqual(fakeData.Name, result.Data.Name);
            }

        }

       
        [Test]
        public async Task CreateUser_ReturnsSuccess()
        {
            var users = new List<User>
            {
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "aliceadffasffasfd@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1afdsfasds@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alicefasdfsf2@email.com",
                    Password = "12345"
                }
            };


            var mockUserQueryable = users.BuildMock();

          

            var fakeData = new UserDTO
            {
                Email = "baovantruong@gmail.com",
                Name = "jlfajsfl"
            };

            _mockUserRepo
                .Setup(r => r.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) =>
                        mockUserQueryable.Where(predicate));
            _mockUserRepo.Setup(r => r.Insert(It.IsAny<User>()));

            var data = await _userService.Create(fakeData);



            Assert.AreEqual(200, data.StatusCode);
            Assert.AreEqual(fakeData.Name, data.Data.Name);
            Assert.AreEqual(fakeData.Email, data.Data.Email);
        }


        [Test]
        public async Task GetAllUser_ReturnsSuccess() {
            var users = new List<User>
            {
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice",
                    Email = "alice@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice1",
                    Email = "alice1@email.com",
                    Password = "12345"
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alice2@email.com",
                    Password = "12345"
                },
            };

            var mockUserQueryable = users.BuildMock();
            _mockUserRepo.Setup(r => r.GetQueryable()).Returns(mockUserQueryable.AsQueryable());

            var result = await _userService.GetAll();

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetById_UserExists_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var fakeUser = new User { Id = userId, Name = "Alice", Email = "alice@email.com", Password="12345" };

            var users = new List<User> 
            { 
                fakeUser, 
                new User { 
                    Id =  Guid.NewGuid(), 
                    Name = "Alice1", 
                    Email = "alice1@email.com", 
                    Password = "12345" 
                },
                new User {
                    Id =  Guid.NewGuid(),
                    Name = "Alice2",
                    Email = "alice2@email.com",
                    Password = "12345"
                },
            }.AsQueryable();

            var mockUserQueryable = users.BuildMock(); // Tạo mock hỗ trợ async

            _mockUserRepo.Setup(r => r.GetQueryable(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(mockUserQueryable);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(fakeUser.Name, result.Data.Name);
            Assert.AreEqual(fakeUser.Email, result.Data.Email);
        }


    }
}