namespace TicketApi
{

    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }

    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TicketStatus Status { get; set; }

        public Ticket(string title)
        {
            Title = title;
            Status = TicketStatus.Open;
        }
    }

    public record CreateTicketDto(string Title);
    public record UpdateTicketStatusDto(TicketStatus status);
    public record RegisterDto(string Username, string Password);
    public record LoginDto(string Username, string Password);
}
