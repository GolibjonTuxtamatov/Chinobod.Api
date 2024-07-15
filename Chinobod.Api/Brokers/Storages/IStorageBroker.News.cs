using Chinobod.Api.Models.Foundations.News;

namespace Chinobod.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<News> InsertNewsAsync(News news);
        public IQueryable<News> SelectAllNews();
        public ValueTask<News> SelectNewsByIdAsync(Guid id);
        public ValueTask<News> UpdateNewsAsync(News news);
        public ValueTask<News> DeleteNewsASync(News news);
        public ValueTask DeleteNotNeedNews(IQueryable<News> news);
    }
}
