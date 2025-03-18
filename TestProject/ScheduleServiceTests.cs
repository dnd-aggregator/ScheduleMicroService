using Schedule.Application.Abstractions.Persistence.Dbo;
using Schedule.Application.Abstractions.Persistence.Repositories;
using Schedule.Application.Contracts;
using Schedule.Application.Contracts.Requests;
using Schedule.Application.Models;
using Moq;
using Schedule.Application;
using Xunit;
using Assert = Xunit.Assert;

namespace TestProject;

public class ScheduleServiceTests
{
    private readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
    private readonly Mock<IPlayerService> _playerServiceMock;
    private readonly IScheduleService _service;

    public ScheduleServiceTests()
    {
        _scheduleRepositoryMock = new Mock<IScheduleRepository>();
        _playerServiceMock = new Mock<IPlayerService>();
        _service = new ScheduleService(_scheduleRepositoryMock.Object, _playerServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Schedule_And_Return_Id()
    {
        // Arrange
        var request = new CreateScheduleRequest(
            MasterId: 1,
            Location: "TestLocation",
            Date: DateOnly.FromDateTime(DateTime.UtcNow)
        );

        _scheduleRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<ScheduleDbo>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123);

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(123, result);
        _scheduleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ScheduleDbo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Schedule()
    {
        // Arrange
        var expected = new ScheduleModel(
            Id : 1,
            MasterId: 1,
            Location: "TestLocation",
            Date: DateOnly.FromDateTime(DateTime.UtcNow),
            Status: ScheduleStatus.Draft);
            
        _scheduleRepositoryMock
            .Setup(repo => repo.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);
    }
}
