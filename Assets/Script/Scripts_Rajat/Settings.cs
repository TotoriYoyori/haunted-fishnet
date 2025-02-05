using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Toggle vSyncToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TextMeshProUGUI fpsText;

    private Resolution[] resolutions;
    private float deltaTime = 0.0f;

    void Start()
    {
        // Load available screen resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Load resolution settings
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load quality settings
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        // Initialize fullscreen and V-Sync toggle states
        fullScreenToggle.isOn = Screen.fullScreen;
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0; // If vSyncCount = 0, V-Sync is disabled

        // Add listeners
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetVSync(bool isEnabled)
    {
        // Enable V-Sync (1)
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
    }

    void Update()
    {
        // FPS Counter Logic - REMOVE IN PRODUCTION
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}