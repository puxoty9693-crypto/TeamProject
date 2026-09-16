using UnityEngine;
// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "Worker_", menuName = "Data/NPC/Worker")]

public class WorkerData : NPCData
{
    [Header("Worker Stats")]
    [SerializeField] private WorkerStats baseStats = new();

    public WorkerStats BaseStats => baseStats;
    public WorkerRole role;      // Worker 역할
    public string workerName;    // Worker 이름
    public Sprite workerImage;   // Worker 이미지
}