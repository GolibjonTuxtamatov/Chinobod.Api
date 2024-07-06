﻿using System.Linq.Expressions;
using Chinobod.Api.Brokers.DateTimes;
using Chinobod.Api.Brokers.Loggings;
using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using Chinobod.Api.Services.Foundations;
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

        private DateTimeOffset GetRandomDateTime() =>
            DateTimeOffset.UtcNow;
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
