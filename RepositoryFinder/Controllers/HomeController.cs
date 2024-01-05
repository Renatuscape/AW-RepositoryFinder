using Microsoft.AspNetCore.Mvc;
using RepositoryFinder.Integrations;
using RepositoryFinder.Models;
using System.Diagnostics;

namespace RepositoryFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GitHubApi _gitHubApi;
        public HomeController(ILogger<HomeController> logger, GitHubApi gitHubApi)
        {
            _logger = logger;
            _gitHubApi = gitHubApi;
        }


        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost("/")]
        public async Task<IActionResult> Index(IndexViewModel model)
        {

            GitHubUser gitHubUser = await _gitHubApi.GetUserAsync(model.GitHubUserName);
            model.User = gitHubUser;
            model.Repositories = await _gitHubApi.GetRepositories(model.GitHubUserName);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
