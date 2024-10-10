// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;

public class PerformanceController : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Toggle performanceToggle;

    void Start()
    {
        // Attach the button click event to the method that disables half the LEDs
        disableHalfButton.onValueChanged.AddListener(delegate { DisableHalfLEDs(); });
    }

    GameObject[] GetAllLEDs()
    {
        string ledTag;
        ledTag = "LED";
        return GameObject.FindGameObjectsWithTag(ledTag);
    }

    void DisableHalfLEDs()
    {
        //Get all lights
        GameObject[] allLEDs = GetAllLEDs();

        // Loop through the lights
        for (int i = 0; i < allLEDs.Length; i++)
        {
            // Disable every second light
            if (i % 2 == 1)
            {
                allLEDs[i].SetActive(false);  // Enable or disable based on the passed bool
            }
        }
    }
}

