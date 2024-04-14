namespace MyMessenger.MApplication.DTO
{
    public class DataForGridDTO<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int NumberOfPages { get; set; }
    }
}
