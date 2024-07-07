using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class NewsDependencyValidationException : Xeption
    {
        public NewsDependencyValidationException(Xeption innerException)
            : base("News dependency validation error occured, contact support",innerException)
        {}
    }
}
