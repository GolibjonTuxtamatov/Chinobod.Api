using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class FailedNewsServiceException : Xeption
    {
        public FailedNewsServiceException(Exception innerException)
            :base("Failed news service error occured, contact support.",
                 innerException)
        {}
    }
}
