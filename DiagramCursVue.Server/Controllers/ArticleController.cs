using DiagramCursVue.Data;
using DiagramCursVue.Models;
using DiagramCursVue.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace DiagramCursVue.Controllers {
    [ApiController]
    [Route("api/")]
    public class ArticleController : ControllerBase {
        private readonly ILogger<ArticleController> _logger;
        private readonly AppDbContext _context;
        public ArticleController(ILogger<ArticleController> logger, AppDbContext context) {
            _logger = logger;
            _context = context;
        }

        [HttpPost("create-article")]
        public IActionResult CreateArticle(CreateArticle data) {
            try {
                DbArticle.Add(_context, data);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("get-all-articles-teacher")]
        public ActionResult<IEnumerable<Article>> GetAllToTeacher() {
            try {
                List<Article> articles = DbArticle.GetAllArticlesToTeacher(_context);
                return Ok(articles);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("get-all-articles")]
        public ActionResult<IEnumerable<Article>> GetAll() {
            try {
                List<Article> articles = DbArticle.GetAllArticles(_context);
                return Ok(articles);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("get-articles-by-tag")]
        public ActionResult<IEnumerable<Article>> GetArticlesByTag(GetArticleByTags data) {
            try {
                List<Article> articles;

                if (data.tags.Count() == 0) {
                    articles = DbArticle.GetAllArticlesToTeacher(_context);
                }
                else {
                    articles = DbArticle.GetArticlesByTags(_context, data);
                }
                return Ok(articles);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("update-article")]
        public IActionResult UpdateArticle(UpdateArticle data) {
            try {
                DbArticle.Update(_context, data);
                return Ok();
            }
            catch (ArticleException e) {
                return BadRequest(e.Message);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete-article")]
        public IActionResult DeleteArticle(DeleteArticle data) {
            try {
                DbArticle.Delete(_context, data);
                return Ok();
            }
            catch (ArticleException e) {
                return BadRequest(e.Message);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("get-article-by-id")]
        public IActionResult GetArticleById(GetArticle data) {
            try {
                Article article = DbArticle.GetArticleById(_context, data.Id);
                return Ok(article);
            }
            catch (ArticleException e) {
                return BadRequest(e.Message);
            }
            catch (Exception e) {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
