using HoloToolkit.Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using rosapi = RosSharp.RosBridgeClient.Services.RosApi;

namespace RosSharp.RosBridgeClient
{
    public class SceneInformation : MonoBehaviour
    {

        public static string[] nodenames = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        
        //Names of the topics that we get from ROS
        public static string[] topicnames;

        //Tyes der verschiedenen TOpics
        public static List<string> topictypes = new List<string>();

        //Dictionary of Topic names and Date types ot the topics
        public Dictionary<string, string> TopicsAndDataTypes = new Dictionary<string, string>();
        public string currentTopic;
        //private bool readyForeNodeDetails = false;
        private bool readyForeTopicType = false;
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

            GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicTypeRequest, rosapi.TopicTypeResponse>("/rosapi/topic_type", TopicTypeServiceCallHandler, new rosapi.TopicTypeRequest("/joy"));

            //get tf2_msgs/FrameGraph Service
            //GetComponent<RosConnector>().RosSocket.CallService<rosapi.FrameGraphRequest, rosapi.FrameGraphResponse>("/tf2_msgs/FrameGraph", FrameGraphServiceCallHandler, new rosapi.FrameGraphRequest());

            //GetComponent<RosConnector>().RosSocket.CallService<rosapi.NodeDetailsRequest, rosapi.NodeDetailsResponse>("/rosapi/node_details", ServiceCallHandler, new rosapi.NodeDetailsRequest("/gazebo"));

            //Debug.Log("////////////////////////nodenames: " + nodenames.Length);


            //script.updateToolbox(nodenames);
            //wir erstellen nodes aus den Nodenamen, die wir uns geben lassen
            //nodeManager.generateNodes(nodenames);

            //nodeManager.generateNodes(topicnames);

            StartCoroutine(GenerateNodes());

        }

        IEnumerator GenerateNodes()
        {
            
            //print(Time.time);
            yield return new WaitForSeconds(1);
            //print(Time.time);
            StartCoroutine(GetTopicType());
            
            yield return new WaitForSeconds(1);

            foreach (KeyValuePair<string, string> kvp in TopicsAndDataTypes)
            {
                Debug.Log(kvp.Key + "has the type: " + kvp.Value);
            }
            nodeManager.generateNodes(topicnames);
        }

        // Update is called once per frame
        void Update()
        {
            
            /*
            if (readyForeTopicType)
            {
                readyForeTopicType = false;
                GetTopicType();
            }
            */
            
        }



        private void ServiceCallHandler(rosapi.TopicsResponse message)
        {
            //Debug.Log("+++++ Here comes the Topics: ++++++++");
            topicnames = message.topics;
            Debug.Log("Topicnames hat die Länge: "+ topicnames.Length);
            for (int i = 0; i < message.topics.Length; i++)
            {
                //Debug.Log("ROS topic: " + topicnames[i]);
                //GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicTypeRequest, rosapi.TopicTypeResponse>("/rosapi/topic_type", ServiceCallHandler, new rosapi.TopicTypeRequest(topicnames[i]));

            }
            readyForeTopicType = true;
            //Debug.Log("+++++ That were the Topics: ++++++++");

            //nodeManager.generateNodes(topicnames);
        }

        private void FrameGraphServiceCallHandler(rosapi.FrameGraphResponse message)
        {
            Debug.Log("+++++ Here comes the FrameGraphResponse: ++++++++");
            string frameYaml = message.frame_yaml;

            Debug.Log("+++++ frameYaml: "+ frameYaml);

        }




        IEnumerator GetTopicType()
        {
            Debug.Log("GetTopicType: topicnames.length" + topicnames.Length);
            for (int i = 0; i < topicnames.Length; i++)
            {
                currentTopic = topicnames[i];
                yield return StartCoroutine(TopicNamyAndType(topicnames[i]));
                /*
                Task.Run(async () => {

                    await Task.Delay(1000);
                    currentTopic = topicnames[i];
                    GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicTypeRequest, rosapi.TopicTypeResponse>("/rosapi/topic_type", TopicTypeServiceCallHandler, new rosapi.TopicTypeRequest(topicnames[i]));

                    
                    
                });
                */

                //currentTopic = topicnames[i];
                //GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicTypeRequest, rosapi.TopicTypeResponse>("/rosapi/topic_type", TopicTypeServiceCallHandler, new rosapi.TopicTypeRequest(topicnames[i]));


                //Debug.Log("ROS Topic: " + topicnames[i]);

            }

        }

        IEnumerator TopicNamyAndType(string topic)
        {
            GetComponent<RosConnector>().RosSocket.CallService<rosapi.TopicTypeRequest, rosapi.TopicTypeResponse>("/rosapi/topic_type", TopicTypeServiceCallHandler, new rosapi.TopicTypeRequest(topic));
            yield return new WaitForSeconds(0.05f);
        }

        private void TopicTypeServiceCallHandler(rosapi.TopicTypeResponse message)
        {
            Debug.Log("Die topic: " + currentTopic);

            Debug.Log("Hat den Typ: "+ message.type);

            TopicsAndDataTypes.Add(currentTopic, message.type);
            Debug.Log("+++++++++Die Topic: " + currentTopic + " hat den Typ: " + TopicsAndDataTypes[currentTopic]);
        }

        


            private void ServiceCallHandler(rosapi.NodesResponse message)
        {
            //Debug.Log("+++++Here comes the Nodes: +++++");
            nodenames = message.nodes;
            //Debug.Log("nodenames hat die Länge: " + nodenames.Length);
            for (int i = 0; i < nodenames.Length; i++)
            {
                //Debug.Log("ROS Node: " + message.nodes[i]);
                

            }

            



        }







        /*
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
        */

        private void ServiceCallHandler(rosapi.GetParamResponse message)
        {
            Debug.Log("Here comes the Ros Distro: ");
            Debug.Log("ROS distro: " + message.value);
        }

        /*
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
        */
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