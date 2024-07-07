using System.Data;
using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;

namespace Chinobod.Api.Services.Foundations
{
    public partial class NewsService
    {
        private void ValidateNewsOnAdd(News news)
        {
            ValidateNotNull(news);

            Validate((Rule: IsInvalid(news.Id), Parameter: nameof(News.Id)),
                    (Rule: IsInvalid(news.Tile), Parameter: nameof(News.Tile)),
                    (Rule: IsInvalid(news.Description), Parameter: nameof(News.Description)),
                    (Rule: IsInvalid(news.CreatedDate), Parameter: nameof(News.CreatedDate)));
        }

        private static void ValidateNotNull(News nullNews)
        {
            if (nullNews == null)
                throw new NullNewsException();
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule,string Parameter)[] values)
        {
            var invalidNewsException = new InvalidNewsException();

            foreach ((dynamic rule, string parameter) in values)
            {
                if (rule.Condition)
                {
                    invalidNewsException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }
            invalidNewsException.ThrowIfContainsErrors();
        }
    }
}
