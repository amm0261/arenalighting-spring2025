using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Import the namespace for List<T>


public class SelectorController : MonoBehaviour
{
    public GameObject TopDeck;
    public GameObject UpperDeck;
    public GameObject LowerDeck;
    public Toggle TopToggle;
    public Toggle UpperToggle;
    public Toggle LowerToggle;

    public string useSectionName;
    public GameObject CurrentSection;
    public Toggle CurrentToggle;
    public bool toggleState = false;

    // Note: testing section applier
    public GameObject[] sectionLEDs;

    // Start is called before the first frame update
    void Start()
    {
        TopToggle.onValueChanged.AddListener(delegate { ToggleTopDeck(); });
        UpperToggle.onValueChanged.AddListener(delegate { ToggleUpperDeck(); });
        LowerToggle.onValueChanged.AddListener(delegate { ToggleLowerDeck(); });

        CurrentToggle.onValueChanged.AddListener(delegate { ToggleCurrentSection(); });

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleTopDeck()
    {
        TopDeck.SetActive(!TopDeck.activeSelf);
        // Debug.Log("Top");
    }

    void ToggleUpperDeck()
    {
        UpperDeck.SetActive(!UpperDeck.activeSelf);
        // Debug.Log("Upper");
    }

    void ToggleLowerDeck()
    {
        LowerDeck.SetActive(!LowerDeck.activeSelf);
        // Debug.Log("Lower");
    }

    // Easy way to activate or deactivate sections for section applier
    public void ToggleCurrentSection()
    {
        if (!toggleState)
        {
            DeactivateAllElse(useSectionName);
        }
        else
        {
            ReactivateSections(useSectionName);
        }
        toggleState = !toggleState;
    }

    // The section name will be passed externally
    public void ChangeCurrentSection(string sectionName)
    {
        useSectionName = sectionName;
        if (GameObject.Find(sectionName) != null)
        {
            CurrentSection = GameObject.Find(sectionName);
            GetSectionLEDs(sectionName);
        }
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


    public void DeactivateAllElse(string currentSection)
    {
        if (currentSection.ToLower().Contains("low"))
        {
            ToggleTopDeck();
            ToggleUpperDeck();
            DeactivateSection("Lower Deck", currentSection);
        } else if (currentSection.ToLower().Contains("upper"))
        {
            ToggleTopDeck();
            ToggleLowerDeck();
            DeactivateSection("Upper Deck", currentSection);
        } else
        {
            ToggleUpperDeck();
            ToggleLowerDeck();
            DeactivateSection("Top Deck", currentSection);
        }
    }

    private void DeactivateSection(string sectionName, string currentSection)
    {
        if (GameObject.Find(sectionName) != null) 
        {
            Transform sectionTransform = GameObject.Find(sectionName).transform;
            foreach (Transform childTransform in sectionTransform)
            {
                if (childTransform.name != currentSection)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void ReactivateSections(string currentSection)
    {
        if (currentSection.ToLower().Contains("low"))
        {
            ToggleTopDeck();
            ToggleUpperDeck();
            ReactivateSection("Lower Deck");
        }
        else if (currentSection.ToLower().Contains("upper"))
        {
            ToggleTopDeck();
            ToggleLowerDeck();
            ReactivateSection("Upper Deck");
        }
        else
        {
            ToggleUpperDeck();
            ToggleLowerDeck();
            ReactivateSection("Top Deck");
        }
    }

    private void ReactivateSection(string sectionName)
    {
        GameObject sectionObject = GameObject.Find(sectionName);
        if (sectionObject != null)
        {
            Transform sectionTransform = sectionObject.transform;
            foreach (Transform childTransform in sectionTransform)
            {
                childTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Section object not found: " + sectionName);
        }
    }


}
