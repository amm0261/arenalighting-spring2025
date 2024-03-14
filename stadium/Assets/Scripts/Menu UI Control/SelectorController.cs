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

    public void ChangeCurrentSection(string sectionName)
    {
        useSectionName = sectionName;
        if (GameObject.Find(sectionName) != null)
        {
            CurrentSection = GameObject.Find(sectionName);
        }

    }

    // Section Applier
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

    
    // To deactivate every other section except the one being affected
    public void DeactivateAllElse(string currentSection)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        if (currentSection.Contains("Low"))
        {
            Transform lowerTransform = GameObject.Find("Lower Deck").transform;
            foreach (Transform childTransform in lowerTransform)
            {
                if (childTransform.name != currentSection)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }

            if (GameObject.Find("Upper Deck") != null)
            {
                Transform upperTransform = GameObject.Find("Upper Deck").transform;
                foreach (Transform childTransform in upperTransform)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }
            
            if (GameObject.Find("Top Deck") != null)
            {
                Transform topTransform = GameObject.Find("Top Deck").transform;
                foreach (Transform childTransform in topTransform)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }

        }
        else if (currentSection.Contains("Upper"))
        {
            Transform lowerTransform = GameObject.Find("Lower Deck").transform;
            foreach (Transform childTransform in lowerTransform)
            {
                childTransform.gameObject.SetActive(false);
            }

            Transform upperTransform = GameObject.Find("Upper Deck").transform;
            foreach (Transform childTransform in upperTransform)
            {
                if (childTransform.name != currentSection)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }

            Transform topTransform = GameObject.Find("Top Deck").transform;
            foreach (Transform childTransform in topTransform)
            {
                childTransform.gameObject.SetActive(false);
            }
        }
        else
        {
            Transform lowerTransform = GameObject.Find("Lower Deck").transform;
            foreach (Transform childTransform in lowerTransform)
            {
                childTransform.gameObject.SetActive(false);
            }

            Transform upperTransform = GameObject.Find("Upper Deck").transform;
            foreach (Transform childTransform in upperTransform)
            {
                childTransform.gameObject.SetActive(false);
            }

            Transform topTransform = GameObject.Find("Top Deck").transform;
            foreach (Transform childTransform in topTransform)
            {
                if (childTransform.name != currentSection)
                {
                    childTransform.gameObject.SetActive(false);
                }
            }
        }
        
    }

    // To reactivate any deactivated sections
    public void ReactivateSections()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        if (GameObject.Find("Lower Deck") != null)
        {
            Transform lowerTransform = GameObject.Find("Lower Deck").transform;
            foreach (Transform childTransform in lowerTransform)
            {
                childTransform.gameObject.SetActive(true);
            }
        }

        if (GameObject.Find("Upper Deck") != null)
        {
            Transform upperTransform = GameObject.Find("Upper Deck").transform;
            foreach (Transform childTransform in upperTransform)
            {
                childTransform.gameObject.SetActive(true);
            }
        }

        if (GameObject.Find("Top Deck") != null)
        {
            Transform topTransform = GameObject.Find("Top Deck").transform;
            foreach (Transform childTransform in topTransform)
            {
                childTransform.gameObject.SetActive(true);
            }
        }
    }
    
}
