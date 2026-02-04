using UnityEngine;
using Yarn.Unity;
public class FreezePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private DialogueRunner dialogueRunner;
    private PlayerController player;
    private SfxPlayer sfx;

    void Start()
    {
        Debug.Log(" Start running");
    }

    void Awake()
    {
        dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        player = GetComponent<PlayerController>();
        sfx = FindAnyObjectByType<SfxPlayer>();
        if(!player)
        {
            Debug.LogError("Cant find player controller");
            return;
        }
        if(!dialogueRunner)
        {
            Debug.LogError("Can not find Dialogue system");
            return;
        }
        if(!sfx)
        {
            Debug.LogError("Cant find sfx");
            return;
        }

        dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
    }

    void OnDialogueStart()
    {
        player.SetMovementInteractionEnabled(false);
        //Debug.LogError("Freezed Player");
        sfx.PlayDialogueUISFX();
        
    }
    void OnDialogueComplete()
    {
        player.SetMovementInteractionEnabled(true);
        //Debug.LogError("UnFreezed Player");
    }

}