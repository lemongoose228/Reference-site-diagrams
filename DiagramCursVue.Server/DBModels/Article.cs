namespace DiagramCursVue {
    public class Article
    {
        public int Id { get; set; }
        public string shortDescription { get; set; }
        public string articleText { get; set; }
        public List<string> tags { get; set; }
        public bool publish {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
