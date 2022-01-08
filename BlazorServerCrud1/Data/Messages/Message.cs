namespace BlazorServerCrud1.Data.Messages
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
