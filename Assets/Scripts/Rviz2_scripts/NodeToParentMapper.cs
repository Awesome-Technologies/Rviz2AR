using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeToParentMapper : MonoBehaviour {

    private GameObject toolbox;
    private ObjectCollectionNodes toolboxNodeCollection;

    // Use this for initialization
    void Start () {

        toolbox = GameObject.FindGameObjectWithTag("testCubeToolbox");
        //toolboxNodeCollection = toolbox.GetComponent<ObjectCollectionNodes>;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void detachFromParent()
    {


        //Der Buton ist in der Toolbox
        if (this.transform.parent != null)
        {
            this.transform.parent = null;
        }

        //-- wir rufen die Update Funktion hier auf, dasmit die Restlichen Nodes in der Toolbox wieder sortiert werden
        toolbox.GetComponent<ObjectCollectionNodes>().UpdateCollection();
    }

    public void attachNodeToToolbox()
    {
        
        this.transform.parent = toolbox.transform;

        this.GetComponent<Transform>().localPosition = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.position;
        this.GetComponent<Transform>().rotation = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.rotation;
        this.GetComponent<Transform>().localScale = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.localScale;

        //-- wir rufen die Update Funktion hier auf, dasmit die hinzugefügte und die anderen Nodes in der Toolbox wieder sortiert werden
        toolbox.GetComponent<ObjectCollectionNodes>().UpdateCollection();
    }
}
