using Microsoft.AspNetCore.Mvc;
using YouTubeApiProject.Services;
using YouTubeApiProject.Models;

public class WatchController : Controller
{
    private readonly YouTubeApiService _youtubeApiService;

    public WatchController(YouTubeApiService youtubeApiService)
    {
        _youtubeApiService = youtubeApiService;
    }

    public async Task<IActionResult> Watch(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var (video, relatedVideos) = await _youtubeApiService.GetVideoWithRelatedAsync(id);

        if (video == null)
        {
            return NotFound();
        }

        var viewModel = new YouTubeVideoViewModel
        {
            Video = video,
            RelatedVideos = relatedVideos
        };

        return View("watch", viewModel);
    }
}
