using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject dialogueRoot;

    [Header("Text")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;

    private string[] currentLines;
    private int currentLineIndex;

    public bool IsOpen => dialogueRoot != null && dialogueRoot.activeSelf;

    private void Awake()
    {
        EnsureDialogueRoot();
        Hide();
    }

    public void Show(string speakerName, string[] lines)
    {
        EnsureDialogueRoot();

        if (dialogueRoot == null)
        {
            return;
        }

        currentLines = lines;
        currentLineIndex = 0;

        if (speakerNameText != null)
        {
            speakerNameText.text = speakerName;
        }

        dialogueRoot.SetActive(true);
        RefreshLine();
    }

    public void AdvanceOrHide()
    {
        if (!IsOpen)
        {
            return;
        }

        currentLineIndex++;
        if (currentLines == null || currentLineIndex >= currentLines.Length)
        {
            Hide();
            return;
        }

        RefreshLine();
    }

    public void Hide()
    {
        EnsureDialogueRoot();

        if (dialogueRoot != null)
        {
            dialogueRoot.SetActive(false);
        }
    }

    private void RefreshLine()
    {
        if (dialogueText == null)
        {
            return;
        }

        bool hasLine = currentLines != null
            && currentLineIndex >= 0
            && currentLineIndex < currentLines.Length;

        dialogueText.text = hasLine ? currentLines[currentLineIndex] : string.Empty;
    }

    private void EnsureDialogueRoot()
    {
        if (dialogueRoot == null)
        {
            dialogueRoot = gameObject;
        }
    }
}
