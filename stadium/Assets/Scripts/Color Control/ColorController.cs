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
    //Make sure to attach these Buttons in the Inspector
    public Toggle fadeToggle;
    public Toggle randomToggle;
    public InputField hexCodeInput;
    public InputField speedInput;
    public TMP_Text errorMessage;

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    Color? hexCodeColor;

    private bool colorNeedsUpdate = false;
    private Color lastColor;

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

    private GameObject[] allLEDs;

    void Start()
    {
        fadeFrame = 0.0f;
        fadeDuration = 1.0f;

        hexCodeInput.onEndEdit.AddListener(OnEditHexCodeString);
        speedInput.onEndEdit.AddListener(OnSetSpeed);
        randomToggle.onValueChanged.AddListener(delegate {
            OnContinualRandomToggle(randomToggle.isOn);
        });

        randomizing = false;
        UpdateAllLEDsCache(); // Initialize the cache of all LEDs
    }

    void Update()
    {
        if (fading)
        {
        GradientColorFade();
        }
        // Apply color updates if there is a need
        else if (colorNeedsUpdate)
        {
        ApplyColorUpdates(lastColor); // Use the last set color
        colorNeedsUpdate = false; // Reset the flag after applying
        }
    }
    public void OnSetSpeed(string speed)
    {
        fadeDuration = float.Parse(speed);
    }

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

    // Cache all LEDs
    void UpdateAllLEDsCache()
    {
        allLEDs = GetAllLEDs();
    }
    GameObject[] GetAllLEDs()
    {
        string ledTag;
        ledTag = "LED";
        return GameObject.FindGameObjectsWithTag(ledTag);
    }

    void UpdateLEDColors(GameObject[] leds, Color newColor)
    {
        lastColor = newColor;
        colorNeedsUpdate = true;
        if (!fadeToggle.isOn)
        {
            ApplyColorUpdates(newColor);
        }
        else
        {
            StartFading(leds, newColor);

        }
    }
    void StartFading(GameObject[] leds, Color endColor)
    {
        GameObject firstLED = leds[0];
            Color startCol = firstLED.GetComponent<Renderer>().material.color;
            gradient = new Gradient();
            colorKey = new GradientColorKey[2];
            colorKey[0].color = startCol;
            colorKey[0].time = 0.0f;
            colorKey[1].color = endColor;
            colorKey[1].time = fadeDuration;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 0.5f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.5f;
            alphaKey[1].time = fadeDuration;

            gradient.SetKeys(colorKey, alphaKey);
            fadeEnd = endColor;
            fadeFrame = 0.0f;
            fadeTime = 0.0f;
            fading = true;
    }
    void GradientColorFade()
    {
        var startTime = Time.realtimeSinceStartup;

        if (fading)
        {
            fadeTime += Time.deltaTime;
            fadeFrame = fadeTime / fadeDuration;
            Color frameColor = gradient.Evaluate(fadeFrame);

            ApplyColorUpdates(frameColor);

            if (fadeTime >= fadeDuration)
            {
                fading = false;
                ApplyColorUpdates(gradient.Evaluate(1.0f));
            }
        }
        var elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log($"Gradient calculations took: {elapsedTime * 1000} ms");
    }

    void ApplyColorUpdates(Color colorToApply)
    {
        if (allLEDs == null || allLEDs.Length == 0) return;

        foreach (GameObject LED in allLEDs)
        {
            Renderer ledRenderer = LED.GetComponent<Renderer>();
            ledRenderer.material.color = colorToApply;
            ledRenderer.material.SetColor("_EmissionColor", colorToApply);
        }
    }
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

    public void OnEditHexCodeString(string hexCodeString)
    {
        hexCodeColor = null;

        if (hexCodeString == null || hexCodeString == "")
        {
            HideErrorMessage();
            return;
        }

        if (hexCodeString.Length != 6)
        {
            ShowErrorMessage("Error: Inputs must be exactly 6 characters long.");
            return;
        }

        if (ColorUtility.TryParseHtmlString("#" + hexCodeString, out Color newColor))
        {
            hexCodeColor = newColor;
            HideErrorMessage();
        }
        else
        {
            ShowErrorMessage("Error:" + hexCodeString + " is not a valid hexadecimal value.");
            Debug.Log("Error: " + hexCodeString + " is not a valid hexadecimal value.");
        }
    }

    public void OnSetHexCodeColor()
    {
        if (!hexCodeColor.HasValue) { return; }
        GameObject[] allLEDs = GetAllLEDs();
        UpdateLEDColors(allLEDs, hexCodeColor.Value);
    }

    public void OnSetRandom()
    {
        // Function specifically made to handle "Random" button call.
        SetRandom();
    }

    void SetRandom()
    {
        // Disable fading
        fading = false;

        Color newColor;
        GameObject[] allLEDs = GetAllLEDs();
        foreach (GameObject LED in allLEDs)
        {
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
            LED.GetComponent<Renderer>().material.color = newColor;
            LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);
        }
    }
    //Displays error message when prompted by other method
    void ShowErrorMessage(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }
    void HideErrorMessage()
    {

        errorMessage.gameObject.SetActive(false);
    }
}

