using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    /// <summary>
    /// List of objects with generated data on the object.
    /// </summary>
    [SerializeField]
    public List<CollectionNode> NodeList;

    public List<CollectionNode> NodesToDisplay;

    /// <summary>
    /// Gibt an, wieviele Seiten an Nodes in der Toolox wir haben
    /// </summary>
    public int Seitenzahl;

    /// <summary>
    /// Gibt an, welche Seite der Toolbox wir gerade anschauen
    /// </summary>
    public int currentSite = 1;

    /// <summary>
    /// Vriable des Scriptes, das für die Darstellung und Sortierung der Nodes zuständig ist.
    /// </summary>
    private ObjectCollectionNodes toolboxNodes;



    // Use this for initialization
    void Start () {

        toolboxNodes = GameObject.FindGameObjectWithTag("testCubeToolbox").GetComponent<ObjectCollectionNodes>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Hier werden aus den Nodes, die wir von ROS bekommen Nodes in Unity generiert
    /// Nachdem alle Nodes erstellt wurden, wird ein Teil der Nodes an die Toolbox weitergegeben um sie darzustellen
    /// </summary>
    public void generateNodes( string[] nodesNames)
    {   
        // Wenn wir die Nodes nur Updaten müssen
        if(NodeList != null)
        {

        }
        // wenn wir zum ersten mal ein Nodeupdate bekommen
        else
        {
            NodeList = new List<CollectionNode>();

            //Hier werden die Nodes zum ersten mal erzeugt
            for (int i = 0; i < nodesNames.Length; i++)
            {
                GameObject prefab = GameObject.Find("ButtonHolographic");
                GameObject clone = Instantiate(prefab, this.transform);

                //hier müssen wir eigentlich noch irgendwie den Typ von den Nodes abfragen.

                CollectionNode node = new CollectionNode();

                node.Name = nodesNames[i];
                node.transform = clone.transform;
                node.inToolbox = true;
                node.displayedInToolbox = false;
                NodeList.Add(node);
            }

        }

        

        updateSeitenanzahl();
        displayNodes();
    }

    /// <summary>
    /// Je nach Anzahl von Nodes in unserem Array können wir nicht alle Nodes in der Toolbox darstellen
    /// Wir haben also mehrere Seiten, je 15 Nodes, die wir anzeigen können
    /// hier wird errechnet, wieviele Seiten wir anhand der generierten Nodes haben werden.
    /// </summary>
    public void updateSeitenanzahl()
    {
        Seitenzahl = (NodeList.Count % 15) + 1;
    }

    public void displayNodes()
    {
        NodesToDisplay = new List<CollectionNode>();

        for (int i= (Seitenzahl-1)*15; i< Seitenzahl*15; i++)
        {
            if (i >= NodeList.Count)
            {
                break;
            }
            else
            {
                NodesToDisplay.Add(NodeList[i]);
            }
        }
        toolboxNodes.UpdateCollection();        
    }

}
