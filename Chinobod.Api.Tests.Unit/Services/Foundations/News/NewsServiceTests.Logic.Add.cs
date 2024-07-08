using Chinobod.Api.Models.Foundations.News;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace Chinobod.Api.Tests.Unit.Services.Foundations
{
    public partial class NewsServiceTests
    {
        [Fact]
        public async Task ShouldAddNewsAsync()
        {
            //given
            DateTimeOffset currentDate = GetEarlierDateTime();
            News randomNews = CreateRandomNews(currentDate);
            News inputNews = randomNews;
            News storageNews = inputNews;
            News expectedNews = storageNews.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(currentDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(inputNews))
                    .ReturnsAsync(storageNews);

            //when
            News actualNews =
                await this.newsService.AddNewsAsync(inputNews);

            //then
            actualNews.Should().BeEquivalentTo(expectedNews);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(inputNews),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
