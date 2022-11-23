using System.Threading.Tasks;

namespace LevelEditor.Storage
{
    public interface ILevelStorage
    {
        Task<LevelSave> GetLevel(string code);

    }
}
