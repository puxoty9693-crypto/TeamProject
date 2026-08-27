using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
  
}
