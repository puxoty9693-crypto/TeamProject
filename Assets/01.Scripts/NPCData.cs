using UnityEngine;

public enum NPCRole { Chef, Server, Cashier}

[CreateAssetMenu(fileName ="NPC_", menuName ="Data/NPC")]
public class NPCData : ScriptableObject
{
    public NPCRole role;
    public string npcName;
    public Sprite npcImage;
}
