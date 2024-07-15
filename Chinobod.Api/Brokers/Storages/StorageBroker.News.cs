using Chinobod.Api.Models.Foundations.News;
using Microsoft.EntityFrameworkCore;

namespace Chinobod.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<News> News { get; set; }


        public async ValueTask<News> InsertNewsAsync(News news) =>
            await InsertAsync(news);

        public IQueryable<News> SelectAllNews() =>
            SelectAll<News>()
            .OrderBy(news => news.ShouldDelete)
            .OrderByDescending(news => news);

        public async ValueTask<News> SelectNewsByIdAsync(Guid id) =>
            await SelectByIdAsync<News>(id);

        public async ValueTask<News> UpdateNewsAsync(News news) =>
            await UpdateAsync(news);

        public async ValueTask<News> DeleteNewsASync(News news) =>
            await DeleteAsync(news);

        public async ValueTask DeleteNotNeedNews(IQueryable<News> news)
        {
            using var broker = new StorageBroker();

            broker.RemoveRange(news);
            await broker.SaveChangesAsync();
        }
    }
}
