using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTubeApiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly YouTubeApiService _youtubeService;

        public HomeController(YouTubeApiService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        public async Task<IActionResult> Index()
        {
            var trendingVideos = await _youtubeService.GetTrendingVideosAsync();
            return View(trendingVideos);
        }
    }
}
