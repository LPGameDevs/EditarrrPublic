using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LevelEditor.Storage
{
    public class LocalStorage : ILevelStorage
    {
        public Task<LevelSave> GetLevel(string code)
        {
            Task<LevelSave> levelData = Task.FromResult(new LevelSave());

            if (code.Length == 0)
            {
                return levelData;
            }

            string fileName = EditorLevelStorage.LevelStoragePath + code + ".json";
            if (!File.Exists(fileName))
            {
                return levelData;
            }

            string data = File.ReadAllText(fileName);
            if (data.Length < 3)
            {
                return levelData;
            }

            return Task.FromResult(JsonConvert.DeserializeObject<LevelSave>(data));

        }
    }
}
