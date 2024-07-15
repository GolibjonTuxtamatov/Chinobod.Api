using Chinobod.Api.Brokers.DateTimes;
using Chinobod.Api.Brokers.Loggings;
using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Models.Foundations.News;

namespace Chinobod.Api.Services.Foundations
{
    public partial class NewsService : INewsService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public NewsService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;

        }

        public ValueTask<News> AddNewsAsync(News news) =>
            TryCatch(async () =>
            {
                ValidateNewsOnAdd(news);

                return await this.storageBroker.InsertNewsAsync(news);
            });

        public async ValueTask<News> ModifyNewsAsync(News news) =>
            await this.storageBroker.UpdateNewsAsync(news);

        public async ValueTask<News> RemoveNewsAsync(Guid id)
        {
            News storageNews =
                await this.storageBroker.SelectNewsByIdAsync(id);

            return await this.storageBroker.DeleteNewsASync(storageNews);
        }

        public IQueryable<News> RetriveAllNews()
        {
            var sortedNewses = new List<News>();

            var newses = this.storageBroker.SelectAllNews();

            sortedNewses.AddRange(newses.Select(news => news)
                                        .Where(news => news.ShouldDelete == false));

            sortedNewses.AddRange(newses.Select(news => news)
                                        .Where(news => news.ShouldDelete == true));

            return sortedNewses.AsQueryable();
        }

        public async ValueTask<News> RetrieveNewsByIdAsync(Guid id) =>
            await this.storageBroker.SelectNewsByIdAsync(id);
    }
}
