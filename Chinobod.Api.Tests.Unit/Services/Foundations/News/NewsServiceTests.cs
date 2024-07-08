using System.Linq.Expressions;
using System.Runtime.Serialization;
using Chinobod.Api.Brokers.DateTimes;
using Chinobod.Api.Brokers.Loggings;
using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Services.Foundations;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Chinobod.Api.Tests.Unit.Services.Foundations
{
    public partial class NewsServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly INewsService newsService;

        public NewsServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.newsService = new NewsService(this.storageBrokerMock.Object,
                                           this.loggingBrokerMock.Object,
                                           this.dateTimeBrokerMock.Object);
        }

        private static string CreateRandomString() =>
            new MnemonicString().GetValue();

        private SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private DateTimeOffset GetRandomDateTime() =>
            DateTimeOffset.UtcNow;

        private DateTimeOffset GetEarlierDateTime()
        {
            var someTime = new TimeSpan(1, 2, 30, 0);

            return DateTimeOffset.UtcNow - someTime;
        }

        private News CreateRandomNews(DateTimeOffset dateTimeOffset) =>
            CreateNewsFiller(dateTimeOffset).Create();

        private Filler<News> CreateNewsFiller(DateTimeOffset dateTimeOffset)
        {
            Filler<News> newsFiller = new Filler<News>();

            newsFiller.Setup()
                .OnType<DateTimeOffset>()
                .Use(dateTimeOffset);

            return newsFiller;
        }



        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption exception) =>
            actualException => actualException.SameExceptionAs(exception);
    }
}
