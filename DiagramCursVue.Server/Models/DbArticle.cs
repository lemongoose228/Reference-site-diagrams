using DiagramCursVue;
using DiagramCursVue.Data;
using DiagramCursVue.Requests;
using System.Security.Cryptography;

namespace DiagramCursVue.Models {
    public class DbArticle {

        public static bool Add(AppDbContext context, CreateArticle data) {
            Article file = new Article {
                publish = data.publish,
                shortDescription = data.shortDescription,
                articleText = data.articleText,
                tags = data.tags,
            };

            context.Articles.Add(file);
            context.SaveChanges();

            return true;
        }

        public static bool Delete(AppDbContext context, DeleteArticle data) {
            var foundArticle = context.Articles.Where(a => a.Id == data.Id).First();

            if (foundArticle != null) {
                context.Remove(foundArticle);

                context.SaveChanges();
            }
            else {
                throw new ArticleException("Статья не найдена");
            }

            return true;
        }

        public static bool Update(AppDbContext context, UpdateArticle data) {
            var foundArticle = context.Articles.Where(a => a.Id == data.Id).First();

            if (foundArticle != null) {
                foundArticle.publish = data.publish;
                foundArticle.articleText = data.articleText;
                foundArticle.tags = data.tags;
                foundArticle.shortDescription = data.shortDescription;

                context.SaveChanges();
            }
            else {
                throw new ArticleException("Статья не найдена");
            }

            return true;
        }

        public static List<Article> GetArticlesByTags (AppDbContext context, GetArticleByTags data) {
            List<Article> filteredArticles = new List<Article>();
            
            foreach (var tag in data.tags) {
                List<Article> articles;
                if (data.isTeacher) {
                    articles = context.Articles.Where(a => a.tags.Contains(tag)).ToList();
                }
                else {
                    articles = context.Articles.Where(a => a.tags.Contains(tag) && a.publish == true).ToList();
                }
                filteredArticles.AddRange(articles);
            }

            filteredArticles = filteredArticles.Distinct().ToList();

            return filteredArticles;
        }

        public static List<Article> GetAllArticlesToTeacher(AppDbContext context) {
            List<Article> filteredArticles = context.Articles.ToList();

            return filteredArticles;
        }

        public static List<Article> GetAllArticles(AppDbContext context) {
            List<Article> filteredArticles = context.Articles.Where(a => a.publish == true).ToList();

            return filteredArticles;
        }

        public static Article GetArticleById(AppDbContext context, int id) {
            Article filteredArticle = context.Articles.Where(a => a.Id == id).First();

            return filteredArticle;
        }
    }
}
