namespace MyMessenger.Application.DTO
{
    public class AllDataRetrievalParametersDTO
    {
        public Dictionary<string, bool> Sort { get; set; } = new Dictionary<string, bool>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Subs { get; set; } = "";
    }
}
