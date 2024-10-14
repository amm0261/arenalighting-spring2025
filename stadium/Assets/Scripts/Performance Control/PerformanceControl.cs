// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;

public class PerformanceControl : MonoBehaviour
{
    private GameObject[] allLEDs;
    private bool isHalfDisabled = false;

    void Start()
    {
        // Cache all LED GameObjects at the start
        allLEDs = GetAllLEDs();
        
        // Attach the button click event to the method
    }


    GameObject[] GetAllLEDs()
    {
        string ledTag;
        ledTag = "LED";
        return GameObject.FindGameObjectsWithTag(ledTag);
    }

    public void DisableHalfLEDs()
    {
        //refresh LEDs
        GameObject[] allCopy = allLEDs;

        if (allCopy == null || allCopy.Length == 0)
        {
            Debug.LogWarning("No LEDs found!");
            return;
        }

        // Loop through the lights
        for (int i = 0; i < allCopy.Length; i++)
        {
            // Disable every second light
            if (i % 2 == 0)
            {
                allCopy[i].SetActive(!isHalfDisabled);
            }
        }

        isHalfDisabled = !isHalfDisabled;
    }
}

