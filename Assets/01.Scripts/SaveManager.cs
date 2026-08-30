// 플레이어 진행 데이터(PlayerData)를 JSON 파일로 저장/로드
using System.IO;
using UnityEngine;

public class SaveManager : MMSingleton<SaveManager>
{
    public PlayerData CurrentData { get; private set; } // 현재 로드된 플레이어 데이터

    private string SavePath => Application.persistentDataPath + "/save.json"; // 세이브 파일 경로

    [SerializeField] private float autoSaveInterval = 30f;
    private float autoSaveTimer;

    private void Start()
    {
        CurrentData = LoadGame();
    }

    private void Update()
    {
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveInterval) 
        {
            autoSaveTimer = 0f;
            SaveGame();
        }
    }


    // 현재 데이터를 JSON으로 저장
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(SavePath, json);
    }

    // 세이브 파일을 읽어 PlayerData로 반환 (없으면 새 데이터)
    private PlayerData LoadGame()
    {
        if (!File.Exists(SavePath)) return new PlayerData();
        return JsonUtility.FromJson<PlayerData>(File.ReadAllText(SavePath));
    }

    // 앱이 백그라운드로 가거나 종료될 때도 저장 (모바일 대비)
    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }


}