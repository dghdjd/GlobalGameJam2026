using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;


public class OptionHighlightWatcher : MonoBehaviour
{
    private SfxPlayer sfx;

    private OptionItem lastHighlighted;

    void Awake()
    {
        sfx = FindAnyObjectByType<SfxPlayer>();
    }

    void Update()
    {
        if(!sfx) return;

        OptionItem highlighted = null;

        var items = FindObjectsByType<OptionItem>(FindObjectsSortMode.None);
        foreach (var it in items)
        {
            if (!it.isActiveAndEnabled) continue;
            if (it.IsHighlighted)
            {
                highlighted = it;
                break;
            }
        }

        if (highlighted != null && highlighted != lastHighlighted)
        {
            sfx.PlayUIHover();
            lastHighlighted = highlighted;
        }

        if (highlighted == null)
        {
            lastHighlighted = null;
        }
        
        bool submit = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame ||
                        Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;

        if(highlighted && submit) sfx.PlayUIClick();
    }
}
