using Newtonsoft.Json;

namespace LevelEditor.Storage
{
    public class DatabaseResponse
    {
        [JsonProperty("login")]
        public string Login { get; set; }
    }
}
