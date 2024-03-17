namespace MVCApp2.Models
{
    public class Post
    {
        public int id { get; set; }

        public string title { get; set; } = null!;

        public string body { get; set; } = null!;

        public int userId { get; set; }
    }
}
