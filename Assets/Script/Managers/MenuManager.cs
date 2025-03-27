using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    
    [Header("Volume Settings")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundAudioSource;

    [Header("Display Settings")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle windowedToggle;

    [Header("Misc Settings")]
    [SerializeField] private string mainMenuSceneName = "Menu";
    [SerializeField] private string gameSceneName = "Enzo";

    private void Start()
    {
        InitializeSettings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName); 
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void SaveSettings()
    {
        AudioListener.volume = musicVolumeSlider.value;
        musicAudioSource.volume = musicVolumeSlider.value;
        soundAudioSource.volume = soundVolumeSlider.value;

        bool isFullscreen = fullscreenToggle.isOn;
        bool isWindowed = windowedToggle.isOn;

        Screen.fullScreen = isFullscreen;
        Screen.SetResolution(GetScreenWidthFromDropdown(), GetScreenHeightFromDropdown(), isFullscreen);

        Debug.Log("Settings Saved.");
    }

    public void SetVolume(float value)
    {
        musicAudioSource.volume = musicVolumeSlider.value;
        soundAudioSource.volume = soundVolumeSlider.value;
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    public void SetWindowed(bool value)
    {
        Screen.fullScreen = !value;
    }

    private void InitializeSettings()
    {
        musicVolumeSlider.value = musicAudioSource.volume;
        soundVolumeSlider.value = soundAudioSource.volume;

        fullscreenToggle.isOn = Screen.fullScreen;

        InitializeResolutionDropdown();

        windowedToggle.isOn = !Screen.fullScreen;
    }

    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        Resolution[] resolutions = Screen.resolutions;
        var options = new System.Collections.Generic.List<string>();

        foreach (var resolution in resolutions)
        {
            options.Add(resolution.width + " x " + resolution.height);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GetResolutionDropdownIndex(Screen.width, Screen.height);
    }

    private int GetResolutionDropdownIndex(int screenWidth, int screenHeight)
    {
        Resolution[] resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == screenWidth && resolutions[i].height == screenHeight)
                return i;
        }

        return 0;
    }

    private int GetScreenWidthFromDropdown()
    {
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        int width = int.Parse(selectedResolution.Split('x')[0].Trim());
        return width;
    }

    private int GetScreenHeightFromDropdown()
    {
        string selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        int height = int.Parse(selectedResolution.Split('x')[1].Trim());
        return height;
    }
}
