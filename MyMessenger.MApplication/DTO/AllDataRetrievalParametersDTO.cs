namespace MyMessenger.MApplication.DTO
{
    public class AllDataRetrievalParametersDTO
    {
        public Dictionary<string, bool>? Sort { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? Subs { get; set; }
    }
}
