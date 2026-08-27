using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; private set; }
    public PlayerData CurrentData { get; private set; }

    private string SavePath = Application.persistentDataPath + "/save.json";

    private void Awake()
    {
        if(Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() => CurrentData = LoadGame();

    public void SaveGame() 
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(SavePath, json);
    }

    private PlayerData LoadGame() 
    {
        if(!File.Exists(SavePath)) return new PlayerData();
        return JsonUtility.FromJson<PlayerData>(File.ReadAllText(SavePath));
    }
    
        
    




}
