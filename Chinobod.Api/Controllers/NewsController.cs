using Chinobod.Api.Models.Foundations;
using Chinobod.Api.Services.Foundations;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Chinobod.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : RESTFulController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService) =>
            this.newsService = newsService;

        [HttpPost]
        public async ValueTask<ActionResult<News>> PostNewsAsync(News news)
        {
            News postedNews = await this.newsService.AddNewsASync(news);

            return Created(postedNews);
        }


        [HttpGet]
        public ActionResult<IQueryable<News>> GetAllNews() =>
            Ok(this.newsService.RetriveAllNews());

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<News>> GetNewsByIdAsync(Guid id)
        {
            News foundNews = await this.newsService.SelectNewsByIdAsync(id);

            return Ok(foundNews);
        }

        [HttpPut]
        public async ValueTask<ActionResult<News>> PutNewsAsync(News newNews)
        {
            News putedNews = await this.newsService.ModifyNewsAsync(newNews);

            return Ok(newNews);
        }

        [HttpDelete("{id}")]
        public async ValueTask<ActionResult<News>> DeleteNewsAsync(Guid id)
        {
            News deletedNews = await this.newsService.RemoveNewsAsync(id);

            return Ok(deletedNews);
        }
    }
}
