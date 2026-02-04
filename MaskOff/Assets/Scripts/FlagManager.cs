using UnityEngine;

public class FlagManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // getting the dialogue runner
        var runner = FindAnyObjectByType<Yarn.Unity.DialogueRunner>();
        if (runner == null)
        {
            Debug.LogWarning("Was unable to find a dialogue runner");
            return;
        }

        // attempting to find a float called $gold
        if (runner.VariableStorage.TryGetValue<float>("$gold", out var gold))
        {
            // we found the variable
            // it's value has been stored into the gold parameter
            // we can now use the gold variable
            if (gold > 100)
            {
                Debug.Log("they are rich, unlock the Player Is Rich cheevo!");
            }
        }
        else
        {
            // we failed to find $gold
            Debug.LogWarning("Was unable to find a number value for $gold");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
