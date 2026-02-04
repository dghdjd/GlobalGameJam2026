using System.Threading;
using UnityEngine;
using Yarn.Markup;
using TMPro;
using Yarn.Unity;


public class LineSoundHandler : ActionMarkupHandler
{
    [Header("Drag Line Presenter's Character: Name Field here")]
    [SerializeField] private TMP_Text characterNameField;

    private SfxPlayer sfx;
    public void Awake()
    {
        sfx = FindAnyObjectByType<SfxPlayer>();
    }


    public override void OnLineDisplayBegin(MarkupParseResult line, TMP_Text text)
    {
        if(!characterNameField)
        {
            Debug.LogError("no characterNameField");
            return;
        }
        if(!sfx)
        {
            Debug.LogError(" No sfx Error");
            return;
        }

        string name = characterNameField.text;
        sfx.PlayVoice(name);
        return;
    }

    public override void OnLineDisplayComplete()
    {
        if(!characterNameField || !sfx)
        {
            Debug.LogError("LineSoundHandler OnLineDisplayComplete Error");
            return;
        }
        sfx.EndVoice();
        return;
    }

    public override void OnPrepareForLine(MarkupParseResult line, TMP_Text text)
    {
        return;
    }

    public override YarnTask OnCharacterWillAppear(int currentCharacterIndex, MarkupParseResult line, CancellationToken cancellationToken)
    {
        return YarnTask.CompletedTask;
    }

    public override void OnLineWillDismiss()
    {
        return;
    }
}


