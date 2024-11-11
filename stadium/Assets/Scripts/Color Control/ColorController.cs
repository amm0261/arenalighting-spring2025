using System.Collections.Generic;
// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ColorController : MonoBehaviour
{
    // UI Elements
    // Make sure to attach these Buttons in the Inspector
    public Toggle fadeToggle;       // Toggle for enabling and disabling fading
    public Toggle randomToggle;     // Toggle for enable and disabling random color generation
    public InputField hexCodeInput; // Input for hex code color codes
    public InputField fadeTimeInput;   // Input for duration of fade for color crossfade
    public TMP_Text errorMessage;       // Text field for displaying error messages

    // Gradient properties
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    Color? hexCodeColor;    // Stores hex color values

    // State Variables
    private Dictionary<string, GameObject[]> activeLEDsCached = new Dictionary<string, GameObject[]>();  // Caches active LEDs for each section
    private bool colorNeedsUpdate = false;  // Flag to check if color needs to be updated.
    private Color lastColor;                // Stores the last color that was applied to the LEDs.
    bool isRandomizing;                     // Flag to check if random color generation is enabled
    public bool isFading;                   // Flag to check if a color fade is in progress

    // Fade properties
    [SerializeField]
    public Color fadeEndColor;              // Color that the fade will end on
    [SerializeField]
    public float fadeFrame;                 // Current frame of the fade
    [SerializeField]
    public float fadeDuration = 1.0f;       // Duration of the fade in seconds
    [SerializeField]
    float fadeTime;                         // Time spent fading
    public SelectorController selectorController;

    void Start()
    {
        fadeFrame = 0.0f;
        fadeDuration = 1.0f;

        // Sets up listeners for UI inputs
        hexCodeInput.onEndEdit.AddListener(OnEditHexCodeString);
        fadeTimeInput.onEndEdit.AddListener(OnSetFadeTime);
        randomToggle.onValueChanged.AddListener(delegate {
            OnContinualRandomToggle(randomToggle.isOn);
        });

        isRandomizing = false;

        // Find the GameObject with the SelectorController component
        GameObject selectorControllerObject = GameObject.Find("SelectorController");

        // Check if the GameObject was found
        if (selectorControllerObject != null)
        {
            // Get the SelectorController component from the GameObject
            selectorController = selectorControllerObject.GetComponent<SelectorController>();
        }

        // Check whether current section is checked or not (also happens in update)
        CacheLEDGroups();
    }
    void Update()
    {
        if (isFading)
        {
        GradientColorFade();
        }
    }
    // Handles UI Input for fade time
    public void OnSetFadeTime(string fadeTime)
    {
        fadeDuration = float.Parse(fadeTime); // Sets fade duration based on input
    }

    public void OnContinualRandomToggle(bool toggleState)
    {
        // Start or stop random color geenration based on toggle state
        if (toggleState)
        {
            if (!isRandomizing)
            {
                InvokeRepeating("SetRandom", 0.0f, 0.5f); // Calls SetRandom every 0.5 seconds
                isRandomizing = true; // Updates state to ensure continual randomized colors
            }
        }
        else
        {
            CancelInvoke(); // Stop random color updates
            isRandomizing = false; // Updates state to prevent additional color randomization.
        }
    }

    // LED Management
    // Handles updating the LED cache.
    void CacheLEDGroups()
    {
        activeLEDsCached.Clear(); // Clears the cache to prevent duplicates
        foreach (string sectionName in selectorController.sectionNames)
        {
            SectionCollider sectioncollider = GameObject.Find(sectionName)?.GetComponent<SectionCollider>(); // Gets all LEDs in the section
            activeLEDsCached[sectionName] = sectioncollider.sectionLEDs; // Adds LEDs to the cache
        }
    }

    GameObject[] GetCachedLEDs()
    {
        List<GameObject> allActiveLEDs = new List<GameObject>(); // List to store all active LEDs
        foreach (var section in activeLEDsCached.Values) // Iterates through each section in the cache
        {
            allActiveLEDs.AddRange(section); // Adds all LEDs in the section to the list
        }
        return allActiveLEDs.ToArray(); // Returns the list as an array
    }


    void GradientColorFade()
    {
        GameObject[] allActiveLEDs = GetCachedLEDs(); // Gets LEDs from the cache

        if (isFading)
        {
            fadeTime += Time.deltaTime;
            fadeFrame = fadeTime / fadeDuration;
            Color frameColor = gradient.Evaluate(fadeFrame);
            if (fadeTime >= fadeDuration)
            {
                isFading = false;
                frameColor = gradient.Evaluate(1.0f);
            }

            foreach (GameObject LED in allActiveLEDs)
            {
                LED.GetComponent<Renderer>().material.color = frameColor;
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", frameColor);
            }
        }
    }

    void UpdateLEDColors(GameObject[] leds, Color newColor)
    {
        // Debugging
        Debug.Log("Starting update function");
        Debug.Log(leds[0].GetComponent<Renderer>().material.color);
        Debug.Log(leds[0].GetComponent<Renderer>().material.GetColor("_EmissionColor"));

        if (!fadeToggle.isOn)
        {
            ApplyColorUpdates(newColor); // Directly apply color
        }
        else
        {
            StartFading(allActiveLEDs, newColor); // Start fading process

        }
    }
    void StartFading(GameObject[] leds, Color endcolor)
    {
        // Prepare gradient for fade effect
        GameObject firstLED = leds[0];
            Color startColor = firstLED.GetComponent<Renderer>().material.color; // Gets current color of the first LED as the start color for the fade
            gradient = new Gradient(); // Initialize gradient
            colorKey = new GradientColorKey[2]; // Set up color keys
            colorKey[0].color = startColor; // Start color
            colorKey[0].time = 0.0f; // At time 0.0 seconds
            colorKey[1].color = endColor; // End color
            colorKey[1].time = fadeDuration; // At time equal to fade duration

            alphaKey = new GradientAlphaKey[2]; // Set up alpha keys
            alphaKey[0].alpha = 0.5f; // Start alpha
            alphaKey[0].time = 0.0f; // At time 0.0 seconds
            alphaKey[1].alpha = 0.5f; // End alpha
            alphaKey[1].time = fadeDuration; // At fade duration

            gradient.SetKeys(colorKey, alphaKey); // Set gradient keys
            fadeEndColor = endColor; // Set end color for fading
            fadeFrame = 0.0f; // Reset fade frame
            fadeTime = 0.0f;  // Reset fade time
            isFading = true;  // Set fade flag to true to start fading process
    }
    void ColorFade()
    {
        var startTime = Time.realtimeSinceStartup; // Measure time taken for fading calculations

        if (isFading)
        {
            fadeTime += Time.deltaTime; // Increment fade time
            fadeFrame = fadeTime / fadeDuration; // Calculate fade frame based on time and duration
            Color frameColor = gradient.Evaluate(fadeFrame); // Get color for current frame

            ApplyColorUpdates(frameColor); // Apply the calculated color

            // Check if fade duration has been completed
            if (fadeTime >= fadeDuration)
            {
                isFading = false; // Reset fading flag
                ApplyColorUpdates(gradient.Evaluate(1.0f)); // Apply final color
            }
        }
        var elapsedTime = Time.realtimeSinceStartup - startTime; // Calculate elapsed time for fade calculations
        Debug.Log($"Gradient calculations took: {elapsedTime * 1000} ms"); //log time taken for performance monitoring
    }

    void ApplyColorUpdates(Color colorToApply)
    {
        //Apply color to all LEDs in the array

        foreach (GameObject LED in GetCachedLEDs())
        {
            Renderer ledRenderer = LED.GetComponent<Renderer>(); // Get renderer of the LED
            ledRenderer.material.color = colorToApply; // Set material color
            ledRenderer.material.SetColor("_EmissionColor", colorToApply); // Set emission color
        }

        // Debugging
        Debug.Log("Ending update function");
        Debug.Log(allLEDs[0].GetComponent<Renderer>().material.color);
        Debug.Log(allLEDs[0].GetComponent<Renderer>().material.GetColor("_EmissionColor"));

    }

    // Set predefined colors for LEDS
    public void OnSetBlue1()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.012f, 0.141f, 0.302f, .500f);
        GameObject[] allLEDs = GetCachedLEDs(); // Get all LEDs
        UpdateLEDColors(allLEDs, newColor);
    }

    public void OnSetBlue2()
    {
        // string htmlValue = "#03244d";
        Color newColor =  new Color(0.2862745f, 0.4313726f, 0.6117647f, .5f);  // Define color for Blue 2
        GameObject[] allLEDs = GetCachedLEDs(); // Get all LEDs
        UpdateLEDColors(allLEDs, newColor); // Update color
    }

    public void OnSetOrange1()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.8666667f, 0.3333333f, 0.04705882f, .5f);  // Define color for Orange 1
        GameObject[] allLEDs = GetCachedLEDs();
        UpdateLEDColors(allLEDs, newColor); // Update color
    }

    public void OnSetOrange2()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.9647059f, 0.5019608f, 0.1490196f, .5f); // Define color for Orange 2
        GameObject[] allLEDs = GetCachedLEDs();
        UpdateLEDColors(allLEDs, newColor); // Update color
    }

    // Handle editing the hex code color codes
    public void OnEditHexCodeString(string hexCodeString)
    {
        hexCodeColor = null; // Reset hex color
        // Validates hex input
        if (hexCodeString == null || hexCodeString == "")
        {
            HideErrorMessage();
            return;
        }

        if (hexCodeString.Length != 6)
        {
            ShowErrorMessage("Error: Inputs must be exactly 6 characters long."); // Display error message if length is invalid
            return;
        }

        // Try to parse the hex code string to a color
        if (ColorUtility.TryParseHtmlString("#" + hexCodeString, out Color newColor))
        {
            hexCodeColor = newColor; // Store valid color
            HideErrorMessage();
        }
        else
        {
            ShowErrorMessage("Error:" + hexCodeString + " is not a valid hexadecimal value."); // Show error for invalid input
            Debug.Log("Error: " + hexCodeString + " is not a valid hexadecimal value."); // Log error (probably not needed)
        }
    }

    // Apply color based on hex code input
    public void OnSetHexCodeColor()
    {
        if (!hexCodeColor.HasValue) { return; } // Check if color is valid
        GameObject[] allLEDs = GetCachedLEDs(); // Get all LEDs
        UpdateLEDColors(allLEDs, hexCodeColor.Value); // Update LED colors
    }
    // Random Color Generation
    public void OnSetRandom()
    {
        // Function specifically made to handle "Random" button call.
        SetRandom();
    }

    void SetRandom()
    {
        isFading = false; // Stop any fading effect

        Color newColor; // Define a new color
        GameObject[] allLEDs = GetCachedLEDs(); // Get all LEDs
        foreach (GameObject LED in allLEDs)
        {
            // Generate color based on given a range
            int randomCol = Random.Range(1, 6);
            if (randomCol < 3)
            {
                newColor = new Color(0.2862745f, 0.4313726f, 0.6117647f, .5f);
            }
            else if (randomCol > 4)
            {
                newColor = new Color(1.0f, 1.0f, 1.0f, .5f);
            }
            else
            {
                newColor = new Color(0.8666667f, 0.3333333f, 0.04705882f, .5f);
            }
            LED.GetComponent<Renderer>().material.color = newColor; // Set LED color
            LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor); // Set emission color
        }
    }
    // Handles Errors and providing a message about the error to the user.
    void ShowErrorMessage(string message)
    {
        errorMessage.text = message; // Set error message text
        errorMessage.gameObject.SetActive(true); // Show error message
    }
    void HideErrorMessage()
    {

        errorMessage.gameObject.SetActive(false); // Hide error message
    }
}
