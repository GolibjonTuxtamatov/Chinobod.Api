namespace Chinobod.Api.Models.Foundations.News
{
    public class News
    {
        public Guid Id { get; set; }
        public string Tile { get; set; }
        public string Description { get; set; }
        public bool ShouldDelete { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}