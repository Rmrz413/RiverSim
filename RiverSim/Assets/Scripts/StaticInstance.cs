using UnityEngine;

/// <summary>
/// Static instance, upon a new instace it overrides the current instance. Resets the state.
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
            }
            
            return instance;
        }
    }
    protected virtual void Awake() => instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        instance = null;
        Destroy(this);
    }
}

/// <summary>
/// Static instance turned into singleton. This will destroy any new versions created.
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else if (Instance == null) base.Awake();
    }
}

/// <summary>
/// Persistent singleton between sessions.
/// </summary>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);        
    }
}
