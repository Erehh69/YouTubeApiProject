using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeApiProject.Models;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;

        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"];
        }

        // Method to search videos
        public async Task<(List<YouTubeVideoModel>, string nextPageToken, string prevPageToken)> SearchVideosAsync(string query, string pageToken = "")
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.Q = query;
                searchRequest.MaxResults = 10;
                searchRequest.PageToken = pageToken; // Handle pagination
                searchRequest.Type = "video";

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id.VideoId
                }).ToList();

                return (videos, searchResponse.NextPageToken, searchResponse.PrevPageToken);
            }
            catch (Exception ex)
            {
                // Handle error, return empty list if failure occurs
                return (new List<YouTubeVideoModel>(), null, null);
            }
        }


        // Method to get trending videos (already defined)
        public async Task<List<YouTubeVideoModel>> GetTrendingVideosAsync()
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var searchRequest = youtubeService.Videos.List("snippet,statistics");
                searchRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular;
                searchRequest.MaxResults = 10; // Fetch 10 trending videos
                searchRequest.RegionCode = "MY"; // You can change this to your desired region

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id,
                    ViewCount = item.Statistics.ViewCount?.ToString() ?? "N/A",
                    LikeCount = item.Statistics.LikeCount?.ToString() ?? "N/A"
                }).ToList();

                return videos;
            }
            catch (Exception ex)
            {
                // Handle error, return empty list if failure occurs
                return new List<YouTubeVideoModel>();
            }
        }
    }
}
