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

        public async Task<(List<YouTubeVideoModel>, string, string)> SearchVideosAsync(string query, string pageToken = "")
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
                searchRequest.PageToken = pageToken;

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id.VideoId // Get the VideoId for the clickable link
                }).ToList();

                return (videos, searchResponse.NextPageToken, searchResponse.PrevPageToken);
            }
            catch (Exception ex)
            {
                return (new List<YouTubeVideoModel>(), null, null);
            }
        }
    }
}
