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
    /**
    // White Sections
    public GameObject section1;
    public GameObject section2;
    public GameObject section4;
    public GameObject section6;
    public GameObject section8;
    public GameObject section10;
    public GameObject section11;
    public GameObject section24;
    public GameObject section26;
    public GameObject section28;
    public GameObject section30;
    public GameObject section32;
    public GameObject section33;
    public GameObject section34;

    // Orange Sections
    //public GameObject section33; // Duplicate, commented out in Blue
    public GameObject section29;
    public GameObject section25;
    public GameObject section22;
    public GameObject section21;
    public GameObject section16;
    //public GameObject section11; // Duplicate, commented out in White
    public GameObject section7;
    public GameObject section3;
    public GameObject section46;
    public GameObject section44;
    public GameObject section39;
    public GameObject section15;
    public GameObject section17;
    //public GameObject section21; // Duplicate, commented out above in Orange
    //public GameObject section22; // Duplicate, commented out above in Orange
    public GameObject section23;
    public GameObject section38;
    public GameObject section40;
    //public GameObject section44; // Duplicate, commented out above in Orange
    public GameObject section45;
    //public GameObject section46; // Duplicate, commented out above in Orange
    public GameObject section62;
    public GameObject section60;
    public GameObject section54;
    public GameObject section53;
    public GameObject section50;
    public GameObject section49;
    public GameObject section114;
    public GameObject section113;
    public GameObject section110;
    public GameObject section109;
    public GameObject section106;
    public GameObject section105;

    // Blue Sections
    public GameObject section19;
    public GameObject section31;
    public GameObject section27;
    //public GameObject section24; // Duplicate, commented out in White
    public GameObject section35;
    public GameObject section37;
    public GameObject section41;
    public GameObject section9;
    public GameObject section5;
    //public GameObject section1; // Duplicate, commented out in White
    public GameObject section12;
    public GameObject section14;
    //public GameObject section12; // Duplicate, commented out above in Blue
    public GameObject section13;
    //public GameObject section14; // Duplicate, commented out above in Blue
    public GameObject section18;
    public GameObject section20;
    //public GameObject section35; // Duplicate, commented out above in Blue
    public GameObject section36;
    //public GameObject section37; // Duplicate, commented out above in Blue
    //public GameObject section41; // Duplicate, commented out above in Blue
    public GameObject section42;
    public GameObject section58;
    public GameObject section56;
    public GameObject section52;
    public GameObject section51;
    public GameObject section48;
    public GameObject section47;
    public GameObject section112;
    public GameObject section111;
    public GameObject section108;
    public GameObject section107;
    public GameObject section104;
    public GameObject section103;
    **/


    //public Toggle StripeToggle;
    //public Toggle EndzonesToggle;
    //public Toggle FiftyYardToggle;

    //public string useSectionName;
    //public Toggle CurrentToggle;
    public bool toggleState = false;

    // A replacement to useSectionName, eventually
    public List<string> useSectionNames;

    // Note: testing section applier
    public GameObject[] sectionLEDs;

    // Start is called before the first frame update
    void Start()
    {
        //StripeToggle.onValueChanged.AddListener(delegate { ToggleStripes(); });
        //EndzonesToggle.onValueChanged.AddListener(delegate { ToggleEndzones(); });
        //FiftyYardToggle.onValueChanged.AddListener(delegate { ToggleFifty(); });

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