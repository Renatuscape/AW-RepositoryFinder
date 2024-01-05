namespace RepositoryFinder.Models
{
    public class IndexViewModel
    {
        public string GitHubUserName { get; set; } = string.Empty;
        public GitHubUser? User { get; set; }
        public Repository[]? Repositories { get; set; }
    }
}
