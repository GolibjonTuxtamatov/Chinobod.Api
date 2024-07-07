using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class NewsValidationException : Xeption
    {
        public NewsValidationException(Xeption innerException)
            : base("News validation error occured,fix the error and try again.", innerException)
        { }
    }
}
