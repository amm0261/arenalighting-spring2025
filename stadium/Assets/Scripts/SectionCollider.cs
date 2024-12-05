using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCollider : MonoBehaviour
{
    public GameObject selectorController;
    public SelectorController selectorControllerScript;
    public string sectionName;
    
    void Start()
    {
        sectionName = transform.parent.name;
        // get the selector controller so we can update the script's useSectionNames variable
        selectorController = GameObject.Find("SelectorController");
        selectorControllerScript = selectorController.GetComponent<SelectorController>();
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
