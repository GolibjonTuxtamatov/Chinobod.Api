using Chinobod.Api.Brokers.DateTimes;
using Chinobod.Api.Brokers.Loggings;
using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using Xeptions;

namespace Chinobod.Api.Services.Foundations
{
    public class NewsService : INewsService
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

        public async ValueTask<News> AddNewsAsync(News news)
        {
            try
            {
                if (news == null)
                    throw new NullNewsException();

                return await this.storageBroker.InsertNewsAsync(news);
            }
            catch(NullNewsException nullNewsException)
            {
                throw CreateAndLogValidationException(nullNewsException);
            }
        }

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

        private Xeption CreateAndLogValidationException(Xeption exception)
        {
            var newsValidationException =
                new NewsValidationException(exception);

            this.loggingBroker.LogError(newsValidationException);

            return newsValidationException;
        }
    }
}
