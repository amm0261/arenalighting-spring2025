// To use this example, attach this script to an empty GameObject.
// Create three buttons (Create>UI>Button). Next, select your
// empty GameObject in the Hierarchy and click and drag each of your
// Buttons from the Hierarchy to the Your First Button, Your Second Button
// and Your Third Button fields in the Inspector.
// Click each Button in Play Mode to output their message to the console.
// Note that click means press down and then release.

using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    //Hold references to UI elements in the inspector
    public Toggle fadeToggle;
    public Toggle randomToggle;
    public InputField hexCodeInput;
    public InputField speedInput;

    //Variables for managing color fading gradient
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    Color? hexCodeColor;

    //Variables for controlling fading and randomizing
    [SerializeField]
    public bool fading;
    [SerializeField]
    public Color fadeEnd;
    [SerializeField]
    public float fadeFrame;
    [SerializeField]
    public float fadeDuration;
    [SerializeField]
    float fadeTime;
    [SerializeField]
    bool randomizing;

    void Start()
    {
        //Var init
        fadeFrame = 0.0f;
        fadeDuration = 1.0f;

        //Add listeners for the input fields and toggles to get user inputs
        hexCodeInput.onEndEdit.AddListener(OnEditHexCodeString);
        speedInput.onEndEdit.AddListener(OnSetSpeed);
        randomToggle.onValueChanged.AddListener(delegate {
            OnContinualRandomToggle(randomToggle.isOn);
        });

        randomizing = false;
    }

    void Update()
    {
        GradientColorFade();
    }

    //Take in user input, parse it and set speed of color fade
    public void OnSetSpeed(string speed)
    {
        fadeDuration = float.Parse(speed);
    }

    //Toggle for continual randomization
    public void OnContinualRandomToggle(bool toggleState)
    {
        if (toggleState)
        {
            if (!randomizing)
            {
                InvokeRepeating("SetRandom", 0.0f, 0.5f);
                randomizing = true;
            }
        }
        else
        {
            CancelInvoke();
            randomizing = false;
        }
    }

    // Method to get all LEDs in the scene
    GameObject[] GetAllLEDs()
    {
        string ledTag;
        ledTag = "LED";

        // Finding and returning all objects with LED tag
        return GameObject.FindGameObjectsWithTag(ledTag);
    }

    // Method to perform color fading based on current state
    void GradientColorFade()
    {
        GameObject[] allLEDs = GetAllLEDs();
        if (fading)
        {
            // Update fade time based on frame time
            fadeTime += Time.deltaTime;

            // Calculate fade frame
            fadeFrame = fadeTime / fadeDuration;

             // Evaluate color at current frame
            Color frameColor = gradient.Evaluate(fadeFrame);

            // If fade time exceeds duration, stop fading
            if (fadeTime >= fadeDuration)
            {
                fading = false;

                // Ensure frame color is at the end color
                frameColor = gradient.Evaluate(1.0f);
            }

            foreach (GameObject LED in allLEDs)
            {
                // Update LED color
                LED.GetComponent<Renderer>().material.color = frameColor;

                 // Update emission color
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", frameColor);
            }
        }
    }

    // Method to update LED colors either instantly or through fading based on toggle state
    void UpdateLEDColors(GameObject[] leds, Color newColor)
    {
        // Check if fading is disabled
        if (!fadeToggle.isOn) // Update colors instantly
        {
            foreach (GameObject LED in leds)
            {
                // Update LED color
                LED.GetComponent<Renderer>().material.color = newColor;

                // Update emission color
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);
            }
        }
        else // Fade colors to the new color
        {
            // Get the first LED
            GameObject firstLED = leds[0];

            // Get starting color
            Color startCol = firstLED.GetComponent<Renderer>().material.color;
            
            // Get target color
            Color endCol = newColor;

            // Create new gradient
            gradient = new Gradient();

            // Set gradient color keys
            colorKey = new GradientColorKey[2];
            colorKey[0].color = startCol;
            colorKey[0].time = 0.0f;
            colorKey[1].color = endCol;
            colorKey[1].time = fadeDuration;

            // Set gradient alpha keys
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 0.5f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.5f;
            alphaKey[1].time = fadeDuration;

            // Apply color and alpha keys to gradient
            gradient.SetKeys(colorKey, alphaKey);

            // Set fade end color
            fadeEnd = endCol;

            // Reset fade frame
            fadeFrame = 0.0f;

            // Reset fade time
            fadeTime = 0.0f;

            // Set fading flag to true
            fading = true;
        }
    }

    // Methods to set LED color to predefined colors 1-4
    public void OnSetBlue1()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.012f, 0.141f, 0.302f, .500f);
        GameObject[] allLEDs = GetAllLEDs();
        UpdateLEDColors(allLEDs, newColor);
    }

    public void OnSetBlue2()
    {
        // string htmlValue = "#03244d";
        Color newColor =  new Color(0.2862745f, 0.4313726f, 0.6117647f, .5f);
        GameObject[] allLEDs = GetAllLEDs();
        UpdateLEDColors(allLEDs, newColor);
    }

    public void OnSetOrange1()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.8666667f, 0.3333333f, 0.04705882f, .5f);
        GameObject[] allLEDs = GetAllLEDs();
        UpdateLEDColors(allLEDs, newColor);
    }

    public void OnSetOrange2()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.9647059f, 0.5019608f, 0.1490196f, .5f);
        GameObject[] allLEDs = GetAllLEDs();
        UpdateLEDColors(allLEDs, newColor);
    }

    // Method to handle user input of hexadecimal color code
    public void OnEditHexCodeString(string hexCodeString)
    {
        // Reset hexCodeColor
        hexCodeColor = null;

        // Exit if the input is invalid
        if (hexCodeString == null || hexCodeString == "" || hexCodeString.Length != 8)
        {
            return;
        }

        Color newColor;

        // Pre-emptively add '#' to the input to form a valid HTML color code
        string htmlValue = "#" + hexCodeString;

        // Try parsing the color from the HTML color code
        {
        if (ColorUtility.TryParseHtmlString(htmlValue, out newColor))
        {
            // Set hexCodeColor if parsing is successful
            hexCodeColor = newColor;
        }
        else
        {
            // Log an error if parsing fails
            Debug.Log("Error: " + hexCodeString + " is not a valid hexadecimal value.");
        }
    }


    // Method to set LED color to the hexadecimal color specified by the user
    public void OnSetHexCodeColor()
    {
        // Exit if hexCodeColor is null
        if (!hexCodeColor.HasValue) { return; }

        // Get all LEDs
        GameObject[] allLEDs = GetAllLEDs();

        // Update LED colors
        UpdateLEDColors(allLEDs, hexCodeColor.Value);
    }

    public void OnSetRandom()
    {
        // Function specifically made to handle "Random" button call.
        SetRandom();
    }

    // Method to set random colors to LEDs
    void SetRandom()
    {
        // Disable fading
        fading = false;

        // Declare a variable to hold the new random color
        Color newColor;

        // Get all LED game objects in the scene
        GameObject[] allLEDs = GetAllLEDs();

        // Iterate through each LED game object
        foreach (GameObject LED in allLEDs)
        {
            // Generate a random number between 1 and 5
            int randomCol = Random.Range(1, 6);

            // Check the random number to determine the color to assign
            if (randomCol < 3)
            {
                // Assign a blue color with specified RGB values and alpha
                newColor = new Color(0.2862745f, 0.4313726f, 0.6117647f, .5f);
            }
            else if (randomCol > 4)
            {
                // Assign a white color with maximum RGB values and alpha
                newColor = new Color(1.0f, 1.0f, 1.0f, .5f);
            }
            else
            {
                // Assign an orange color with specified RGB values and alpha
                newColor = new Color(0.8666667f, 0.3333333f, 0.04705882f, .5f);
            }

             // Set the color of the LED's material
            LED.GetComponent<Renderer>().material.color = newColor;

            // Set the emission color of the LED's material
            LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);
        }
    }
}

