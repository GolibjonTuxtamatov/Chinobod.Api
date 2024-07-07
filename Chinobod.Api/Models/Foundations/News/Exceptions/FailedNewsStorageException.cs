using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class FailedNewsStorageException : Xeption
    {
        public FailedNewsStorageException(Exception innerException)
            :base("Failed news storage error occured, contact support.",innerException)
        {}
    }
}
