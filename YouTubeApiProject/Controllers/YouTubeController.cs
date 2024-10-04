using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;
using System.Collections.Generic;

namespace YouTubeApiProject.Controllers
{
    public class YouTubeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public YouTubeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        public IActionResult Index()
        {
            return View(new List<YouTubeVideoModel>()); // Pass an empty list initially
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query, string pageToken = "")
        {
            var (videos, nextPageToken, prevPageToken) = await _youtubeService.SearchVideosAsync(query, pageToken);

            ViewBag.NextPageToken = nextPageToken;
            ViewBag.PrevPageToken = prevPageToken;
            ViewBag.Query = query;

            return View("Index", videos);
        }
    }
}
