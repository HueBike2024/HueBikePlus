using Core.SeedWork.ExtendEntities;

namespace MasterData.Application.DTOs.Post
{
    public class PostResponse : BaseExtendEntities
    {
        public int Index { get; set; }
        public long Id { get; set; }
        public string Tilte { get; set; }
        public string Image {  get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
