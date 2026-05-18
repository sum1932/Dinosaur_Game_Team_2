using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCDialogueInteractable : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private string speakerName = "NPC";
    [TextArea(2, 4)]
    [SerializeField] private string[] dialogueLines =
    {
        "안녕! 이곳에 온 걸 환영해."
    };

    [Header("UI")]
    [SerializeField] private GameObject promptObject;
    [SerializeField] private DialogueUI dialogueUI;

    private PlayerMovement nearbyPlayer;
    private bool warnedMissingDialogueUI;

    private void Reset()
    {
        Collider interactionCollider = GetComponent<Collider>();
        interactionCollider.isTrigger = true;
    }

    private void Awake()
    {
        Collider interactionCollider = GetComponent<Collider>();
        if (interactionCollider != null)
        {
            interactionCollider.isTrigger = true;
        }

        SetPromptVisible(false);
    }

    private void OnDisable()
    {
        UnsubscribeFromPlayer();
        SetPromptVisible(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryRegisterPlayer(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryRegisterPlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player == null || player != nearbyPlayer)
        {
            return;
        }

        if (dialogueUI != null)
        {
            dialogueUI.Hide();
        }

        UnsubscribeFromPlayer();
        SetPromptVisible(false);
    }

    private void HandleInteractPressed()
    {
        ResolveDialogueUI();

        if (dialogueUI == null)
        {
            if (!warnedMissingDialogueUI)
            {
                Debug.LogWarning($"{name} cannot open dialogue because Dialogue UI is not assigned.");
                warnedMissingDialogueUI = true;
            }

            return;
        }

        if (dialogueUI.IsOpen)
        {
            dialogueUI.AdvanceOrHide();
        }
        else
        {
            dialogueUI.Show(speakerName, dialogueLines);
        }

        UpdatePromptVisibility();
    }

    private void UpdatePromptVisibility()
    {
        bool shouldShowPrompt = nearbyPlayer != null
            && (dialogueUI == null || !dialogueUI.IsOpen);

        SetPromptVisible(shouldShowPrompt);
    }

    private void SetPromptVisible(bool isVisible)
    {
        if (promptObject != null)
        {
            promptObject.SetActive(isVisible);
        }
    }

    private void UnsubscribeFromPlayer()
    {
        if (nearbyPlayer != null)
        {
            nearbyPlayer.InteractPressed -= HandleInteractPressed;
            nearbyPlayer = null;
        }
    }

    private void TryRegisterPlayer(Collider other)
    {
        PlayerMovement player = other.GetComponentInParent<PlayerMovement>();
        if (player == null || nearbyPlayer == player)
        {
            return;
        }

        UnsubscribeFromPlayer();
        nearbyPlayer = player;
        nearbyPlayer.InteractPressed += HandleInteractPressed;
        ResolveDialogueUI();
        UpdatePromptVisibility();
    }

    private void ResolveDialogueUI()
    {
        if (dialogueUI != null)
        {
            return;
        }

        dialogueUI = Object.FindFirstObjectByType<DialogueUI>(FindObjectsInactive.Include);
    }
}
