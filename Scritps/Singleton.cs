using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}