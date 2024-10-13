using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeApiProject.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly string _apiKey;

        public YouTubeApiService(IConfiguration configuration)
        {
            _apiKey = configuration["YouTubeApiKey"];
        }

        // Method to get trending videos
        public async Task<List<YouTubeVideoModel>> GetTrendingVideosAsync(string regionCode = "US")
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                // Define a video list request to get the most popular videos
                var videoRequest = youtubeService.Videos.List("snippet,statistics");
                videoRequest.Chart = VideosResource.ListRequest.ChartEnum.MostPopular; // Fetch trending videos
                videoRequest.RegionCode = regionCode; // You can specify a region code like "US"
                videoRequest.MaxResults = 10; // Number of videos to retrieve

                var videoResponse = await videoRequest.ExecuteAsync();

                var trendingVideos = videoResponse.Items.Select(video => new YouTubeVideoModel
                {
                    VideoId = video.Id,
                    Title = video.Snippet.Title,
                    Description = video.Snippet.Description,
                    ThumbnailUrl = video.Snippet.Thumbnails.Medium.Url,
                    ViewCount = video.Statistics.ViewCount?.ToString() ?? "N/A",
                    LikeCount = video.Statistics.LikeCount?.ToString() ?? "N/A"
                }).ToList();

                return trendingVideos;
            }
            catch (Exception ex)
            {
                // Handle the error (consider logging the exception)
                return new List<YouTubeVideoModel>(); // Return an empty list in case of an error
            }
        }

        // Method to get a video by ID
        public async Task<YouTubeVideoModel> GetVideoByIdAsync(string videoId)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var videoRequest = youtubeService.Videos.List("snippet,statistics");
                videoRequest.Id = videoId;

                var videoResponse = await videoRequest.ExecuteAsync();
                var video = videoResponse.Items.FirstOrDefault();

                if (video != null)
                {
                    return new YouTubeVideoModel
                    {
                        VideoId = video.Id,
                        Title = video.Snippet.Title,
                        Description = video.Snippet.Description,
                        ThumbnailUrl = video.Snippet.Thumbnails.Medium.Url,
                        ViewCount = video.Statistics.ViewCount?.ToString() ?? "N/A",
                        LikeCount = video.Statistics.LikeCount?.ToString() ?? "N/A"
                    };
                }

                return null; // Return null if the video is not found
            }
            catch (Exception ex)
            {
                // Handle the error (consider logging the exception)
                return null;
            }
        }

        // Method to get related videos based on a video ID
        public async Task<List<YouTubeVideoModel>> GetRelatedVideosAsync(string videoId)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "YouTubeProject"
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.MaxResults = 10;
                searchRequest.Type = "video";

                // Instead of using RelatedToVideoId, set the search query
                searchRequest.Q = videoId; // Use the videoId in the search query

                var searchResponse = await searchRequest.ExecuteAsync();

                var relatedVideos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    VideoId = item.Id.VideoId,
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url
                }).ToList();

                return relatedVideos;
            }
            catch (Exception ex)
            {
                // Handle the error properly
                return new List<YouTubeVideoModel>(); // Return an empty list in case of an error
            }
        }

        // Method to get a video by ID along with related videos
        public async Task<(YouTubeVideoModel video, List<YouTubeVideoModel> relatedVideos)> GetVideoWithRelatedAsync(string videoId)
        {
            var video = await GetVideoByIdAsync(videoId);
            var relatedVideos = await GetRelatedVideosAsync(videoId);
            return (video, relatedVideos);
        }

        // Method to search for videos based on a query
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
                searchRequest.Q = query; // Set search query
                searchRequest.MaxResults = 10; // Limit results
                searchRequest.PageToken = pageToken; // Handle pagination
                searchRequest.Type = "video"; // Set type to video

                var searchResponse = await searchRequest.ExecuteAsync();

                var videos = searchResponse.Items.Select(item => new YouTubeVideoModel
                {
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                    VideoId = item.Id.VideoId
                }).ToList();

                return (videos, searchResponse.NextPageToken, searchResponse.PrevPageToken); // Return results and pagination tokens
            }
            catch (Exception ex)
            {
                // Handle error properly (consider logging the exception)
                return (new List<YouTubeVideoModel>(), null, null); // Return empty list on error
            }
        }
    }
}
