using UnityEngine;

public class PersistentGameManager : MonoBehaviour
{
    private static PersistentGameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}