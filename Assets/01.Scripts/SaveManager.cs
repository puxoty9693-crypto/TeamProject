// 플레이어 진행 데이터(PlayerData)를 JSON 파일로 저장/로드
using System.IO;
using UnityEngine;

public class SaveManager : MMSingleton<SaveManager>
{
    public PlayerData CurrentData { get; private set; } // 현재 로드된 플레이어 데이터

    private string SavePath => Application.persistentDataPath + "/save.json"; // 세이브 파일 경로

    private void Start()
    {
        CurrentData = LoadGame();
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
}