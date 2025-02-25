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
    /*************/
    /* VARIABLES */
    /*************/

    //Make sure to attach these Buttons in the Inspector
    public Toggle fadeToggle;
    public Toggle randomToggle;
    public InputField hexCodeInput;
    public InputField speedInput;

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    Color? hexCodeColor;

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

    [SerializeField]
    GameObject[] allLEDs;

    [SerializeField]
    Color previousColor; //TODO: connect this to global controller so that the previous color is always correctly stored

    // Note: testing for section applier
    public bool sectionToggle;
    public GameObject[] sectionLEDs;
    public SelectorController selectorController;



    /**********************/
    /* START() & UPDATE() */
    /**********************/

    void Start()
    {
        fadeFrame = 0.0f;
        fadeDuration = 1.0f;

        // "Hexcode" field in UI
        hexCodeInput.onEndEdit.AddListener(OnEditHexCodeString);

        // "Fade time" field in UI
        speedInput.onEndEdit.AddListener(OnSetSpeed);

        // Toggle for the "continual random" UI option
        randomizing = false;
        randomToggle.onValueChanged.AddListener(delegate {
            OnContinualRandomToggle(randomToggle.isOn);
        }); 

        // Find the GameObject with the SelectorController component
        GameObject selectorControllerObject = GameObject.Find("SelectorController");

        // Check if the GameObject was found
        if (selectorControllerObject != null)
        {
            // Get the SelectorController component from the GameObject
            selectorController = selectorControllerObject.GetComponent<SelectorController>();
        }

        // Check whether current section is checked or not (also happens in update)
        checkToggle();
    }

    void Update()
    {
        GradientColorFade();

        // Note: testing for section applier
        checkToggle();
        GetSectionLights();
    }



    /*****************/
    /* SECTION-BASED */
    /*****************/

    GameObject[] GetAllLEDs()
    {
        return GameObject.FindGameObjectsWithTag("LED");
    }

    // Note: testing for section applier
    public void GetSectionLights()
    {
        sectionLEDs = selectorController.sectionLEDs;
    }

    void checkToggle()
    {
        if (selectorController.CurrentToggle.isOn)
        {
            sectionToggle = true;
        }
        else
        {
            sectionToggle = false;
        }
    }



    /*************************/
    /* MISC. COLOR FUNCTIONS */
    /*************************/

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

    void GradientColorFade()
    {
        if (fading) {
            GameObject[] LEDsToUpdate;
            if (sectionToggle) {
                LEDsToUpdate = sectionLEDs;

            } else {
                if (allLEDs == null) {
                    allLEDs = GetAllLEDs();
                }
                LEDsToUpdate = allLEDs;
            }

            fadeTime += Time.deltaTime;
            fadeFrame = fadeTime / fadeDuration;
            Color frameColor = gradient.Evaluate(fadeFrame);
            if (fadeTime >= fadeDuration)
            {
                fading = false;
                frameColor = gradient.Evaluate(1.0f);
            } else {
                frameColor = Color.Lerp(previousColor, gradient.Evaluate(1.0f), fadeFrame);
            }

            previousColor = frameColor;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", frameColor);
            block.SetColor("_EmissionColor", frameColor);
            block.SetFloat("_Alpha", 1);

            foreach (GameObject LED in LEDsToUpdate)
            {
                Renderer renderer = LED.GetComponent<Renderer>();
                renderer.SetPropertyBlock(block);
            }

            //foreach (GameObject LED in LEDsToUpdate)
            //{
            //    LED.GetComponent<Renderer>().material.color = frameColor;
            //    LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", frameColor);
            //}
        } else {
            allLEDs = null;
        }
    }

    void UpdateLEDColors(GameObject[] leds, Color newColor)
    {
        // Debugging
        Debug.Log("Starting update function");
        GetSectionLights();
        Debug.Log(leds[0].GetComponent<Renderer>().material.color);
        Debug.Log(leds[0].GetComponent<Renderer>().material.GetColor("_EmissionColor"));

        if (!fadeToggle.isOn)
        {
            foreach (GameObject LED in leds)
            {
                LED.GetComponent<Renderer>().material.color = newColor;
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", newColor);
            }
        }
        else
        {
            GameObject firstLED = leds[0];
            Color startCol = firstLED.GetComponent<Renderer>().material.color;
            Color endCol = newColor;
            gradient = new Gradient();

            colorKey = new GradientColorKey[2];
            colorKey[0].color = startCol;
            colorKey[0].time = 0.0f;
            colorKey[1].color = endCol;
            colorKey[1].time = fadeDuration;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 0.5f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.5f;
            alphaKey[1].time = fadeDuration;

            gradient.SetKeys(colorKey, alphaKey);
            fadeEnd = endCol;
            fadeFrame = 0.0f;
            fadeTime = 0.0f;
            fading = true;
        }

        // Debugging
        Debug.Log("Ending update function");
        Debug.Log(leds[0].GetComponent<Renderer>().material.color);
        Debug.Log(leds[0].GetComponent<Renderer>().material.GetColor("_EmissionColor"));

    }
    public void OnSetBlue1()
    {
        // string htmlValue = "#03244d";
        Color newColor = new Color(0.012f, 0.141f, 0.302f, .500f);

        // All or current section only
        if (sectionToggle)
        {
            UpdateLEDColors(sectionLEDs, newColor);
        } else
        {
            GameObject[] allLEDs = GetAllLEDs();
            UpdateLEDColors(allLEDs, newColor);
        }
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

    public void OnEditHexCodeString(string hexCodeString) // Runs when hexcode field is edited
    {
        hexCodeColor = null;
        if (hexCodeString.StartsWith("#")){
            hexCodeString = hexCodeString.Substring(1); // Sanitize string by removing the initial pound sign, if it exists
        }

        if (hexCodeString == null || hexCodeString == "" || (hexCodeString.Length != 8 && hexCodeString.Length != 6 && hexCodeString.Length != 3)) {
            return;
        }

        Color newColor;
        string htmlValue = "#" + hexCodeString;

        if (ColorUtility.TryParseHtmlString(htmlValue, out newColor)) {
            hexCodeColor = newColor;
            Debug.Log(hexCodeColor);
        } else {
            Debug.Log("Error: " + hexCodeString + " is not a valid hexadecimal value.");
        }
    }
    public void OnSetHexCodeColor() // Runs when hexcode field is set
    {
        Debug.Log(hexCodeColor);
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
}
