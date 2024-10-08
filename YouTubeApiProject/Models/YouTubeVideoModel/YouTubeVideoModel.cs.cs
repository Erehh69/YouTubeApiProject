namespace YouTubeApiProject.Models
{
    public class YouTubeVideoModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoId { get; set; }
        public string ViewCount { get; set; }  // New property for total views
        public string LikeCount { get; set; }  // New property for total likes
    }
}
