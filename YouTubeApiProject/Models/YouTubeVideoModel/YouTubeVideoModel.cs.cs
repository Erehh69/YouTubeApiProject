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

    public class YouTubeSearchResultsModel
    {
        public List<YouTubeVideoModel> Videos { get; set; }
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public string Query { get; set; }
    }

    public class WatchViewModel
    {
        public YouTubeVideoModel CurrentVideo { get; set; }
        public List<YouTubeVideoModel> RelatedVideos { get; set; }
    }

    public class YouTubeVideoViewModel
    {
        public YouTubeVideoModel Video { get; set; }
        public List<YouTubeVideoModel> RelatedVideos { get; set; }
    }
}
