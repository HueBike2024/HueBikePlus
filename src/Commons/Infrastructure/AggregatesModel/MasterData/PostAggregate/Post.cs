using Core.Models.Base;

namespace Infrastructure.AggregatesModel.MasterData.PostAggregate
{
    public class Post : BaseEntity
    {
       
        public string Title { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Content { get; set; }

       

        public Post() { }
        public Post(string title,string image, string content)
        {
            Title = title;
            Image = image;
            Content = content;
        }

        public static void Update(ref Post post, string title,string image, string content)
        {
            post.Title = title;
            post.Image = image;
            post.Content = content;
        }
        public static void DeletePost(ref Post post)
        {
            post.IsDeleted = true;
        }

    }
}
