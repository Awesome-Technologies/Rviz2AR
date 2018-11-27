using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeToParentMapper : MonoBehaviour {

    private GameObject toolbox;
    private ObjectCollectionNodes toolboxNodeCollection;
    private NodeManager nodeManager;

    // Use this for initialization
    void Start () {

        toolbox = GameObject.FindGameObjectWithTag("testCubeToolbox");
        nodeManager = GameObject.Find("Rviz2AR_ToolBox").GetComponent<NodeManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void detachFromParent()
    {
        Vector3 posInGrid = new Vector3();

        //Der Buton ist in der Toolbox
        if (this.transform.parent != null)
        {
            posInGrid = this.transform.localPosition;
            this.transform.parent = null;
        }

        //Wir schreiben in die Nodeliste Rein, dass die Node wieder in der Toolbox ist
        nodeManager.signOutToolbox(this.name);



        //-- wir rufen die Update Funktion hier auf, dasmit die Restlichen Nodes in der Toolbox wieder sortiert werden
        //toolbox.GetComponent<ObjectCollectionNodes>().UpdateCollection();

        toolbox.GetComponent<ObjectCollectionNodes>().insertEmpteOnRemoveposition(posInGrid);
    }

    public void attachNodeToToolbox()
    {
        Debug.Log(" EINFÜGEEEENNNN");
        this.transform.parent = toolbox.transform;

        
        this.GetComponent<Transform>().localPosition = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.position;
        this.GetComponent<Transform>().rotation = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.rotation;
        this.GetComponent<Transform>().localScale = toolbox.GetComponent<ObjectCollectionNodes>().NodeList[0].transform.localScale;

        //Wir schreiben in die Nodeliste Rein, dass die Node wieder in der Toolbox ist
        nodeManager.signInToolbox(this.name);

        //-- wir rufen die Update Funktion hier auf, dasmit die hinzugefügte und die anderen Nodes in der Toolbox wieder sortiert werden
        toolbox.GetComponent<ObjectCollectionNodes>().UpdateCollection();
    }
}
