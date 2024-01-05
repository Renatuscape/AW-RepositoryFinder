using System.Text.Json.Serialization;

namespace RepositoryFinder.Models
{
    public class Repository
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("languages_url")]
        public string LanguagesUrl {  get; set; } = string.Empty;

        [JsonIgnore]
        public Dictionary<string, int>? Languages { get; set; }
    }
}
