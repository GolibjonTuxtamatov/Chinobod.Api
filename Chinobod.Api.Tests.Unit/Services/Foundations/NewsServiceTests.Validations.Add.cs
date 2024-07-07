using System.Linq.Expressions;
using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using FluentAssertions;
using Moq;

namespace Chinobod.Api.Tests.Unit.Services.Foundations
{
    public partial class NewsServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfNewsIsNullAndLogItAsync()
        {
            //given
            News nullNews = null;

            var nullNewsException = new NullNewsException();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(nullNews))
                    .ThrowsAsync(nullNewsException);

            var expectedNewsValidationException =
                new NewsValidationException(nullNewsException);

            //when
            ValueTask<News> addNewsTask =
                this.newsService.AddNewsAsync(nullNews);

            NewsValidationException actualNewsValidationException =
                await Assert.ThrowsAsync<NewsValidationException>(addNewsTask.AsTask);

            //then
            actualNewsValidationException.Should().BeEquivalentTo(expectedNewsValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedNewsValidationException)))
                    ,Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(It.IsAny<News>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfNewsIsInvalidAndLogItAsync(string invalidString)
        {
            //given
            var invalidNews = new News
            {
                Tile = invalidString
            };

            var invalidNewsException = new InvalidNewsException();

            invalidNewsException.AddData(
                key: nameof(News.Id),
                values: "Id is required");

            invalidNewsException.AddData(
                key: nameof(News.Tile),
                values: "Text is required");

            invalidNewsException.AddData(
                key: nameof(News.Description),
                values: "Text is required");

            invalidNewsException.AddData(
                key: nameof(News.CreatedDate),
                values: "Date is required");

            var expectedNewsValidationException =
                new NewsValidationException(invalidNewsException);

            //when
            ValueTask<News> addNewsTask =
                this.newsService.AddNewsAsync(invalidNews);

            NewsValidationException actulNewsValidationException =
                await Assert.ThrowsAsync<NewsValidationException>(addNewsTask.AsTask);

            //then
            actulNewsValidationException.Should().BeEquivalentTo(expectedNewsValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedNewsValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(It.IsAny<News>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsPreviousAndLogItAsync()
        {
            //given
            DateTimeOffset earlierDate = GetEarlierDateTime();
            News someNews = CreateRandomNews(earlierDate);

            var invalidNewsException = new InvalidNewsException();

            invalidNewsException.AddData(
                key: nameof(News.CreatedDate),
                values: "Date should be now");

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(earlierDate);

            var expectedNewsValidationException =
                new NewsValidationException(invalidNewsException);

            //when
            ValueTask<News> addNewsTask =
                this.newsService.AddNewsAsync(someNews);

            NewsValidationException actualNewsVlidationException =
                await Assert.ThrowsAsync<NewsValidationException>(addNewsTask.AsTask);

            //then
            actualNewsVlidationException.Should().BeEquivalentTo(expectedNewsValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedNewsValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(It.IsAny<News>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
