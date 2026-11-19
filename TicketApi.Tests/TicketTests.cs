using TicketApi;
namespace TicketApi.Tests;

public class TicketTests
{
	[Fact]
	public void NewTicket_StartsWithOpenStatus()
	{
		//Arrange
		string title = "Damaged product!";

		//Act
		var ticket = new Ticket(title);

		//Assert
		Assert.Equal(TicketStatus.Open, ticket.Status);
	}

    [Fact]
    public void NewTicket_KeepsTheTitle()
    {
        // Arrange
        string title = "Wrong item delivered";

        // Act
        var ticket = new Ticket(title);

        // Assert
        Assert.Equal(title, ticket.Title);
    }
}
