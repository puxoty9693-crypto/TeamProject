using UnityEngine;

public enum WorkerRole { Chef, Waiter, Cashier } // 요리사, 웨이터, 계산원

// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "Worker_", menuName = "Data/Worker")]
public class WorkerData : ScriptableObject
{
    public WorkerRole role;      // Worker 역할
    public string workerName;    // Worker 이름
    public Sprite workerImage;   // Worker 이미지
}