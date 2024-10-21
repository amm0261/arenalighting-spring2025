using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Import the namespace for List<T>

public class SelectorController : MonoBehaviour
{
    public GameObject allDecks;
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

    // A replacement to useSectionName, eventually
    public List<string> useSectionNames;

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

    // Take input from SectionSelectionInput, then check if each section actually exists
    public void CheckSectionValidity()
    {
     Debug.Log("ADD FUNCTIONALITY");
    }

    // Easy way to activate or deactivate sections for section applier
    public void ToggleCurrentSection()
    {
        if (!toggleState)
        {
            DeactivateAllElse(useSectionNames);
        }
        else
        {
            ReactivateSections(useSectionNames);
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

    public void DeactivateAllElse(List<string> currentSections)
    {
        Debug.Log("Deactivating multiple sections!");
        // loop through every section
        foreach (Transform deck in allDecks.transform) {
            foreach (Transform iterSection in deck) {
                if (!currentSections.Contains(iterSection.name))
                {
                    iterSection.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ReactivateSections(List<string> currentSections) {
        Debug.Log("Reactivating multiple sections!");
        // loop through every section
        foreach (Transform deck in allDecks.transform) {
            foreach (Transform iterSection in deck) {
                if (!currentSections.Contains(iterSection.name))
                {
                    iterSection.gameObject.SetActive(true);
                }
            }
        }
    }

}
