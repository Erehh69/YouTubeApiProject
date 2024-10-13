using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        // Display Search Page
        public IActionResult Index()
        {
            return View(new YouTubeSearchResultsModel
            {
                Videos = new List<YouTubeVideoModel>(),
                NextPageToken = "",
                PrevPageToken = ""
            });
        }

        // Handle the search query
        [HttpPost]
        public async Task<IActionResult> Search(string query, string pageToken = "")
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Index", new YouTubeSearchResultsModel
                {
                    Videos = new List<YouTubeVideoModel>(),
                    NextPageToken = "",
                    PrevPageToken = ""
                });
            }

            var (videos, nextPageToken, prevPageToken) = await _youtubeService.SearchVideosAsync(query, pageToken);

            // Here, we can return the videos from the search along with related videos if needed
            return View("Index", new YouTubeSearchResultsModel
            {
                Videos = videos,
                NextPageToken = nextPageToken,
                PrevPageToken = prevPageToken,
                Query = query
            });
        }

        // Action to watch the video and display related videos
        public async Task<IActionResult> Watch(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
            {
                return NotFound();
            }

            // Fetch video details using YouTube API
            var (video, relatedVideos) = await _youtubeService.GetVideoWithRelatedAsync(videoId);
            if (video == null)
            {
                return NotFound();
            }

            // Create a view model to pass video and related videos to the view
            var watchViewModel = new YouTubeVideoViewModel
            {
                Video = video,
                RelatedVideos = relatedVideos
            };

            return View(watchViewModel); // Pass video and related videos to the view
        }
    }
}
