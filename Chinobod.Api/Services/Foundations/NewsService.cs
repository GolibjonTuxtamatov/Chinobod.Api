using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Models.Foundations;

namespace Chinobod.Api.Services.Foundations
{
    public class NewsService : INewsService
    {
        private readonly IStorageBroker storageBroker;

        public NewsService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public async ValueTask<News> AddNewsASync(News news) =>
            await this.storageBroker.InsertNewsAsync(news);

        public async ValueTask<News> ModifyNewsAsync(News news) =>
            await this.storageBroker.UpdateNewsAsync(news);

        public async ValueTask<News> RemoveNewsAsync(Guid id)
        {
            News storageNews =
                await this.storageBroker.SelectNewsByIdAsync(id);

            return await this.storageBroker.DeleteNewsASync(storageNews);
        }

        public IQueryable<News> RetriveAllNews() =>
            this.storageBroker.SelectAllNews();

        public async ValueTask<News> SelectNewsByIdAsync(Guid id) =>
            await this.SelectNewsByIdAsync(id);
    }
}
