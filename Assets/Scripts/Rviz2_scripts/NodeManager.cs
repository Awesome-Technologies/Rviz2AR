using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    /// <summary>
    /// Liste mit allen Nodes, die in der Toolbox angezeigt werden können
    /// </summary>
    private List<CollectionNode> NodeList = null;

    /// <summary>
    /// Liste mit allen Nodes, die außerhalb der Toolbox abgelegt wurden
    /// </summary>
    private List<CollectionNode> NodesOutside = null;

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
    /// Start und finish geben an, welche Elemente der NodeListe durchlaufen werden, um Nodes darzustellen.
    /// </summary>
    int start;
    int finish;

    /// <summary>
    /// Variable des Scriptes, das für die Darstellung und Sortierung der Nodes zuständig ist.
    /// </summary>
    private ObjectCollectionNodes toolboxNodes;
    private GameObject toolbox;



    // Use this for initialization
    void Start () {

        toolboxNodes = GameObject.FindGameObjectWithTag("testCubeToolbox").GetComponent<ObjectCollectionNodes>();
        toolbox = GameObject.FindGameObjectWithTag("testCubeToolbox");

        NodesOutside = new List<CollectionNode>();

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

        addEmptyNode();

        

        updateSeitenanzahl();
        displayNodes();

        Debug.Log("--Länge der Nodeliste: " + NodeList.Count);
        //toolboxNodes.UpdateCollection();
    }

    /// <summary>
    /// An das Ende der List wird eine leere Node angehängt
    /// Diese Node wird dazu benutzt um Nodes, die wieder in die Toolbox angehängt werden richtig darzustellen.
    /// </summary>
    public void addEmptyNode()
    {
        //hier müssen wir eigentlich noch irgendwie den Typ von den Nodes abfragen.

        CollectionNode node = new CollectionNode();

        node.Name = "emptyNode";
        node.transform = null;
        node.inToolbox = true;
        node.displayedInToolbox = false;
        NodeList.Add(node);
    }

    /// <summary>
    /// Je nach Anzahl von Nodes in unserem Array können wir nicht alle Nodes in der Toolbox darstellen
    /// Wir haben also mehrere Seiten, je 15 Nodes, die wir anzeigen können
    /// hier wird errechnet, wieviele Seiten wir anhand der generierten Nodes haben werden.
    /// </summary>
    public void updateSeitenanzahl()
    {
        Debug.Log("NodeList.Count: " + NodeList.Count);
        Debug.Log("ElementsToDisplayInToolbox: " + ElementsToDisplayInToolbox);
        //Seitenzahl = (NodeList.Count % ElementsToDisplayInToolbox) + 1;
        Seitenzahl = (NodeList.Count + ElementsToDisplayInToolbox-1)/ ElementsToDisplayInToolbox ;
        Debug.Log("Seitenzahl: " + Seitenzahl);
    }

    public void displayNodes()
    {
        deleteOldNodes();

        //Debug.Log("currentSite: "+  currentSite);
        //Debug.Log("start: "+  start);

        start = (currentSite - 1) * ElementsToDisplayInToolbox;
        if(currentSite * ElementsToDisplayInToolbox > NodeList.Count)
        {
            finish = NodeList.Count;
        }
        else
        {
            finish = currentSite * ElementsToDisplayInToolbox;
        }

        //Debug.Log("finish: " + finish);

        for (int i= start; i< finish; i++)
        {
            //Debug.Log("i und Nodename:" + i + " / " + NodeList[i].Name);

            if (NodeList[i].Name == "emptyNode")
            {
                GameObject empty = new GameObject();
                empty.name = "emptyNode";

                NodeList[i].NodeGameObject = empty;
                NodeList[i].transform = empty.transform;
                NodeList[i].displayedInToolbox = true;
                NodeList[i].transform.parent = toolboxNodes.transform;
            }
            else
            {
                GameObject prefab = GameObject.Find("ButtonHolographic");
                GameObject clone = Instantiate(prefab, this.transform);
                clone.name = NodeList[i].Name;
                clone.GetComponentInChildren<TextMesh>().text = NodeList[i].Name;
                //Debug.Log("Transform der Node: " + clone.transform.ToString());

                NodeList[i].NodeGameObject = clone;
                NodeList[i].transform = clone.transform;
                NodeList[i].displayedInToolbox = true;



                //NodesToDisplay.Add(NodeList[i]);
                NodeList[i].transform.parent = toolboxNodes.transform;
                //Debug.Log("--Neue Nodeliste: " + toolboxNodes.NodeList.Count);
            }
        }
        //Debug.Log("--Toolbox Kinder: " + toolboxNodes.transform.childCount);
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

        displayNodes();
        //deleteOldNodes();
   



    }

    public void deleteOldNodes()
    {
        //Debug.Log("--Wir löschen die alten Nodes");

        //toolboxNodes.deleteAllNodes();


        
        for (int i = 0; i< NodeList.Count; i++)
        {
            if (NodeList[i].displayedInToolbox)
            {
                //Debug.Log("--Die Node mit dem Namen: " + NodeList[i].Name + " Wird in der Toolbox angezeigt ");
                NodeList[i].displayedInToolbox = false;
                NodeList[i].transform = null;
                //Debug.Log("--Neuer Transform: "+ NodeList[i].transform);
                DestroyImmediate(NodeList[i].NodeGameObject);
       
                

            }
        }

        toolboxNodes.deleteAllNodes();


        toolboxNodes.NodeList.Clear();


        //Debug.Log("--Toolbox Kinder nach Delete: " + toolboxNodes.transform.childCount);
        //Debug.Log("--Neue Nodeliste: " + toolboxNodes.NodeList.Count);
        

    }

    /// <summary>
    /// Wenn eine Node aus der Toolbox genommen und in die Umgebung eingefügt wird,
    /// dann wird sie aus der Nodelist entfernt und in die Liste der Nodes außerhalb der Toolbox gelegt
    /// </summary>
    public void signOutToolbox(string name)
    {
        for (int i = start; i < finish; i++)
        {
            if(NodeList[i].Name == name)
            {
                NodesOutside.Add(NodeList[i]);
                NodeList.Remove(NodeList[i]);
                break;
            }
        }
    }

    /// <summary>
    /// Wenn eine Node aus der Toolbox genommen und in die Umgebung eingefügt wird,
    /// dann wird sie aus der Nodelist entfernt und in die Liste der Nodes außerhalb der Toolbox gelegt
    /// </summary>
    public void signInToolbox(string name)
    {
        for (int i = 0; i < NodesOutside.Count; i++)
        {
            if (NodesOutside[i].Name == name)
            {
                NodeList.Add(NodesOutside[i]);
                NodesOutside.Remove(NodesOutside[i]);
                break;
            }
        }
    }

    /// <summary>
    /// Wenn eine Node aus der Umgebung in die Toolbox gelegt wird, dann
    /// dann wird sie aus der Nodelist entfernt und in die Liste der Nodes außerhalb der Toolbox gelegt
    /// </summary>
    public void openLastSite()
    {
        Debug.Log("openLastSite");
        currentSite = Seitenzahl;
        displayNodes();

    }

    public void updateCurrentSite()
    {
        Debug.Log("updateCurrentSite");
        displayNodes();
    }


}
