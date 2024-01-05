using RepositoryFinder.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace RepositoryFinder.Integrations
{
    public interface IGithubApi
    {
        Task<Repository[]> GetRepositories(string username);
    }

    public class GitHubApi : IGithubApi
    {
        private readonly HttpClient _httpClient;

        public GitHubApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //httpClient uses a new socket each time. Use as few as possible, max 10 usually
            //Dependency injection like this is vastly preferable to creating a HttpClient in the method
        }
        public async Task<GitHubUser> GetUserAsync(string userName)
        {

            var response = await _httpClient.GetAsync($"https://api.github.com/users/{userName}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception();
            }

            var content = await response.Content.ReadAsStringAsync();


            GitHubUser user = JsonSerializer.Deserialize<GitHubUser>(content);

            return user;
        }

        public async Task<Repository[]> GetRepositories(string username)
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/users/{username}/repos");

            if (response.StatusCode == HttpStatusCode.NotFound) { throw new Exception(); }
            var content = await response.Content.ReadAsStringAsync();

            Repository[] repositories = JsonSerializer.Deserialize<Repository[]>(content);

            if (repositories is not null)
            {
                    foreach (Repository repo in repositories)
                    {
                        var foundlanguages = await _httpClient.GetAsync(repo.LanguagesUrl);
                        var stringifiedLanguages = await foundlanguages.Content.ReadAsStringAsync();
                        repo.Languages = JsonSerializer.Deserialize<Dictionary<string, int>>(stringifiedLanguages);
                    }
            }

            return repositories;
        }
    }
}