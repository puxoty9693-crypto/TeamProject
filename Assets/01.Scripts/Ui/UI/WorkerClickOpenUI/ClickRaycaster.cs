using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRaycaster : MonoBehaviour
{
    [SerializeField] LayerMask clickableLayers;

    private void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, clickableLayers);

        if (hit.collider == null)
        {
            return;
        }
        var npcSlot = hit.collider.GetComponent<WorkerWorldSlot>();
        if (npcSlot != null)
        {
            npcSlot.OpenPopup();
            return;
        }

        var tableSlot = hit.collider.GetComponent<PlaceableObjectClickHandler>();
        if (tableSlot != null)
        {
            tableSlot.OnClicked();
        }
    }
}