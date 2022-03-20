namespace Minitwit_BE.Domain
{
    public class Message
    {
        public int MessageId { get; set; }
        public int AuthorId { get; set; }
        public string? Text { get; set; }
        public DateTime PublishDate { get; set; }
        public bool Flagged { get; set; }
    }
}
