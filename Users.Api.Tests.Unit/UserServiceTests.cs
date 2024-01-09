using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;
using Xunit;

namespace Users.Api.Tests.Unit
{
    public class UserServiceTests
    {
        private UserService _sut;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();
        public UserServiceTests()
        {
            _sut = new UserService(_userRepository, _logger);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>()    
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Test",
                }
            };
            _userRepository.GetAllAsync().Returns(users);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(result, users);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenThatUserDoesNotExist()
        {
            // Arrange
            _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            // Act
            var result = await _sut.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }
    }
}
