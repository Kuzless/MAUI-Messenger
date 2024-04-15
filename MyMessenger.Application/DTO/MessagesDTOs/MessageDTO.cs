namespace MyMessenger.Application.DTO.MessagesDTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Text { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
