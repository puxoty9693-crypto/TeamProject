using System;
using UnityEngine;

[Serializable]
public class WorkerStats
{
    [SerializeField] private float cooking = 1f;
    [SerializeField] private float serving = 1f;
    [SerializeField] private float cashing = 1f;    // 결제

    public float Cooking => cooking;
    public float Serving => serving;
    public float Cashing => cashing;

    public WorkerStats Clone()  // 원본 스탯 데이터 사용 X
    {
        return new WorkerStats
        {
            cooking = cooking,
            serving = serving,
            cashing = cashing
        };
    }

}
