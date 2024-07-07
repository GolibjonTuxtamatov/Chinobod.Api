using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class AlreadyExistNewsException : Xeption
    {
        public AlreadyExistNewsException(Exception innerException)
            :base("News alredy exist.")
        {}
    }
}
