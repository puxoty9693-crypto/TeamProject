using UnityEngine;

// HousingGrid.Place()로 생성된 오브젝트에 붙는 식별 정보
public class PlacedGridObject : MonoBehaviour
{
    public string ObjId { get; private set; }       // 타입 ("table", "decor_1" 등)
    public string InstanceId { get; private set; }  // 개별 인스턴스 ID ("table_0" 등)

    public void Init(string objId, string instanceId)
    {
        ObjId = objId;
        InstanceId = instanceId;
    }
}