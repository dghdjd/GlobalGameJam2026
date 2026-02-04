using UnityEngine;
using Yarn.Unity;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlayerInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Interaction Hotkey")]
    public Key interactKey;

    private DialogueRunner dialogueRunner;
    private NPC currentNpc;
    private List<NPC> nearbyNpcs = new List<NPC>();
    private DialoguePortrait portrait;
    void Awake()
    {
        dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        portrait = FindAnyObjectByType<DialoguePortrait>();

    }
    

    // Update is called once per frame
    void Update()
    {
        if (dialogueRunner == null)
        {
            Debug.LogError("Can find DialogueRunner");
            return;
        }
        if(!portrait)
        {
            Debug.LogError("Can not find Dialogue Portrait Script");
            return;
        }

        UpdateCurrentNpc();

        if (ShouldTriggerDialogue())
        {
            portrait.SetPortrait(currentNpc.DisplayName);
            dialogueRunner.StartDialogue(currentNpc.DisplayName);
        }
    }
    void UpdateCurrentNpc()
{
    if (nearbyNpcs.Count == 0)
    {
        currentNpc = null;
        return;
    }

    float minDist = float.MaxValue;
    NPC closest = null;

    foreach (var npc in nearbyNpcs)
    {
        float d = Vector2.Distance(transform.position, npc.transform.position);
        if (d < minDist)
        {
            minDist = d;
            closest = npc;
        }
    }

    currentNpc = closest;
}

    private bool ShouldTriggerDialogue()
    {
        bool correctKeyPressed = Keyboard.current[interactKey].wasPressedThisFrame;
        return currentNpc != null && correctKeyPressed && !dialogueRunner.IsDialogueRunning;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"Hit {other.gameObject.name}");
        var npc = other.GetComponent<NPC>();
        
        if(npc != null && !nearbyNpcs.Contains(npc))
        {
            nearbyNpcs.Add(npc);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log($"Left {other.gameObject.name}");

        var npc = other.GetComponent<NPC>();
        if(npc != null)
        {
            nearbyNpcs.Remove(npc);
        }
    }
}
