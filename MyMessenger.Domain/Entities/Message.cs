namespace MyMessenger.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
