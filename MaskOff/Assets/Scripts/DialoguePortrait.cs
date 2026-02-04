using UnityEngine;
using UnityEngine.UI;

public class DialoguePortrait : MonoBehaviour
{
    public Image characterImage;

    public void SetPortrait(string npcName)
    {
        Sprite sprite = Resources.Load<Sprite>("Portraits/" + npcName);

        if (sprite != null)
        {
            characterImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Portrait not found for " + npcName);
        }
    }
}
