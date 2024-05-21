namespace Core.Models.Settings
{
    public class MinIOSetting
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string PublicUrl { get; set; }
        public string Folder { get; set; }
    }
}
