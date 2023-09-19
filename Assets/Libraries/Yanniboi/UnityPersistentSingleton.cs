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

    protected virtual void Awake ()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad (transform.gameObject);
        }
        else
        {
            if(this != _instance)
            {
                Destroy(gameObject);
            }
        }
    }
}
