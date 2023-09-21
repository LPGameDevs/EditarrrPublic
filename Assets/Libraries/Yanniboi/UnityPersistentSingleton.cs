using UnityEngine;

public class UnityPersistentSingleton<T> : MonoBehaviour	where T : Component

{
    protected static T _instance;
    public float InitializationTime;

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

        InitializationTime=Time.time;

        DontDestroyOnLoad (this.gameObject);
        // we check for existing objects of the same type
        T[] check = FindObjectsOfType<T>();
        foreach (T searched in check)
        {
            if (searched!=this)
            {
                // if we find another object of the same type (not this), and if it's older than our current object, we destroy it.
                if (searched.GetComponent<UnityPersistentSingleton<T>>().InitializationTime<InitializationTime)
                {
                    Destroy (searched.gameObject);
                }
            }
        }

        if (_instance == null)
        {
            _instance = this as T;
        }
    }
}
