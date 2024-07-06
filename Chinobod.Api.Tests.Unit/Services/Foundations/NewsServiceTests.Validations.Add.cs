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
    }
}
