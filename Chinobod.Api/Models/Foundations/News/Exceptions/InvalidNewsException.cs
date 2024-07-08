using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class InvalidNewsException : Xeption
    {
        public InvalidNewsException()
            : base("News is invalid.")
        { }
    }
}
