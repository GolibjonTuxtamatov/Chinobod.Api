using Chinobod.Api.Models.Foundations.News;

namespace Chinobod.Api.Services.Foundations
{
    public interface INewsService
    {
        public ValueTask<News> AddNewsAsync(News news);
        public IQueryable<News> RetriveAllNews();
        public ValueTask<News> SelectNewsByIdAsync(Guid id);
        public ValueTask<News> ModifyNewsAsync(News news);
        public ValueTask<News> RemoveNewsAsync(Guid id);
    }
}
