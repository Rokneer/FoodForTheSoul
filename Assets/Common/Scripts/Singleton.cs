using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(true);
            }
            else if (instance != FindObjectOfType<T>(true))
            {
                Destroy(FindObjectOfType<T>(true));
            }
            return instance;
        }
    }
}
