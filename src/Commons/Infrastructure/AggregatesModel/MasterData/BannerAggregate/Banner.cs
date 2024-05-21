using Core.Models.Base;

namespace Infrastructure.AggregatesModel.MasterData.BannerAggregate
{
    public class Banner : BaseEntity
    {

        public string Title { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Type { get; set; }





        public Banner() { }
        public Banner(string title, string image ,string type)
        {
            Title = title;
            Image = image;
            Type = type;
          
        }

        public static void Update(ref Banner banner, string title, string image)
        {
            banner.Title = title;
            banner.Image = image;
           
        }
        public static void DeleteBanner(ref Banner banner)
        {
            banner.IsDeleted = true;
        }

    }
}
