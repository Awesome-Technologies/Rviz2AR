using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    /// <summary>
    /// List of objects with generated data on the object.
    /// </summary>
    private List<CollectionNode> NodeList = null;

    /// <summary>
    /// Gibt an, wieviele Seiten an Nodes in der Toolox wir haben
    /// </summary>
    private int Seitenzahl=1;

    /// <summary>
    /// Gibt an, welche Seite der Toolbox wir gerade anschauen
    /// </summary>
    private int currentSite = 1;

    /// <summary>
    /// ANzahl der Nodes, die wir in der Ansicht in der Toolbox anzeigen<
    /// </summary>
    private int ElementsToDisplayInToolbox = 9;

    /// <summary>
    /// Variable des Scriptes, das für die Darstellung und Sortierung der Nodes zuständig ist.
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
        Debug.Log("generateNodes");
        // Wenn wir die Nodes nur Updaten müssen
        //if (NodeList != null)
        if (NodeList != null)
        {
            Debug.Log("wir gehen in diesen Zweig?");
            Debug.Log("Die Nodeliste: "+ NodeList);
        }
        // wenn wir zum ersten mal ein Nodeupdate bekommen
        else
        {
            Debug.Log("generateNodes, wir erstellen die Nodelist");
            NodeList = new List<CollectionNode>();

            //Hier werden die Nodes zum ersten mal erzeugt
            for (int i = 0; i < nodesNames.Length; i++)
            {
                //hier müssen wir eigentlich noch irgendwie den Typ von den Nodes abfragen.

                CollectionNode node = new CollectionNode();

                node.Name = nodesNames[i];
                node.transform = null;
                node.inToolbox = true;
                node.displayedInToolbox = false;
                NodeList.Add(node);

                //wir hängen das Object an die Toolbox dran
                //node.transform.parent = toolboxNodes.transform;
            }
            

        }

        

        updateSeitenanzahl();
        displayNodes();

        Debug.Log("--Länge der Nodeliste: " + NodeList.Count);
        toolboxNodes.UpdateCollection();
    }

    /// <summary>
    /// Je nach Anzahl von Nodes in unserem Array können wir nicht alle Nodes in der Toolbox darstellen
    /// Wir haben also mehrere Seiten, je 15 Nodes, die wir anzeigen können
    /// hier wird errechnet, wieviele Seiten wir anhand der generierten Nodes haben werden.
    /// </summary>
    public void updateSeitenanzahl()
    {
        Seitenzahl = (NodeList.Count % ElementsToDisplayInToolbox) + 1;
    }

    public void displayNodes()
    {
        deleteOldNodes();

        for (int i= (currentSite - 1)* ElementsToDisplayInToolbox; i< currentSite * ElementsToDisplayInToolbox; i++)
        {
            Debug.Log("i und Nodename:" + i + " / " + NodeList[i].Name);
            if (i >= NodeList.Count)
            {
                break;
            }
            else
            {
                GameObject prefab = GameObject.Find("ButtonHolographic");
                GameObject clone = Instantiate(prefab, this.transform);
                clone.GetComponentInChildren<TextMesh>().text = NodeList[i].Name;
                Debug.Log("Transform der Node: " + clone.transform.ToString());

                NodeList[i].transform = clone.transform;
                NodeList[i].displayedInToolbox = true;



                //NodesToDisplay.Add(NodeList[i]);
                NodeList[i].transform.parent = toolboxNodes.transform;
            }
        }
        toolboxNodes.UpdateCollection();        
    }

    /// <summary>
    /// Einer der Buttons (Up/Down Button) zum Anzeigen einer anderen Auflistung der Nodes wurde gedrückt
    /// </summary>
    public void upDownButtonPressed(string button)
    {
        //Wrie haben eh nur eine Seite zum Anschauen, e sgibt also nichts zu Ändern
        if (Seitenzahl != 1)
        {
            if (button == "up" && currentSite > 1)
            {
                currentSite--;
            }
            else if (button == "down" && currentSite < Seitenzahl)
            {
                currentSite++;
            }

        }
        

    }

}
