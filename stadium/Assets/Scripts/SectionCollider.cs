using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCollider : MonoBehaviour
{
    public GameObject selectorController;
    public SelectorController selectorControllerScript;
    public string sectionName;
    public GameObject LEDPrefab;
    public List<GameObject> sectionLEDs;
    
    void Start()
    {
        sectionName = transform.parent.name;
        // Get the selector controller so we can update the script's useSectionNames variable
        selectorController = GameObject.Find("SelectorController");
        selectorControllerScript = selectorController.GetComponent<SelectorController>();

        // Find children LEDs for this section
        Transform parent = transform.parent;
        AddDescendantsWithTag(parent, "LED", sectionLEDs);
    }

    // https://discussions.unity.com/t/finding-all-children-of-object/653529
    // Credit to AndyGainey in above forum post
    // Will find all decendents of a parent with a specific tag, and add them to a given list
    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }
    
    // This will trigger when the mouse clicks on the box collider of a section
    public void OnMouseDown()
    {
        // Add or remove the section to the selected sections list
        if (selectorControllerScript.useSectionNames.Contains(sectionName))
        {
            selectorControllerScript.useSectionNames.Remove(sectionName);
        }
        else 
        {
            selectorControllerScript.useSectionNames.Add(sectionName);
        }
        
    }
}
