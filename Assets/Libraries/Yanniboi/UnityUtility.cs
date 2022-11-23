using UnityEngine;

namespace Yanniboi
{
    public static class UnityUtility
    {
        public static bool IsNull<T>(this T myObject, string message = "") where T : class
        {
            if (myObject is Object obj)
            {
                if (!obj)
                {
                    Debug.LogError("The object is null! " + message);
                    return false;
                }
            }
            else
            {
                if (myObject == null)
                {
                    Debug.LogError("The object is null! " + message);
                    return false;
                }
            }

            return true;
        }
    }
}
