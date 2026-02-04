using UnityEngine;
using Yarn.Unity;
public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    //Clips
    [Header("Clips")]
    public AudioClip click;
    public AudioClip select;
    public AudioClip dialogueOpen;
    public AudioClip dialogueClose;
    public AudioClip alien;
    public AudioClip ghost;
    public AudioClip goblin;
    public AudioClip pumpkin;
    public AudioClip witch;
    public AudioClip franky;
    public AudioClip bc;
    public AudioClip footstep;



    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 1f;

    private AudioSource oneShotSource;
    private AudioSource footstepLoop;
    private AudioSource dialogueLoop;

    [YarnCommand("play_dialogue")]
    public static void PlayDialogueSFX(string Name)
    {
        return;
    }




    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // oneShotSource
        oneShotSource = GetComponent<AudioSource>();
        if (oneShotSource == null) oneShotSource = gameObject.AddComponent<AudioSource>();
        oneShotSource.playOnAwake = false;
        oneShotSource.spatialBlend = 0f;

        // loop source for footstep
        footstepLoop = gameObject.AddComponent<AudioSource>();
        footstepLoop.playOnAwake = false;
        footstepLoop.spatialBlend = 0f;
        footstepLoop.loop = true;

        // loop source for footstep
        dialogueLoop = gameObject.AddComponent<AudioSource>();
        dialogueLoop.playOnAwake = false;
        dialogueLoop.spatialBlend = 0f;
        dialogueLoop.loop = true;


    }




//Private Functions:
private void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        oneShotSource.PlayOneShot(clip, volume * volume);
    }

private void PlayLoop(AudioSource source, AudioClip clip)
    {
        if (clip == null) return;

        if (source.isPlaying && source.clip == clip) return;

        source.Stop();
        source.clip = clip;
        source.volume = volume * volume;
        source.loop = true;
        source.Play();
    }

private void StopLoop(AudioSource source)
    {
        source.Stop();
        source.clip = null;
    }




// Public functions:
public void PlayDialogueUISFX()
    {
        PlayOneShot(dialogueOpen);
    }
public void PlayFootstep()
    {
        PlayLoop(footstepLoop, footstep);
    }
public void EndFootstep()
    {
        StopLoop(footstepLoop);
    }

public void PlayVoice(string npcName)
    {
        AudioClip clip = npcName switch
        {
            "Neil-A" => alien,
            "Ghost" => ghost,
            "Goblin" => goblin,
            "Pumpkin" => pumpkin,
            "Witch" => witch,
            "Frankenstein" => franky,
            "Cat" => bc,
            _ => null
        };

        if (clip == null)
        {
            Debug.LogError("clip is null");
            return;
        }

        PlayLoop(dialogueLoop, clip);
    }
    public void EndVoice()
    {
        dialogueLoop.Stop();
        dialogueLoop.clip = null;
    }

    public void PlayUIHover()
    {
        PlayOneShot(select);
    }
    public void PlayUIClick()
    {
        PlayOneShot(click);
    }
}