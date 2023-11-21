using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ReferenceFinder
{
    [MenuItem("Tools/Find Missing references in scene")]
    public static void FindMissingReferences()
    {
        GameObject[] objectsInScene = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in objectsInScene)
        {
            var components = go.GetComponents<Component>();

            foreach (var c in components)
            {
                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                        {
                            ShowError(FullObjectPath(go), sp.name);
                        }
                    }
                }
            }
        }

        Debug.Log("Finished searching through game objects in scnee");
    }

    private static void ShowError(string objectName, string propertyName)
    {
        Debug.LogError("Missing reference found in: " + objectName + ", Property : " + propertyName);
    }

    private static string FullObjectPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : FullObjectPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}
