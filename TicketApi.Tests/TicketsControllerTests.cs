using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketApi;

namespace TicketApi.Tests;

public class TicketsControllerTests
{
	private static TicketDbContext CreateContext(string dbName)
	{
		var options = new DbContextOptionsBuilder<TicketDbContext>()
			.UseInMemoryDatabase(dbName)
			.Options;
		return new TicketDbContext(options);
	}

	[Fact]
	public async Task GetById_ReturnsNotFound_WhenTicketDoesNotExist()
	{
		//Arrange
		var context = CreateContext(Guid.NewGuid().ToString());
		var controller = new TicketsController(context);

		//Act
		var result = await controller.GetById(99);

		//Assert
		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Create_SavesTicketToDatabase()
	{
		//Arrange
		string dbName = Guid.NewGuid().ToString();
		var controller = new TicketsController(CreateContext(dbName));

		//Act
		var result = await controller.Create(new CreateTicketDto("Damaged product!"));

		//Assert
		var checkContext = CreateContext(dbName);
		var saved = await checkContext.Tickets.SingleAsync();
		Assert.Equal("Damaged product!", saved.Title);
		Assert.Equal(TicketStatus.Open, saved.Status);
	}

    [Fact]
    public async Task UpdateStatus_ChangesStatusInDatabase()
    {
        // Arrange
        string dbName = Guid.NewGuid().ToString();
        var setupContext = CreateContext(dbName);
        var ticket = new Ticket("Wrong item delivered");
        setupContext.Tickets.Add(ticket);
        await setupContext.SaveChangesAsync();

        var controller = new TicketsController(CreateContext(dbName));

        // Act
        var result = await controller.UpdateStatus(ticket.Id, new UpdateTicketStatusDto(TicketStatus.Closed));

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var checkContext = CreateContext(dbName);
        var updated = await checkContext.Tickets.SingleAsync();
        Assert.Equal(TicketStatus.Closed, updated.Status);
    }

	[Fact]
	public async Task DeleteTicket_RemovesTicketFromDatabase()
	{
        //Arrange
        string dbName = Guid.NewGuid().ToString();
        var setupContext = CreateContext(dbName);
        var ticket = new Ticket("Wrong item delivered");
        setupContext.Tickets.Add(ticket);
        await setupContext.SaveChangesAsync();

        var controller = new TicketsController(CreateContext(dbName));

		//Act
		var result = await controller.DeleteTicket(ticket.Id);

		//Assert
		var checkContext = CreateContext(dbName);
        Assert.Equal(0, await checkContext.Tickets.CountAsync());
    }

	[Fact]
	public async Task UpdateStatus_ReturnsNotFound_WhenTicketDoesNotExist()
	{
        //Arrange
        var context = CreateContext(Guid.NewGuid().ToString());
        var controller = new TicketsController(context);

		//Act
		var result = await controller.UpdateStatus(99, new UpdateTicketStatusDto(TicketStatus.Closed));

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
