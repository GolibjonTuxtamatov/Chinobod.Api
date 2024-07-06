using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chinobod.Api.Models.Foundations;
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
            News randomNews = CreateRandomNews(GetRandomDateTime());
            News inputNews = randomNews;
            News storageNews = inputNews;
            News expectedNews = storageNews.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertNewsAsync(inputNews))
                    .ReturnsAsync(storageNews);

            //when
            News actualNews =
                await this.newsService.AddNewsASync(inputNews);

            //then
            actualNews.Should().BeEquivalentTo(expectedNews);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertNewsAsync(inputNews),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
