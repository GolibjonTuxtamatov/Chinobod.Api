using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (InvalidNewsException invalidNewsException)
            {
                throw CreateAndLogValidationException(invalidNewsException);
            }
            catch (SqlException sqlException)
            {
                var failedNewsStorageException =
                    new FailedNewsStorageException(sqlException);

                throw CreateAndLogCriticalDependencyValidationException(failedNewsStorageException);
            }
        }

        private Xeption CreateAndLogValidationException(Xeption exception)
        {

            var newsValidationException =
                new NewsValidationException(exception);

            this.loggingBroker.LogError(newsValidationException);

            return newsValidationException;
        }
        
        private Xeption CreateAndLogCriticalDependencyValidationException(Xeption exception)
        {

            var newsDependencyValidationException =
                new NewsDependencyValidationException(exception);

            this.loggingBroker.LogCritical(newsDependencyValidationException);

            return newsDependencyValidationException;
        }
    }
}
