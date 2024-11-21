using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FixedCameraControl : MonoBehaviour
{
    public TMP_Text fixedPositionNameText;
    public Camera MainCamera;

    public Vector3 startPosition;
    public Vector3 startRotation;
    public float speed;

    private Dictionary<string, Tuple<Vector3, Vector3>> fixedViews;
    private int currentPosition;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public SelectorController selectorController;


    private void Start()
    {

        // Find the GameObject with the SelectorController component
        GameObject selectorControllerObject = GameObject.Find("SelectorController");

        // Check if the GameObject was found
        if (selectorControllerObject != null)
        {
            // Get the SelectorController component from the GameObject
            selectorController = selectorControllerObject.GetComponent<SelectorController>();
        }


        fixedViews = FixedCameraControlData.LoadFixedCameraControlData();
        SetupDefaultFixedView();
    }

    private void Update()
    {
        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetPosition, speed * Time.deltaTime);
        MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void SetupDefaultFixedView()
    {
        List<string> keyList = new List<string>(this.fixedViews.Keys);
        UpdateUI(keyList[currentPosition]);
        MoveTo(fixedViews["Default"].Item1, fixedViews["Default"].Item2);
    }

    public void ChangePositionRight()
    {
        List<string> keyList = new List<string>(this.fixedViews.Keys);
        if (currentPosition + 1 == keyList.Count)
        {
            currentPosition = 0;
        }
        else
        {
            currentPosition += 1;
        }

        string newPositionName = keyList[currentPosition];

        Tuple<Vector3, Vector3> transformData = fixedViews[newPositionName];

        UpdateUI(newPositionName);
        MoveTo(transformData.Item1, transformData.Item2);
        UpdateSection(newPositionName);

    }

    public void ChangePositionLeft()
    {
        List<string> keyList = new List<string>(this.fixedViews.Keys);
        if (currentPosition == 0)
        {
            currentPosition = keyList.Count - 1;
        }
        else
        {
            currentPosition -= 1;
        }

        string newPositionName = keyList[currentPosition];

        Tuple<Vector3, Vector3> transformData = fixedViews[newPositionName];

        UpdateUI(newPositionName);
        MoveTo(transformData.Item1, transformData.Item2);
        UpdateSection(newPositionName);

    }

    private void UpdateUI(string positionName)
    {
        fixedPositionNameText.text = positionName;

    }

    private void UpdateSection(string positionName)
    {
        selectorController.ChangeCurrentSection(positionName);
    }

    public void MoveTo(Vector3 pos, Vector3 rotationVector)
    {
        targetPosition = pos;
        targetRotation = Quaternion.Euler(rotationVector);
    }
}
