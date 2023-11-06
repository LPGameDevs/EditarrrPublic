using System.Runtime.InteropServices;
using Editarrr.Misc;
using Singletons;
using UI;
using UnityEngine;

namespace Editarrr.LevelEditor
{
    [CreateAssetMenu(fileName = "AchievementPool", menuName = "Pool/new Achievement Pool")]
    public class AchievementPool : ScriptableObject
    {
       [field: SerializeField] public PopupAchievement[] PopupAchievements { get; private set; }

        public PopupAchievement Get(GameAchievement type)
        {
            if (type == GameAchievement.None)
            {
                return null;
            }

            int length = this.PopupAchievements.Length;

            for (int i = 0; i < length; i++)
            {
                PopupAchievement achievement = this.PopupAchievements[i];

                if (achievement.Type == type)
                    return achievement;
            }

            return null;
        }
    }
}
