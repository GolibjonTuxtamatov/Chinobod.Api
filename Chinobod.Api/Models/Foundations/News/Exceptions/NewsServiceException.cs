using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class NewsServiceException : Xeption
    {
        public NewsServiceException(Xeption innerException)
            :base("News service error occured, contact support.",
                 innerException)
        { }
    }
}
