using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button menuButton;
    
    [Header("Settings")]
    [SerializeField] private Button closeGameButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Button closeMenuButton;
    
    [Header("Cancel Confirmation Popup")]
    [SerializeField] private GameObject closeConfirmationPanel;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button confirmCloseButton;
    [SerializeField] private Button cancelCloseButton;
    
    private bool isMenuOpen = false;
    private Coroutine volumeUpdateCoroutine;
    private const float VOLUME_UPDATE_DELAY = 0.1f;
    public static OptionsMenu Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }
    }
    
    void Start()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        
        if (closeConfirmationPanel != null)
        {
            closeConfirmationPanel.SetActive(false);
        }
        
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ToggleMenu);
        }
        
        if (closeGameButton != null)
        {
            closeGameButton.onClick.AddListener(ShowCloseConfirmation);
        }
        
        if (closeMenuButton != null)
        {
            closeMenuButton.onClick.AddListener(CloseMenu);
        }
        
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            UpdateVolumeText();
        }
        
        if (confirmCloseButton != null)
        {
            confirmCloseButton.onClick.AddListener(ConfirmClose);
        }
        
        if (cancelCloseButton != null)
        {
            cancelCloseButton.onClick.AddListener(CancelClose);
        }
    }
    
    public void OpenMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            isMenuOpen = true;
        }
    }
    
    public void CloseMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            isMenuOpen = false;
        }
    }
    
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    
    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }
    
    void OnVolumeSliderChanged(float value)
    {
        UpdateVolumeText();
        
        if (volumeUpdateCoroutine != null)
        {
            StopCoroutine(volumeUpdateCoroutine);
        }
        
        volumeUpdateCoroutine = StartCoroutine(DelayedVolumeUpdate(value));
    }
    
    IEnumerator DelayedVolumeUpdate(float value)
    {
        yield return new WaitForSeconds(VOLUME_UPDATE_DELAY);
        
        AudioListener.volume = value;
        
        volumeUpdateCoroutine = null;
    }
    
    void UpdateVolumeText()
    {
        if (volumeText != null && volumeSlider != null)
        {
            int volumePercent = Mathf.RoundToInt(volumeSlider.value * 100f);
            volumeText.text = "Volume: " + volumePercent.ToString();
        }
    }
    
    public void ShowCloseConfirmation()
    {
        if (closeConfirmationPanel != null)
        {
            closeConfirmationPanel.SetActive(true);
        }
    }
    
    public void HideCloseConfirmation()
    {   
        if (closeConfirmationPanel != null)
        {
            closeConfirmationPanel.SetActive(false);
        }
    }
    
    public void ConfirmClose()
    {
        Application.Quit();

        // If running in the Unity Editor, stop Play Mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void CancelClose()
    {
        HideCloseConfirmation();
    }
    
    void OnEnable()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            UpdateVolumeText();
        }
    }
}
