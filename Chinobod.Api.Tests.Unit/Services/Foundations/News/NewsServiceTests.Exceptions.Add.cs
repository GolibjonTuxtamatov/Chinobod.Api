using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;

namespace Chinobod.Api.Tests.Unit.Services.Foundations
{
    public partial class NewsServiceTests
    {
        [Fact]
        public async Task ShoudThrowCriticalDependecyValidationExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset currentDate = GetRandomDateTime();
            News someNews = CreateRandomNews(currentDate);

            SqlException sqlException = GetSqlException();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(currentDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(someNews))
                    .Throws(sqlException);

            var failedNewsStorageException = new FailedNewsStorageException(sqlException);

            var expectedNewsDependencyValidationException =
                new NewsDependencyValidationException(failedNewsStorageException);

            //when
            ValueTask<News> addNewsTask = this.newsService.AddNewsAsync(someNews);

            NewsDependencyValidationException actualNewsDependencyException =
                await Assert.ThrowsAsync<NewsDependencyValidationException>(addNewsTask.AsTask);

            //then
            actualNewsDependencyException.Should().BeEquivalentTo(expectedNewsDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(someNews),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedNewsDependencyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationDependencyExceptionOnAddIfDuplicateKeyErrorOccurs()
        {
            //given
            DateTimeOffset randomTime = GetRandomDateTime();
            News someNews = CreateRandomNews(randomTime);
            string randomString = CreateRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomString);

            var alreadyExistNewsException =
                new AlreadyExistNewsException(duplicateKeyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(someNews))
                    .Throws(duplicateKeyException);

            var expectedNewsDependenccyValidationException =
                new NewsDependencyValidationException(alreadyExistNewsException);

            //when
            ValueTask<News> addNewsTask =
                this.newsService.AddNewsAsync(someNews);

            NewsDependencyValidationException actualNewsDependencyValidationException =
                await Assert.ThrowsAsync<NewsDependencyValidationException>(addNewsTask.AsTask);

            //
            actualNewsDependencyValidationException.Should().BeEquivalentTo(expectedNewsDependenccyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(someNews),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedNewsDependenccyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTime();
            News someNews = CreateRandomNews(randomDate);

            var serviceException = new Exception();

            var failedNewsServiceException =
                new FailedNewsServiceException(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(someNews))
                    .ThrowsAsync(serviceException);

            var expectedNewsServiceException =
                new NewsServiceException(failedNewsServiceException);

            //when
            ValueTask<News> addNewsTask =
                this.newsService.AddNewsAsync(someNews);

            NewsServiceException actualNewsServiceException =
                await Assert.ThrowsAsync<NewsServiceException>(addNewsTask.AsTask);

            //then
            actualNewsServiceException.Should().BeEquivalentTo(expectedNewsServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(someNews),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedNewsServiceException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
