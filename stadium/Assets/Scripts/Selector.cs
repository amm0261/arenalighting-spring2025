using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    // Method to get a list of section lights based on given section names
    List<Transform> getSectionLights(string[] sections)
    {
        // Creating a new list to store section lights
        List<Transform> lightsList = new List<Transform>();

        // Iterating over each section name in the array
        foreach (string section in sections)
        {
            // Finding the GameObject with the specified section name
            GameObject selectedSection = GameObject.Find(section);

            // Logging the selected section (commented out, for debugging purposes)
            //Debug.Log(selectedSection);

            // Getting all child transforms of the selected section
            Transform[] selectedSectionLights = selectedSection.GetComponentsInChildren<Transform>();
           
           // Iterating over each child transform
           foreach (Transform light in selectedSectionLights)
            {
                // Checking if the child transform's name contains "LEDTemplate"
                if (light.name.Contains("LEDTemplate"))
                {
                    // Adding the transform to the lightsList
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
            // Creating an array of section names (empty in this case)
            string[] sections = new string[]{""};
            // For example: new string[]{"Section 16", "Section 19"};
            getSectionLights(sections);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
