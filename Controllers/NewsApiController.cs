using CRUD_NEWS.Helpers;
using CRUD_NEWS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Web;

using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;

namespace CRUD_NEWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsApiController : ControllerBase
    {
        private readonly NewsDbContext dbContext;
        public NewsApiController(NewsDbContext dbContext)
        {
            this.dbContext = dbContext;
          
        }
        [HttpGet]
        [Route("/api/news/all")]
        public async Task<IActionResult> NewsList()
        {
            try
            {
                IEnumerable<NewsModel> newsList = await dbContext.News
                                                            .Where(news => !news.is_deleted) // Example condition
                                                            .ToListAsync();
                int resultLength = newsList.Count();
                // Log the result length
                // Logger.LogInformation($"Number of news items: {resultLength}");
                var array = new string[] { Guid.NewGuid().ToString() };
                // Return JsonResult with the anonymous object
                return new JsonResult(Json.Success(newsList, "Success message", "Exception message")); 
            }
            catch (Exception )
            {
                // Log the exception
                // Logger.LogError(ex, "Error occurred while fetching news items.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching news items.");
            }
        }

 
        [HttpPost]
        [Route("/api/news/create")]
        public async Task<IActionResult> Create([FromBody] NewsModel news, HttpPostedFileBase file)
        {
            try
            {
                Console.WriteLine(news);
                if (news == null)
                {
                    return BadRequest("News item is null.");
                }

                // Generate a unique ID for the news item
                news.id = Guid.NewGuid().ToString();

                // Set the creation timestamp
                news.created_at = DateTime.Now;

                // Set the 'is_deleted' flag to false
                news.is_deleted = false;
                
                // Validate the incoming model
                if (!ModelState.IsValid)
                {
                    // Return a bad request response with validation errors
                    return BadRequest(ModelState);
                }

                // Add the news item to the database context
                dbContext.News.Add(news);

                // Save changes to the database asynchronously
                await dbContext.SaveChangesAsync();

                // Return a success response with the created news item
                return Ok(new { success = true, message = "News item created successfully", data = news });
            }
            catch (Exception )
            {
                // Log the exception
                // Logger.LogError(ex, "Error occurred while creating a news item.");

                // Return a 500 Internal Server Error response with an error message
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while creating a news item.");
            }
        }

        [HttpPost]
        [Route("/api/news/update")]
        public async Task<IActionResult> Update([FromBody] NewsModel news)
        {
            try
            {
                var existingNews = await dbContext.News.FindAsync(news.id);

                if (existingNews == null)
                {
                    return NotFound(); // Return a 404 Not Found response if the news item doesn't exist
                }

                news.updated_at = DateTime.Now;Console.WriteLine(news.updated_at);
                dbContext.News.Update(existingNews);
                await dbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "News item updated successfully", data = existingNews });
            }
            catch (Exception)
            {
                // Log the exception or handle it in an appropriate way
                return StatusCode(500, new { success = false, message = "An error occurred while updating the news item" });
            }
        }

        [HttpPost]
        [Route("/api/news/delete")]
        public async Task<IActionResult> Delete([FromBody] NewsModel news)
        {
            try
            {
                var existingNews = await dbContext.News.FindAsync(news.id);

                if (existingNews == null)
                {
                    return NotFound(); // Return a 404 Not Found response if the news item doesn't exist
                }

                // Update the existing news item with the data from the request
            
                existingNews.is_deleted = true;

                dbContext.News.Update(existingNews);
                await dbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "News item updated successfully", data = existingNews });
            }
            catch (Exception)
            {
                // Log the exception or handle it in an appropriate way
                return StatusCode(500, new { success = false, message = "An error occurred while updating the news item" });
            }
        }
        [HttpGet("/api/news/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            // Your logic to fetch news by ID
            // For demonstration, let's assume we're just returning the news ID
            var news = await dbContext.News.FindAsync(id);
            if (news == null)
            {
                return Ok(new { success = true, message = "News item created successfully", data = news });
            }
            if (news.img == null || news.img == "")
            {

                news.img = "/uploads/about-2.png";
            }
         

            return Ok(new { success = true, message = "News item created successfully", data = news });
        }
        /*
        [HttpGet]
        [Route("/api/news")]
        public Task<IActionResult> NewsList()
        {
            // Implement this method if needed
        }
        */
    }
}
