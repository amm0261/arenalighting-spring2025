using UnityEngine;
using UnityEngine.UI;

public class GlowFadeController : MonoBehaviour
{
    // References to UI buttons and toggle
    public Button fadeButton;
    public Button glowButton;
    public Button flashButton;
    public Toggle pulseToggle;

    // Variables for controlling pulse effect
    public float pulseSpeed;
    public AnimationCurve BrightnessCurve;
    public Color startingColor;
    public bool flashing;
    public InputField pulseSpeedInput;
    Material emissiveMaterial;

    // Function to get all game objects with tag "LED"
    GameObject[] GetAllLEDs()
    {
        string ledTag = "LED";
        return GameObject.FindGameObjectsWithTag(ledTag);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize pulse speed and add listeners to buttons and toggle
        pulseSpeed = 1.0f;
        fadeButton.onClick.AddListener(Fade);
        glowButton.onClick.AddListener(Glow);
        flashButton.onClick.AddListener(Flash);
        pulseSpeedInput.onEndEdit.AddListener(SetPulseSpeed);
        flashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get all LED game objects and apply pulse effect if toggle is on
        GameObject[] allLEDs = GetAllLEDs();

        if (pulseToggle.isOn)
        {
            float scaledTime = Time.time * pulseSpeed;

            foreach (GameObject LED in allLEDs)
            {
                // Get the material of the LED
                emissiveMaterial = LED.GetComponent<Renderer>().material;
                startingColor = LED.GetComponent<Renderer>().material.color;

                // Calculate brightness using animation curve and apply color with increased brightness
                float brightness = BrightnessCurve.Evaluate(scaledTime);
                Color color = startingColor;
                color = new Color(
                    color.r * Mathf.Pow(2, brightness),
                    color.g * Mathf.Pow(2, brightness),
                    color.b * Mathf.Pow(2, brightness),
                    color.a);

                // Set the emission color of the material
                emissiveMaterial.SetColor("_EmissionColor", color);
            }
        }
    }

    // Function to fade LEDs (disable emission)
    void Fade()
    {
        GameObject[] allLEDs = GetAllLEDs();
        foreach (GameObject LED in allLEDs)
        {
            // Get the material of the LED and disable emission
            emissiveMaterial = LED.GetComponent<Renderer>().material;
            emissiveMaterial.DisableKeyword("_EMISSION");
        }
    }

    // Function to make LEDs glow (enable emission)
    void Glow()
    {
        GameObject[] allLEDs = GetAllLEDs();
        foreach (GameObject LED in allLEDs)
        {
            // Get the material of the LED and enable emission
            emissiveMaterial = LED.GetComponent<Renderer>().material;
            emissiveMaterial.EnableKeyword("_EMISSION");
        }
    }

    // Function to toggle flashing effect
    void Flash()
    {
        // Toggle flashing state and start or stop flashing accordingly
        flashing = !flashing;
        if (flashing)
        {
            InvokeRepeating("Fade", 0.0f, 1.0f); // Start fading every second
            InvokeRepeating("Glow", 0.5f, 1.0f); // Start glowing every second (after 0.5s delay)
        }
        else
        {
            CancelInvoke(); // Stop all repeating invokes
        }
    }

    // Function to set pulse speed based on user input
    void SetPulseSpeed(string speed)
    {
        if (speed == null || speed == "") return; // If input is empty, do nothing
        pulseSpeed = float.Parse(speed); // Parse input string to float and set pulse speed
    }
}
