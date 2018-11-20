using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rosapi = RosSharp.RosBridgeClient.Services.RosApi;

namespace RosSharp.RosBridgeClient
{
    public class SceneInformation : MonoBehaviour
    {

        //zum Testen eingefügt: Hier sollen die Namen der Nodes gespeichert werden.
        //public static string[] nodenames = new string[14];
        //public static string[] nodenames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        public static string[] nodenames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        //public static string[] nodenames = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string[] topicnames;
        //private bool readyForeNodeDetails = false;
        private string currentNode;

        public LinkedList<ROSNode> nodelist = new LinkedList<ROSNode>();

        private ObjectCollectionNodes script;
        private NodeManager nodeManager;


        // Use this for initialization
        void Start()
        {

            script = GameObject.FindGameObjectWithTag("testCubeToolbox").GetComponent<ObjectCollectionNodes>();
            nodeManager = GameObject.Find("Rviz2AR_ToolBox").GetComponent<NodeManager>();

            //Get all the Nodes
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.NodesRequest, rosapi.NodesResponse>("/rosapi/nodes", ServiceCallHandler, new rosapi.NodesRequest());
            //Get all the Topics
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicsRequest, rosapi.TopicsResponse>("/rosapi/topics", ServiceCallHandler, new rosapi.TopicsRequest());
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.GetParamRequest, rosapi.GetParamResponse>("/rosapi/get_param", ServiceCallHandler, new rosapi.GetParamRequest("/rosdistro", "default"));

            //GetComponent<RosConnector>().RosSocket.CallService<rosapi.NodeDetailsRequest, rosapi.NodeDetailsResponse>("/rosapi/node_details", ServiceCallHandler, new rosapi.NodeDetailsRequest("/gazebo"));

            //Debug.Log("////////////////////////nodenames: " + nodenames.Length);


            //script.updateToolbox(nodenames);
            //wir erstellen nodes aus den Nodenamen, die wir uns geben lassen
            nodeManager.generateNodes(nodenames);

        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (readyForeNodeDetails)
            {
                GetNodeInfo();
            }
            */

        }



        private void ServiceCallHandler(rosapi.TopicsResponse message)
        {
            //Debug.Log("+++++ Here comes the Topics: ++++++++");
            topicnames = message.topics;
            //Debug.Log("Topicnames hat die Länge: "+ topicnames.Length);
            for (int i = 0; i < message.topics.Length; i++)
            {
                Debug.Log("ROS topic: " + topicnames[i]);
            }

            //Debug.Log("+++++ That were the Topics: ++++++++");
        }

        private void ServiceCallHandler(rosapi.NodesResponse message)
        {
            //Debug.Log("+++++Here comes the Nodes: +++++");
            nodenames = message.nodes;
            //Debug.Log("nodenames hat die Länge: " + nodenames.Length);
            for (int i = 0; i < nodenames.Length; i++)
            {
                Debug.Log("ROS Node: " + message.nodes[i]);
                

            }

            //Debug.Log("////////////////////////nodenames: " + nodenames.Length);
            //GetComponent<ObjectCollectionNodes>().createNodes(nodenames);
            //GameObject.FindGameObjectWithTag("Node_toolbox").GetComponent<ObjectCollectionNodes>().createNodes(nodenames);
            script.updateToolbox(nodenames);
            nodeManager.generateNodes(nodenames);
            //readyForeNodeDetails = true;



        }

        private void ServiceCallHandler(rosapi.NodeDetailsResponse message)
        {
            Debug.Log("--------------Node Details: ");

            ROSNode n = new ROSNode(currentNode);

            n.Subscribing = message.subscribing;
            n.Publishing = message.publishing;
            n.Service = message.services;


            n.ToString();

            nodelist.AddLast(n);

            Debug.Log("-------------Finished Detail: ");


        }

        private void ServiceCallHandler(rosapi.GetParamResponse message)
        {
            Debug.Log("Here comes the Ros Distro: ");
            Debug.Log("ROS distro: " + message.value);
        }

        private void GetNodeInfo()
        {
            for (int i = 0; i < nodenames.Length; i++)
            {
                if( i == nodenames.Length)
                {
                    //readyForeNodeDetails = false;
                }
                else
                {
                    currentNode = nodenames[i];
                    Debug.Log("ROS Node: " + nodenames[i]);
                    GetComponent<RosConnector>().RosSocket.CallService<rosapi.NodeDetailsRequest, rosapi.NodeDetailsResponse>("/rosapi/node_details", ServiceCallHandler, new rosapi.NodeDetailsRequest(nodenames[i]));

                }


            }
            
        }
    }
}


namespace RosSharp.RosBridgeClient
{
    public class ROSNode
    {
        private string nodeName;
        private string[] subscribing;
        private string[] publishing;
        private string[] service;


        public ROSNode(string node)
        {
            NodeName = node;
        
        }

        public string NodeName
        {
            get
            {
                return nodeName;
            }

            set
            {
                nodeName = value;
            }
        }

        public string[] Subscribing
        {
            get
            {
                return subscribing;
            }

            set
            {
                subscribing = value;
            }
        }

        public string[] Publishing
        {
            get
            {
                return publishing;
            }

            set
            {
                publishing = value;
            }
        }

        public string[] Service
        {
            get
            {
                return service;
            }

            set
            {
                service = value;
            }
        }


        public void ToString()
        {
            Debug.Log("///////-----Node: "+ nodeName);
            Debug.Log("");
            Debug.Log("-----Subscribing to: " );

            for (int i = 0; i < subscribing.Length; i++)
            {
                Debug.Log("--- " + subscribing[i]);
            }

            Debug.Log("-----Publishing to: " );

            for (int i = 0; i < publishing.Length; i++)
            {
                Debug.Log("--- " + publishing[i]);
            }

            Debug.Log("-----Services: ");

            for (int i = 0; i < service.Length; i++)
            {
                Debug.Log("--- " + service[i]);
            }

        }
    }
}