using System.Linq.Expressions;
using Azure.Core;
using Castle.Core.Configuration;
using Moq;
using MockQueryable.Moq; // Quan trọng!
using UserService.Models.DTOs;
using UserService.Models.Enities;
using UserService.Repositories.Interfaces;
using UserService.Services.Implements;
using UserService.Services.Interfaces;
using MockQueryable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyTest
{
    public class UserService_Test
    {
        private Mock<IUserRepository> _mockUserRepo;
        private UserServices _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
           

            _userService = new UserServices(
                _mockUserRepo.Object,
                null,
                null
            );
        }


        // test login

        //-----------
        [Test]
        public async Task CreateUser_ReturnsSuccess()
        {
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
            }.AsQueryable();


            var mockUserQueryable = users.BuildMock();

          

            var fakeData = new UserDTO
            {
                Email = "baobao@gmail.com",
                Name = "jlfajsfl"
            };

            _mockUserRepo
                .Setup(r => r.Query(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(mockUserQueryable);
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
            }.AsQueryable();

            var mockUserQueryable = users.BuildMock();
            _mockUserRepo.Setup(r => r.Query()).Returns(mockUserQueryable);

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

            _mockUserRepo.Setup(r => r.Query(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(mockUserQueryable);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(fakeUser.Name, result.Data.Name);
            Assert.AreEqual(fakeUser.Email, result.Data.Email);
            Assert.AreEqual(userId, result.Data.Id);
        }


    }
}