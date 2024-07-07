using Xeptions;

namespace Chinobod.Api.Models.Foundations.News.Exceptions
{
    public class NullNewsException : Xeption
    {
        public NullNewsException()
            : base("News is null.")
        { }
    }
}
