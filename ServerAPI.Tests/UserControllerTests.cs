using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServerApi.Data;
using ServerApi.Models;
using ServerAPI.Controllers;
using Xunit;

public class UserControllerTests
{
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task RegisterUser_ReturnsOk_WhenNicknameIsNew()
    {
        // Arrange (준비)
        var context = GetDatabaseContext();
        var controller = new UserController(context);
        var nickname = "NewPlayer";

        // Act (실행)
        var result = await controller.RegisterUser(nickname);

        // Assert (검증)
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(nickname, returnedUser.Nickname);
    }
}