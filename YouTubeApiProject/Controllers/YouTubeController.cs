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

            return View("Index", new YouTubeSearchResultsModel
            {
                Videos = videos,
                NextPageToken = nextPageToken,
                PrevPageToken = prevPageToken,
                Query = query
            });
        }
    }
}
