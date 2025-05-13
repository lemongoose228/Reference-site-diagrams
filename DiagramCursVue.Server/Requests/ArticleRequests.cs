namespace DiagramCursVue.Requests {
    public class CreateArticle {
        public string shortDescription { get; set; }
        public string articleText {  get; set; }
        public List<string> tags { get; set; }
        public bool publish {  get; set; }
    }

    public class UpdateArticle {
        public int Id { get; set; }
        public string shortDescription { get; set; }
        public string articleText { get; set; }
        public List<string> tags { get; set; }
        public bool publish { get; set; }
    }

    public class DeleteArticle {
        public int Id { get; set; }
    }

    public class GetArticle {
        public int Id { get; set; }
    }

    public class GetArticleByTags {
        public List<string> tags { get; set; }
        public bool isTeacher { get; set; }
    }
}
