using System.Collections.Generic;
using UnityEngine;


public enum DialogueOwner { Server, Chef, Cashier, Waiter }



[CreateAssetMenu(fileName = "Dialogue_", menuName = "Data/Dialogue")]
public class DialogueData : ScriptableObject
{
    public DialogueOwner owner;
    public List<string> lines;
}
