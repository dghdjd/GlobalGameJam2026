using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class ChangeScreen : MonoBehaviour
{
    [Header("Transition Overlay")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private Image blackImage;
    [SerializeField] private TMPro.TextMeshProUGUI overlayText;

    [Header("Characters")]
    [SerializeField] private Transform goblin;
    [SerializeField] private Transform cat;
    [SerializeField] private string targetPositionChildName = "SecondPosition";

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float blackoutDuration = 2f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private void Awake()
    {
        // Ensure fade canvas starts hidden
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(false);
        }
    }

    [YarnCommand("change_scene")]
    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    [YarnCommand("move_cat_and_goblin")]
    public static void MoveCatAndGoblin()
    {
        var changeScreen = FindFirstObjectByType<ChangeScreen>();
        changeScreen.StartCatAndGoblinTransition();
    }

    public void StartCatAndGoblinTransition()
    {
        StartCoroutine(CatAndGoblinTransitionRoutine());
    }

    private IEnumerator CatAndGoblinTransitionRoutine()
    {
        Transform goblinTransform = goblin != null ? goblin : GameObject.Find("Goblin")?.transform;
        Transform catTransform = cat != null ? cat : GameObject.Find("Cat")?.transform;

        // Get target positions from children before we move anything
        Vector3? goblinTargetPos = GetTargetPositionFromChild(goblinTransform);
        Vector3? catTargetPos = GetTargetPositionFromChild(catTransform);

        if (fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(true);

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / fadeInDuration;
                fadeCanvas.alpha = Mathf.Clamp01(t);
                yield return null;
            }
            fadeCanvas.alpha = 1f;
        }
        else if (blackImage != null)
        {
            blackImage.gameObject.SetActive(true);
            Color c = blackImage.color;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / fadeInDuration;
                c.a = Mathf.Clamp01(t);
                blackImage.color = c;
                yield return null;
            }
        }

        if (goblinTransform != null && goblinTargetPos.HasValue)
        {
            goblinTransform.position = goblinTargetPos.Value;
        }
        if (catTransform != null && catTargetPos.HasValue)
        {
            catTransform.position = catTargetPos.Value;
        }

        yield return new WaitForSeconds(blackoutDuration);

        if (fadeCanvas != null)
        {
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime / fadeOutDuration;
                fadeCanvas.alpha = Mathf.Clamp01(t);
                yield return null;
            }
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(false);
        }
        else if (blackImage != null)
        {
            Color c = blackImage.color;
            float t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime / fadeOutDuration;
                c.a = Mathf.Clamp01(t);
                blackImage.color = c;
                yield return null;
            }
            c.a = 0f;
            blackImage.color = c;
            blackImage.gameObject.SetActive(false);
        }
    }

    private Vector3? GetTargetPositionFromChild(Transform character)
    {
        if (character == null || character.childCount == 0)
            return null;

        Transform target = null;
        if (!string.IsNullOrEmpty(targetPositionChildName))
        {
            target = character.Find(targetPositionChildName);
        }
        if (target == null)
        {
            target = character.GetChild(0);
        }
        return target != null ? target.position : (Vector3?)null;
    }
}
