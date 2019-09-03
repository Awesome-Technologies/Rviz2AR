using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.SpatialMapping;

public class PlaceUR5 : MonoBehaviour
{
    private Button button;

    public GameObject ur5;

    private UR5TapToPlace placeUr5;

    // Use this for initialization
    void Start()
    {
        //ur5 = GameObject.Find("ur5Model");
        placeUr5 = ur5.GetComponent<UR5TapToPlace>();
    }

    private void OnEnable()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.OnButtonClicked += OnButtonClicked;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnButtonClicked(GameObject obj)
    {
        Debug.Log("Ich bewege ur5");
        if (ur5.active == false) {
            ur5.active = true;
            placeUr5.IsBeingPlaced = true;
        } else
        {
            ur5.active = false;
        }
    }

}