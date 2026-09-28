using UnityEngine;

[CreateAssetMenu(fileName = "Worker_", menuName = "Data/NPC/Worker")]

public class WorkerData : NPCData
{
    [Header("Worker Stats")]
    [SerializeField] private WorkerStats baseStats = new();

    public WorkerStats BaseStats => baseStats;
}
