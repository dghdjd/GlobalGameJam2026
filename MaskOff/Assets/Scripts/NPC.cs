using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum Name
    {
        Pumpkin,
        Goblin,
        Witch,
        Ghost,
        Cat,
        Frankenstein,
        Fridge,
        Cauldron,
        Bookshelf,
        Bathroom
    }
    public Name NPCName;
    public string DisplayName => NPCName.ToString();


}
