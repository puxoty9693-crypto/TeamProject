using UnityEngine;

public enum WorkerRole { Chef, Waiter, Cashier } // 요리사, 웨이터, 계산원

// NPC 한 명의 기본 정보
[CreateAssetMenu(fileName = "Worker_", menuName = "Data/Worker")]
public class WorkerData : ScriptableObject
{
    public WorkerRole role;      // Worker 역할
    public string WorkerName;    // Worker 이름
    public Sprite WorkerImage;   // Worker 이미지
}