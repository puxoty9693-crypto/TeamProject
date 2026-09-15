using UnityEngine;

public enum WorkerRole { Chef, Server, Cashier } // 요리사, 서빙원, 계산원

// Worker 한 명의 기본 정보
[CreateAssetMenu(fileName = "Worker_", menuName = "Data/Worker")]
public class WokerData : ScriptableObject
{
    public WorkerRole role;      // Worker 역할
    public string workerName;    // Worker 이름
    public Sprite workerImage;   // Worker 이미지
}
