using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using EFxceptions.Models.Exceptions;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistNewsException =
                    new AlreadyExistNewsException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistNewsException);
            }
            catch (Exception serviceException)
            {
                var failedNewsServiceException =
                    new FailedNewsServiceException(serviceException);

                throw CreateAndLogServiceException(failedNewsServiceException);
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
        
        private Xeption CreateAndLogDependencyValidationException(Xeption exception)
        {

            var newsDependencyValidationException =
                new NewsDependencyValidationException(exception);

            this.loggingBroker.LogError(newsDependencyValidationException);

            return newsDependencyValidationException;
        }
        
        private Xeption CreateAndLogServiceException(Xeption exception)
        {

            var newsServiceException =
                new NewsServiceException(exception);

            this.loggingBroker.LogError(newsServiceException);

            return newsServiceException;
        }
    }
}
