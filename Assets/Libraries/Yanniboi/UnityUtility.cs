using UnityEngine;

namespace Yanniboi
{
    public static class UnityUtility
    {
        public static bool IsNull<T>(this T myObject, string message = "", bool logError = false) where T : class
        {
            if (myObject is Object obj)
            {
                if (!obj)
                {
                    if (logError)
                    {
                        Debug.LogError("The object is null! " + message);
                    }

                    return false;
                }
            }
            else
            {
                if (myObject == null)
                {
                    if (logError)
                    {
                        Debug.LogError("The object is null! " + message);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
