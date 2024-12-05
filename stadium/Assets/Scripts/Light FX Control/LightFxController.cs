using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Import the namespace for List<T>
using System.Linq;

public class LightFxController : MonoBehaviour
{
    public GameObject StadiumLeds;


    public GameObject[] sectionsGroup1;
    public GameObject[] sectionsGroup2;
    public GameObject[] sectionsGroup3;

    public bool toggleState = false;

    // A replacement to useSectionName, eventually
    public List<string> useSectionNames;

    // Note: testing section applier
    public GameObject[] sectionLEDs;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to get LEDs within a specific sectionGroup
    GameObject[] GetLEDsInSectionGroup(GameObject[] sectionGroup)
    {
        List<GameObject> combinedList = new List<GameObject>();
         foreach (GameObject section in sectionGroup)
        { 
            combinedList.AddRange(GetLEDsInSection(section));
        }
         return combinedList.ToArray();
            
    }

    // Function to get LEDs within a specific section
    GameObject[] GetLEDsInSection(GameObject section)
    {
        // Find all LEDs that are children of the given section GameObject
        return section.GetComponentsInChildren<Transform>()
                      .Where(t => t.CompareTag("LED"))
                      .Select(t => t.gameObject)
                      .ToArray();
    }


    public void ToggleStripes()
    {
        Color blueColor = new Color(0.012f, 0.141f, 0.302f, .500f);
        Color orangeColor = new Color(0.8666667f, 0.3333333f, 0.04705882f, .5f);
        Color whiteColor = new Color(1f, 1f, 1f, 1f);
                
        GameObject[] group1LEDS = GetLEDsInSectionGroup(sectionsGroup1);
        GameObject[] group2LEDS = GetLEDsInSectionGroup(sectionsGroup2);
        GameObject[] group3LEDS = GetLEDsInSectionGroup(sectionsGroup3);

        foreach (GameObject LED in group1LEDS)
            {
                LED.GetComponent<Renderer>().material.color = whiteColor;
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", whiteColor);
            }

        foreach (GameObject LED in group2LEDS)
            {
                LED.GetComponent<Renderer>().material.color = blueColor;
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", blueColor);
            }

        foreach (GameObject LED in group3LEDS)
            {
                LED.GetComponent<Renderer>().material.color = orangeColor;
                LED.GetComponent<Renderer>().material.SetColor("_EmissionColor", orangeColor);
            }

        //TopDeck.SetActive(!TopDeck.activeSelf);
        // Debug.Log("Top");
    }

    void ToggleEndzones()
    {
        //UpperDeck.SetActive(!UpperDeck.activeSelf);
        // Debug.Log("Upper");
    }

    void ToggleFifty()
    {
        //LowerDeck.SetActive(!LowerDeck.activeSelf);
        // Debug.Log("Lower");
    }

    // Take input from SectionSelectionInput, then check if each section actually exists
    public void CheckSectionValidity()
    {
        Debug.Log("ADD FUNCTIONALITY");
    }

    // Turn off all lights before toggling effect
    public void allLEDsOff()
    {
        //StadiumLEDs.setActive(false);
    }

    // Update SectionLEDs to hold all the lights - this will be passed to color controller
    public void GetSectionLEDs(string sectionName)
    {
        GameObject section = GameObject.Find(sectionName);
        if (section != null)
        {
            // Get all children of parent
            Transform sectionTransform = section.transform;

            // Create a list to store GameObjects with the "LED" tag
            List<GameObject> LEDList = new List<GameObject>();

            // Loop through each child of the parent GameObject
            foreach (Transform child in sectionTransform)
            {
                // Check if the child has the "LED" tag
                if (child.CompareTag("LED"))
                {
                    // Add the child to the LEDList
                    LEDList.Add(child.gameObject);
                }
            }

            // Convert the list to an array
            GameObject[] LEDs = LEDList.ToArray();

            // Assign the array to sectionLEDs
            sectionLEDs = LEDs;

        }
    }

}