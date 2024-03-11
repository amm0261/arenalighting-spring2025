using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    // Variable to keep track of the currently selected camera control type
    private int currentSelectedType = 0;
    
    // List of camera view modes
    private List<string> cameraViewModes = new List<string> { "Fixed", "Dynamic", "Free" };

    // Reference to UI text displaying the camera control type
    public TMP_Text cameraControlTypeText;
    
    // References to UI panels controlling different camera modes
    public GameObject fixedCameraPositionPanel;
    public GameObject dynamicCameraControlPanel;

    // Reference to free camera control script
    [SerializeField]
    FreeCameraControl freeCameraControl;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Initialize UI with the current camera control type
        UpdateUI(cameraViewModes[currentSelectedType]);
    }

    // Method to switch camera control type to the left
    public void ControlTypeLeft()
    {
        if (currentSelectedType == 0)
        {
            currentSelectedType = cameraViewModes.Count - 1;
        } else
        {
            currentSelectedType -= 1;
        }

        // Update UI with the new camera control type
        UpdateUI(cameraViewModes[currentSelectedType]);
    }

    // Method to switch camera control type to the right
    public void ControlTypeRight()
    {
        if (currentSelectedType == cameraViewModes.Count - 1)
        {
            currentSelectedType = 0;
        } else
        {
            currentSelectedType += 1;
        }

        // Update UI with the new camera control type
        UpdateUI(cameraViewModes[currentSelectedType]);
    }

    // Method to update the UI based on the selected camera control type
    private void UpdateUI(string cameraViewMode)
    {
        // Update the camera control type text
        cameraControlTypeText.text = cameraViewMode;

        // Enable/disable UI panels based on the camera view mode
        if (cameraViewMode == "Fixed")
        {
            DisableDynamicMode();
            DisableFreeMode();
            EnableFixedMode();
        }
        else if (cameraViewMode == "Dynamic")
        {
            DisableFixedMode();
            DisableFreeMode();
            EnableDynamicMode();
        }
        else
        {
            DisableFixedMode();
            DisableDynamicMode();
            EnableFreeMode();
        }
    }

    // Method to enable the fixed camera mode UI panel
    private void EnableFixedMode()
    {
        fixedCameraPositionPanel.SetActive(true);
    }

    // Method to disable the fixed camera mode UI panel
    private void DisableFixedMode()
    {
        fixedCameraPositionPanel.SetActive(false);
    }

    // Method to enable the dynamic camera mode UI panel
    private void EnableDynamicMode()
    {
        dynamicCameraControlPanel.SetActive(true);
    }

    // Method to disable the dynamic camera mode UI panel
    private void DisableDynamicMode()
    {
        dynamicCameraControlPanel.SetActive(false);
    }

    // Method to enable free camera mode
    private void EnableFreeMode()
    {
        freeCameraControl.Enable();
    }

    // Method to disable free camera mode
    private void DisableFreeMode()
    {
        freeCameraControl.Disable();
    }
}
