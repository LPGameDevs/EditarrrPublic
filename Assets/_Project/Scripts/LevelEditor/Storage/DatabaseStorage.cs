using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LevelEditor.Storage
{
    public class DatabaseStorage : ILevelStorage
    {
        private readonly HttpClient _client;
        private readonly DatabaseSettings _settings;

        public DatabaseStorage(DatabaseSettings settings)
        {
            _client = new HttpClient();
            _settings = settings;
        }

        public async Task<LevelSave> GetLevel(string code)
        {
            LevelSave levelData = new LevelSave();

            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return levelData;
                }

                var request = new HttpRequestMessage() {
                    RequestUri = new Uri($"{_settings.BaseUrl.TrimEnd('/')}/users/{code}"),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("User-Agent","Unity"); // required
                request.Headers.Add("Authorisation", _settings.Auth);
                var result = await _client.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                {
                    // might want to add a logger in here and serialize entire response and save it for checking later
                    return levelData;
                }

                var json = await result.Content.ReadAsStringAsync();

                var ob =  JsonConvert.DeserializeObject<DatabaseResponse>(json);
                return JsonConvert.DeserializeObject<LevelSave>(json);
            }
            catch (Exception e)
            {
                // might want to log exception
                // and throw or return bad status

                return levelData;
            }
        }
    }
}
