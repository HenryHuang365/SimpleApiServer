using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleApiServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace SimpleApiServer.Tests.Controllers;

[TestClass]
public class UsersControllerTests
{
    private Mock<IUserService> _mockUserService = null!;
    private UsersController _controller = null!;
    private IMemoryCache _cache = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object, _cache);
    }

    [TestMethod]
    public async Task GetAll_ReturnsOkWithUsers()
    {
        // Arrange
        var users = new List<UserProfileDto>
        {
            new() { Id = new Guid(), Name = "Alice" },
            new() { Id = new Guid(), Name = "Bob" }
        };

        _mockUserService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedUsers = okResult.Value as IEnumerable<UserProfileDto>;
        Assert.IsNotNull(returnedUsers);
        Assert.AreEqual(2, returnedUsers.Count());
    }

    [TestMethod]
    public async Task GetUserById_ReturnsOkWithUser()
    {
        var userId = new Guid();
        // Arrange
        var user = new UserProfileDto { Id = userId, Name = "Charlie" };

        _mockUserService.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedUser = okResult.Value as UserProfileDto;
        Assert.IsNotNull(returnedUser);
        Assert.AreEqual("Charlie", returnedUser.Name);
    }

    [TestMethod]
    public async Task GetUserById_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new NotFoundException("User not found"));

        // Act
        var result = await _controller.GetUserById(new Guid());

        // Assert
        var notFound = result as NotFoundObjectResult;
        Assert.IsNotNull(notFound);
        Assert.AreEqual(404, notFound.StatusCode);

        var error = notFound.Value as ErrorResponseDto;
        Assert.IsNotNull(error);
        Assert.AreEqual("User not found", error.Message);
    }
}
