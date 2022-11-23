using System.Collections.Generic;

namespace LevelEditor.Storage
{
    public class DatabaseSettings
    {
        public DatabaseSettings(string baseUrl, string auth)
        {
            BaseUrl = baseUrl;
            Auth = auth;
        }

        public string BaseUrl { get; }
        public string Auth { get; }
    }
}
