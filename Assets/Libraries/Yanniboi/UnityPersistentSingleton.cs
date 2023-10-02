using UnityEngine;

public class UnityPersistentSingleton<T> : MonoBehaviour	where T : Component

{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T> ();
                if (_instance == null)
                {
                    GameObject obj = new GameObject ();
                    _instance = obj.AddComponent<T> ();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this as T;
            DontDestroyOnLoad(GetRootGameObject(gameObject));
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    private GameObject GetRootGameObject(GameObject gameObject)
    {
        if (!gameObject.transform.parent)
            return gameObject;
        else
            return (GetRootGameObject(gameObject.transform.parent.gameObject));
    }
}