using UnityEngine;
using UnityEngine.UI;


public class SelectorController : MonoBehaviour
{
    public GameObject TopDeck;
    public GameObject UpperDeck;
    public GameObject LowerDeck;
    public Toggle TopToggle;
    public Toggle UpperToggle;
    public Toggle LowerToggle;

    /** START NEW CODE : goal is to be able to select only the section you are actively looking at... **/
    public GameObject CurrentSection;
    public Toggle CurrentToggle;

    // Start is called before the first frame update
    void Start()
    {
        TopToggle.onValueChanged.AddListener(delegate { ToggleTopDeck(); });
        UpperToggle.onValueChanged.AddListener(delegate { ToggleUpperDeck(); });
        LowerToggle.onValueChanged.AddListener(delegate { ToggleLowerDeck(); });

        // NEW CODE
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

    // NEW CODE
    void ToggleCurrentSection()
    {
        CurrentSection.SetActive(!CurrentSection.activeSelf);
        Debug.Log("Current");
    }
}
