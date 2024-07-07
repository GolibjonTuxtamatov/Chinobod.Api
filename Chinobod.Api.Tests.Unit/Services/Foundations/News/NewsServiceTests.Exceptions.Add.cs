using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedNewsDependencyValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
