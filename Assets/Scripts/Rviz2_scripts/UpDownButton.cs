using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.Buttons;

/// <summary>
/// Functionality on Button Press for scrolling through the diffenrent Nodes in the Toolbox
/// </summary>
public class UpDownButton : MonoBehaviour {

    [SerializeField]
    private bool isUpButton;

    [SerializeField]
    private bool isDownButton;

    private Button button;

    private NodeManager nodeManager;

    // Use this for initialization
    void Start () {

        nodeManager = GameObject.Find("Rviz2AR_ToolBox").GetComponent<NodeManager>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.OnButtonClicked += OnButtonClicked;
        }
    }

    private void OnButtonClicked(GameObject obj)
    {
        if (isUpButton)
        {
            Debug.Log("KlickedUpButtttooooonnnnn");
            nodeManager.upDownButtonPressed("up");
        }

        if (isDownButton)
        {
            Debug.Log("KlickedDOWNButtttooooonnnnn");
            nodeManager.upDownButtonPressed("down");
        }
        
    }
}
