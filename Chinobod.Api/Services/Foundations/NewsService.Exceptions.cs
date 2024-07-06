using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using Xeptions;

namespace Chinobod.Api.Services.Foundations
{
    public partial class NewsService
    {
        private delegate ValueTask<News> ReturningNewsFunction();

        private async ValueTask<News> TryCatch(ReturningNewsFunction returningNewsFunction)
        {
            try
            {
                return await returningNewsFunction();
            }
            catch (NullNewsException nullNewsException)
            {
                throw CreateAndLogValidationException(nullNewsException);
            }
        }

        private Xeption CreateAndLogValidationException(Xeption exception)
        {

            var newsValidationException =
                new NewsValidationException(exception);

            this.loggingBroker.LogError(newsValidationException);

            return newsValidationException;
        }
    }
}
