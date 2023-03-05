namespace Sales.Shared.DTOs
{
    public static class QueryableExtensions
    {
        public class PaginationDTO
        {
            public int Id { get; set; }

            public int Page { get; set; } = 1;

            public int RecordsNumber { get; set; } = 10;
        }
    }
}