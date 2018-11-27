// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoloToolkit.Unity.Collections
{
    /// <summary>
    /// An Object Collection is simply a set of child objects organized with some
    /// layout parameters.  The object collection can be used to quickly create 
    /// control panels or sets of prefab/objects.
    /// </summary>
    public class ObjectCollectionNodes : MonoBehaviour
    {
        #region public members
        /// <summary>
        /// Action called when collection is updated
        /// </summary>
        public Action<ObjectCollectionNodes> OnCollectionUpdated;

        /// <summary>
        /// List of objects with generated data on the object.
        /// </summary>
        [SerializeField]
        public List<CollectionNode> NodeList = new List<CollectionNode>();

        /// <summary>
        /// Type of sorting to use.
        /// </summary>
        [Tooltip("Type of sorting to use")]
        public SortTypeEnum SortType = SortTypeEnum.None;

        /// <summary>
        /// Whether to treat inactive transforms as 'invisible'
        /// </summary>
        public bool IgnoreInactiveTransforms = true;

        /// <summary>
        /// This is the radius of either the Cylinder or Sphere mapping and is ignored when using the plane mapping.
        /// </summary>
        [Range(0.05f, 5.0f)]
        [Tooltip("Radius for the sphere or cylinder")]
        public float Radius = 2f;

        [SerializeField]
        [Tooltip("Radial range for radial layout")]
        [Range(5f, 360f)]
        private float radialRange = 180f;

        /// <summary>
        /// This is the radial range for creating a radial fan layout.
        /// </summary>
        public float RadialRange
        {
            get { return radialRange; }
            set { radialRange = value; }
        }

        /// <summary>
        /// Number of rows per column (column number is automatically determined).
        /// </summary>
        [Tooltip("Number of rows per column")]
        public int Rows = 3;

        /// <summary>
        /// Number of rows per column (column number is automatically determined).
        /// </summary>
        [Tooltip("Number of rows per column")]
        public int Columns = 3;

        /// <summary>
        /// Width of the Toolbox.
        /// </summary>
        [Tooltip("Width of cell per object")]
        public float Width = 0.45f;

        /// <summary>
        /// Height of the Toolbox.
        /// </summary>
        [Tooltip("Height of cell per object")]
        public float Height = 0.75f;

        /// <summary>
        /// Width of the cell per object in the collection.
        /// </summary>
        [Tooltip("Width of cell per object")]
        public float CellWidth = 0.5f;

        /// <summary>
        /// Height of the cell per object in the collection.
        /// </summary>
        [Tooltip("Height of cell per object")]
        public float CellHeight = 0.5f;

        [SerializeField]
        [Tooltip("Margin between objects horizontally")]
        private float horizontalMargin = 0.2f;

        /// <summary>
        /// Margin between objects horizontally.
        /// </summary>
        public float HorizontalMargin
        {
            get { return horizontalMargin; }
            set { horizontalMargin = value; }
        }

        [SerializeField]
        [Tooltip("Margin between objects vertically")]
        private float verticalMargin = 0.2f;

        /// <summary>
        /// Margin between objects vertically.
        /// </summary>
        public float VerticalMargin
        {
            get { return verticalMargin; }
            set { verticalMargin = value; }
        }

        [SerializeField]
        [Tooltip("Margin between objects in depth")]
        private float depthMargin = 0.2f;

        /// <summary>
        /// Margin between objects in depth.
        /// </summary>
        public float DepthMargin
        {
            get { return depthMargin; }
            set { depthMargin = value; }
        }

        /// <summary>
        /// Reference mesh to use for rendering the sphere layout
        /// </summary>
        [HideInInspector]
        public Mesh SphereMesh;

        /// <summary>
        /// Reference mesh to use for rendering the cylinder layout
        /// </summary>
        [HideInInspector]
        public Mesh CylinderMesh;

        //public float Width { get; private set; }

        //public float Height { get; private set; }

        private Vector3[] nodeGrid;
        #endregion

        #region private variables
        private int _columns;
        private float _circumference;
        private float _radialCellAngle;
        private Vector2 _halfCell;
        #endregion

        /// <summary>
        /// Update collection is called from the editor button on the inspector.
        /// This function rebuilds / updates the layout.
        /// </summary>
        public void UpdateCollection()
        {
            // Check for empty nodes and remove them

            List<CollectionNode> emptyNodes = new List<CollectionNode>();
            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList[i].transform == null || (IgnoreInactiveTransforms && !NodeList[i].transform.gameObject.activeSelf) || NodeList[i].transform.parent == null || !(NodeList[i].transform.parent.gameObject == this.gameObject))
                {
                    emptyNodes.Add(NodeList[i]);

                }
            }

            // Now delete the empty nodes
            for (int i = 0; i < emptyNodes.Count; i++)
            {
                //Debug.Log("+++++ We removed : " + emptyNodes[i].Name);

                Debug.Log("I will remove: " + emptyNodes[i].Name);

                NodeList.Remove(emptyNodes[i]);


                //Debug.Log("+++++ Kinder: " + this.transform.childCount);


                //Destroy(emptyNodes[i].transform.gameObject);
            }

            emptyNodes.Clear();


            // Check when children change and adjust
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform child = this.transform.GetChild(i);

                if (!ContainsNode(child) && (child.gameObject.activeSelf || !IgnoreInactiveTransforms))
                {
                    CollectionNode node = new CollectionNode();

                    node.Name = child.name;
                    node.transform = child;
                    NodeList.Add(node);
                }
            }




            switch (SortType)
            {
                case SortTypeEnum.None:
                    break;

                case SortTypeEnum.Transform:
                    NodeList.Sort(delegate (CollectionNode c1, CollectionNode c2) { return c1.transform.GetSiblingIndex().CompareTo(c2.transform.GetSiblingIndex()); });
                    break;

                case SortTypeEnum.Alphabetical:
                    NodeList.Sort(delegate (CollectionNode c1, CollectionNode c2) { return c1.Name.CompareTo(c2.Name); });
                    break;

                case SortTypeEnum.AlphabeticalReversed:
                    NodeList.Sort(delegate (CollectionNode c1, CollectionNode c2) { return c1.Name.CompareTo(c2.Name); });
                    NodeList.Reverse();
                    break;

                case SortTypeEnum.TransformReversed:
                    NodeList.Sort(delegate (CollectionNode c1, CollectionNode c2) { return c1.transform.GetSiblingIndex().CompareTo(c2.transform.GetSiblingIndex()); });
                    NodeList.Reverse();
                    break;

                case SortTypeEnum.EmptyNodelast:
                    NodeList.Sort(delegate (CollectionNode c1, CollectionNode c2)
                    {

                        //return c1.Name.CompareTo("emptyGameObjectNode");
                        if (c2.Name == "emptyNode")
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }

                    });
                    break;
            }



            _columns = Columns;
            _halfCell = new Vector2(CellWidth * 0.5f, CellHeight * 0.5f);
            _circumference = 2f * Mathf.PI * Radius;
            _radialCellAngle = RadialRange / _columns;

            //updateRowsColums();

            /*
            Debug.Log("Die Daten:");
            Debug.Log("--Width: " + Width);
            Debug.Log("--Height: " + Height);
            Debug.Log("--CellWidth: " + CellWidth);
            Debug.Log("--CellHeight: " + CellHeight);
            Debug.Log("--NodeTransform: " + NodeList[0].transform.localScale.ToString());
            Debug.Log("--NodeTransform: " + NodeList[0].transform.position.ToString());
            */

            generateNodeGrid();
            LayoutChildren();

            if (OnCollectionUpdated != null)
            {
                OnCollectionUpdated.Invoke(this);
            }

        }

        /// <summary>
        /// Internal function for laying out all the children when UpdateCollection is called.
        /// </summary>
        private void generateNodeGrid()
        {

            //Debug.Log("+++++++++++ LayoutChildren() ++++++++++++++ ");

            int cellCounter = 0;
            float startOffsetX;
            float startOffsetY;

            nodeGrid = new Vector3[Rows * _columns];
            //Debug.Log("+++ Nodegrid.Length " + nodeGrid.Length);
            //Debug.Log("+++ Nodelist.Length " + NodeList.Count);


            // Now lets lay out the grid
            startOffsetX = (_columns * 0.5f) * CellWidth;
            startOffsetY = (Rows * 0.5f) * CellHeight;

            // First start with a grid then project onto surface

            //Wir gehen alle ROws durch
            for (int r = 0; r < Rows; r++)
            {
                //Wir gehen alle Colums durch
                for (int c = 0; c < _columns; c++)
                {
                    //Debug.Log("--c: " + c);
                    // Wir sagen wir haben 15 freie Plätze in der Tolbox (5Rows x 3 colums)
                    if (cellCounter < NodeList.Count)
                    //if (cellCounter < Rows * _columns)
                    {
                        /*
                        Debug.Log("### Nodegrid Position ###");
                        Debug.Log("--CellWidth: " + CellWidth);
                        Debug.Log("--startOffsetX: " + startOffsetX);
                        Debug.Log("--_halfCell.x: " + _halfCell.x);
                        Debug.Log("--CellHeight: " + CellHeight);
                        Debug.Log("--startOffsetY: " + startOffsetY);
                        Debug.Log("--_halfCell.y: " + _halfCell.y);
                        */

                        double xOffset = 0.5 / this.transform.localScale.x;
                        double yOffset = 0.5 / this.transform.localScale.y;

                        //Debug.Log("+++CellCounter: " + cellCounter);

                        //nodeGrid[cellCounter] = new Vector3((c * CellWidth) - startOffsetX + _halfCell.x, -(r * CellHeight) + startOffsetY - _halfCell.y, -0.5f) + (Vector3)((NodeList[cellCounter])).Offset;
                        nodeGrid[cellCounter] = new Vector3((c * CellWidth) - startOffsetX + _halfCell.x, -(r * CellHeight) + startOffsetY - _halfCell.y, -0.01f) + (Vector3)((NodeList[cellCounter])).Offset;
                        //nodeGrid[cellCounter] = new Vector3((c * CellWidth) - startOffsetX + _halfCell.x, -(r * CellHeight) + startOffsetY - _halfCell.y, 0f);
                        //nodeGrid[cellCounter] = new Vector3(((c * CellWidth) - startOffsetX + _halfCell.x) * (float)xOffset, (-(r * CellHeight) + startOffsetY - _halfCell.y) * (float)yOffset, -1f);
                        //nodeGrid[cellCounter] = new Vector3(((c * CellWidth) - startOffsetX + _halfCell.x) * (float)xOffset, (-(r * CellHeight) + startOffsetY - _halfCell.y) * (float)yOffset, -0f);

                    }
                    cellCounter++;

                }
            }
        }


        private void LayoutChildren()
        {
            Vector3 newPos = Vector3.zero;

            Debug.Log("+++++++++++ LayoutChildren() ++++++++++++++ ");
            Debug.Log("+NodeLost: " + NodeList.Count);
            for (int i = 0; i < NodeList.Count; i++)
            {
                newPos = nodeGrid[i];
                NodeList[i].transform.localPosition = newPos;
                UpdateNodeFacing(NodeList[i], newPos);
            }

            /*
            Debug.Log("--AfterNodeFacing: ");
            Debug.Log("--NodeTransform: " + NodeList[0].transform.localScale.ToString());
            Debug.Log("--NodeTransform.localPos: " + NodeList[0].transform.localPosition.ToString());
            Debug.Log("--NodeTransform.Pos: " + NodeList[0].transform.position.ToString());
            */
        }

        /// <summary>
        /// Update the facing of a node given the nodes new position for facing orign with node and orientation type
        /// </summary>
        /// <param name="node"></param>
        /// <param name="orientType"></param>
        /// <param name="newPos"></param>
        private void UpdateNodeFacing(CollectionNode node, Vector3 newPos = default(Vector3))
        {
            node.transform.forward = transform.rotation * Vector3.forward;


        }

        /// <summary>
        /// Internal function to check if a node exists in the NodeList.
        /// </summary>
        /// <param name="node">A <see cref="Transform"/> of the node to see if it's in the NodeList</param>
        /// <returns></returns>
        private bool ContainsNode(Transform node)
        {
            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList[i] != null)
                {
                    if (NodeList[i].transform == node)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public GameObject prefab;
        private Transform parentTransform;
        private bool readyForUpdate = false;
        private string[] nodes;

        void Start()
        {
            this.parentTransform = this.transform;
        }

        void Update()
        {
            /*
            if (readyForUpdate)
            {
                createNodes(nodes);
            }
            */

        }

        /// <summary>
        /// Creates Nodes out of the given Array and puts them into the toolbox.
        /// </summary>
        /// <param name="nodes">The source <see cref="string[]"/>names of the nodes to be created</param>
        public void createNodes(string[] nodes)
        {
            //1. Create as many nodes as you have names in the nodes string
            //2. Put the Nodes inside the Rviz2_toolbox as children
            //3. Update the toolbox to make the Nodes visible

            //Debug.Log("--Creating Nodes with: " + nodes.Length);


            //1.
            //public static Object Instantiate(Object original, Transform parent);
            for (int i = 0; i < nodes.Length; i++)
            //for (int i = 0; i < 6; i++)
            {
                //2.
                //GameObject clone = Instantiate(prefab, parentTransform);
                //Debug.Log("--new Clone Scale: " + clone.transform.localScale.ToString());

                /*
                GameObject clone = Instantiate(prefab, null);
                Debug.Log("--CloneScale bevor Child: " + clone.transform.localScale.ToString());
                clone.transform.parent = this.transform;
                Debug.Log("--CloneScale after child: " + clone.transform.localScale.ToString());
                */

                GameObject clone = Instantiate(prefab, this.transform);
                clone.name = "HoloButton" + i;


            }


            //3.
            this.UpdateCollection();
            //this.readyForUpdate = false;

            //Vector3 prefabSize = Vector3.Scale(prefab.transform.localScale, prefab.GetComponent<Mesh>().bounds.size);
            //Vector3 thisSize = Vector3.Scale(this.transform.localScale, this.GetComponent<Mesh>().bounds.size);

            //fiDebug.Log("--prefab Size: " + prefabSize.ToString());

            //Debug.Log("--this.si´ze: " + thisSize.ToString());

            //this.GetComponent<Transform>().sizeDelta = new Vector2(Width, Height);
            //Debug.Log("--We have width and hight: " + Width +" / "+Height);
            //this.GetComponent<Transform>().localScale = prefab.transform.localScale;
            //this.GetComponent<Transform>().localScale.x = this.GetComponent<Transform>().localScale.x * Width;

            //addOneEmptyNodeAtEnd();

            //resizeParent();
        }

        public void updateToolbox(string[] nodeNames)
        {
            //string[] nodeN = { "", "", "", "", "", "" };
            //string[] nodeN = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //this.nodes = nodeN;
            this.nodes = nodeNames;
            //this.readyForUpdate = true;
            createNodes(nodes);
        }


        public void resizeParent()
        {
            //1. wir nehmen alle Kinder aus dem Parent raus
            //2. wir resizen den Parent
            //3. wir fügen die Kinder wieder ein

            Debug.Log("--Nodes:: " + nodes.Length);

            for (int i = 0; i < nodes.Length; i++)
            {
                //1. Wir nehmen die Kinder aus dem Parent raus
                NodeList[i].transform.parent = null;
            }

            //2.wir resizen den Parent
            this.GetComponent<Transform>().localScale = new Vector3(Width + 0.1f, Height + 0.1f, 0.03f);
            //this.GetComponent<Transform>().localScale = new Vector3(Width, Height,0.03f);
            this.GetComponent<Transform>().position = new Vector3(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y, this.GetComponent<Transform>().position.z + 0.02f);
            //this.GetComponent<Transform>().position = NodeList[0].transform.position;

            for (int i = 0; i < nodes.Length; i++)
            {
                //1. Wir stellen die kinder wieder in den Parent
                NodeList[i].transform.parent = this.transform;
            }

            this.GetComponent<Transform>().localScale = new Vector3((this.transform.localScale.x) / 2, (this.transform.localScale.y) / 2, 0.01f);
            //this.transform.localScale += new Vector3(0.1F, 0, 0);

            //addOneEmptyNodeAtEnd();

            //Wir machen hier nochmal ein Update, damit die Skalierung passt. Danach können wir das pdate sooft aufrufen, wie wir wollen.
            this.UpdateCollection();
        }

        public Vector3 getNextEmptyNodePosition()
        {
            //int children = this.transform.childCount;
            //return this.transform.InverseTransformDirection(nodeGrid[children-1]);
            //Debug.Log("++++EmptyNodeName+++: "+ NodeList[NodeList.Count - 1].Name);
            //Debug.Log("++++EmptyNodePosition+++: " + NodeList[NodeList.Count - 1].transform.position);
            for (int i = 0; i < NodeList.Count; i++)
            {
                if(NodeList[i].Name == "emptyNode")
                {
                    return NodeList[i].transform.position;
                }
            }

            return NodeList[NodeList.Count - 1].transform.position;
        }

        //WIr ferwenden die emptyNode ("emptyGameObjectNode") um noch eine Positionsreferenz zum Anzeigen von Nodes zu haben, die wieder in die Toolbox geplaced werden
        public void addOneEmptyNodeAtEnd()
        {
            //--Wenn noch Platz in der Toolbox ist, dann erzeugen wir an der letzten Stelle eine emptyNode
            if (!(NodeList.Count == Rows * _columns))
            {
                GameObject empty = new GameObject();
                empty.name = "emptyGameObjectNode";
                empty.transform.parent = this.transform;
                empty.tag = "emptyGameObjectNode";

                //Instantiate(prefab, this.transform);
            }
        }

        public void deleteAllNodes()
        {
            NodeList.Clear();

            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            //children.ForEach(child => child.GetComponent<Transform> = null);
            children.ForEach(child => DestroyImmediate(child));



            //Debug.Log("--Toolbox Kinder nach Delete: " + transform.childCount);
            //Debug.Log("--Neue Nodeliste: " + NodeList.Count);

        }


        public void insertEmpteOnRemoveposition(Vector3 posInGrid)
        {
            for (int i = 0; i < NodeList.Count; i++)
            {
                //Wir gehen durch die Nodeliste durch und schauen, welche Node gerade entfernt wurde
                if (NodeList[i].transform.parent == null )
                {
                    GameObject empty = new GameObject();
                    empty.name = "emptyNode";
                    empty.transform.parent = this.transform;
                    empty.transform.localPosition = posInGrid;
                    
                    NodeList[i].NodeGameObject = empty;
                    NodeList[i].transform = empty.transform;
                    NodeList[i].displayedInToolbox = true;
                    
                    NodeList[i].Name = empty.name;
                    
                }
            }
        }

    }

}
