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

    // The section name will be passed externally
    public void ChangeCurrentSection(string sectionName)
    {
        useSectionName = sectionName;
        if (GameObject.Find(sectionName) != null)
        {
            CurrentSection = GameObject.Find(sectionName);
        }

    }

    // Easy way to activate or deactivate sections for section applier
    int toggleNumber = 0;
    public void ToggleCurrentSection()
    {
        if (toggleNumber == 0)
        {
            DeactivateAllElse(useSectionName);
            toggleNumber = 1;
        }
        else
        {
            ReactivateSections();
            toggleNumber = 0;
        }
    }

    public void DeactivateAllElse(string currentSection)
    {
        if (currentSection != null)
        {
            DeactivateSection("Lower Deck", currentSection);
            DeactivateSection("Upper Deck", currentSection);
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

    public void ReactivateSections()
    {
        ReactivateSection("Lower Deck");
        ReactivateSection("Upper Deck");
        ReactivateSection("Top Deck");
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
