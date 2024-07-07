using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;

namespace Chinobod.Api.Services.Foundations
{
    public partial class NewsService
    {
        private static void ValidateNotNull(News nullNews)
        {
            if (nullNews == null)
                throw new NullNewsException();
        }
    }
}
