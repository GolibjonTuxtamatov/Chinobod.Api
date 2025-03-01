﻿using Chinobod.Api.Models.Foundations.News;
using Chinobod.Api.Models.Foundations.News.Exceptions;
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
            try
            {
                News postedNews = await this.newsService.AddNewsAsync(news);

                return Created(postedNews);
            }
            catch (NewsValidationException newsValidationException)
            {
                return BadRequest(newsValidationException.InnerException);
            }
            catch (NewsDependencyValidationException newsDependencyValidationException)
                when(newsDependencyValidationException.InnerException is AlreadyExistNewsException)
            {
                return Conflict(newsDependencyValidationException.InnerException);
            }
            catch (NewsDependencyValidationException newsDependencyValidationException)
                when(newsDependencyValidationException is FailedNewsStorageException)
            {
                return InternalServerError(newsDependencyValidationException.InnerException);
            }
            catch (NewsDependencyValidationException newsDependencyValidationException)
            {
                return BadRequest(newsDependencyValidationException.InnerException);
            }
            catch (NewsServiceException newsServiceException)
            {
                return InternalServerError(newsServiceException.InnerException);
            }
        }


        [HttpGet]
        public ActionResult<IQueryable<News>> GetAllNews() =>
            Ok(this.newsService.RetriveAllNews());

        [HttpGet("{id}")]
        public async ValueTask<ActionResult<News>> GetNewsByIdAsync(Guid id)
        {
            News foundNews = await this.newsService.RetrieveNewsByIdAsync(id);

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

        [HttpDelete]
        public async ValueTask DeleteNotNeedNewsAsync() =>
            await this.newsService.RemoveNotNeedNewsAsync();
    }
}
