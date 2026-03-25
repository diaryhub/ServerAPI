using Microsoft.AspNetCore.Mvc;
using Moq;
using ServerAPI.Controllers;
using ServerAPI.DTOs.Request;
using ServerAPI.DTOs.Response;
using ServerAPI.Services.Interfaces;
using Xunit;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();

    [Fact]
    public async Task RegisterUser_ReturnsOk_WhenEmailIsNew()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Nickname = "TestUser",
            Email = "test@example.com",
            Password = "password123"
        };
        var responseDto = new UserResponseDto { Id = 1, Nickname = request.Nickname, Email = request.Email };
        _userServiceMock.Setup(s => s.RegisterUserAsync(request)).ReturnsAsync(responseDto);

        var controller = new UserController(_userServiceMock.Object);

        // Act
        var result = await controller.RegisterUser(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserResponseDto>(okResult.Value);
        Assert.Equal(request.Email, returnedUser.Email);
    }

    [Fact]
    public async Task RegisterUser_ReturnsBadRequest_WhenEmailIsDuplicated()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Nickname = "TestUser",
            Email = "duplicate@example.com",
            Password = "password123"
        };
        _userServiceMock.Setup(s => s.RegisterUserAsync(request)).ReturnsAsync((UserResponseDto?)null);

        var controller = new UserController(_userServiceMock.Object);

        // Act
        var result = await controller.RegisterUser(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CheckEmail_ReturnsTaken_WhenEmailExists()
    {
        // Arrange
        var email = "exists@example.com";
        _userServiceMock.Setup(s => s.IsEmailTakenAsync(email)).ReturnsAsync(true);

        var controller = new UserController(_userServiceMock.Object);

        // Act
        var result = await controller.CheckEmail(email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = okResult.Value!;
        var taken = (bool)value.GetType().GetProperty("taken")!.GetValue(value)!;
        Assert.True(taken);
    }
}
