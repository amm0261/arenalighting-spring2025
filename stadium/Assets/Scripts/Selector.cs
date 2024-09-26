using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    List<Transform> getSectionLights(string[] sections)
    {
        List<Transform> lightsList = new List<Transform>();
        foreach (string section in sections)
        {
            GameObject selectedSection = GameObject.Find(section);
            //Debug.Log(selectedSection);
            Transform[] selectedSectionLights = selectedSection.GetComponentsInChildren<Transform>();
            foreach (Transform light in selectedSectionLights)
            {
                if (light.name.Contains("LEDTemplate"))
                {
                    lightsList.Add(light.transform);
                    //Debug.Log(light);
                }
            }
        }
        return lightsList;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Currently only returns a list of LEDs in a specified section
        bool selector = false;
        if (selector == true)
        {
            string[] sections = new string[]{"Section 16"};
            // For example: new string[]{"Section 16", "Section 19"};
            getSectionLights(sections);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
