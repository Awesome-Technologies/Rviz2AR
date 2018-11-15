using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxRaycaster : MonoBehaviour {

    private bool lookAtToolbox = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //if(Wenn wir gerade eine Drag Geste mit der Hand machen)
        // Wir suchen die Toolbox
        LayerMask mask = LayerMask.GetMask("Toolbox");
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                mask))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
            //Debug.Log("WE ARE LOOKING AT THE TOOLBOXXXXXXXXXXXXX");
            lookAtToolbox = true;
        }else
        {
            lookAtToolbox = false;
        }

    }

    public bool lookingAtToolbox()
    {
        return lookAtToolbox;
    }
    
}
