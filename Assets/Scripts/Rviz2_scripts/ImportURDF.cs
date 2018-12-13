using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;


namespace RosSharp.UrdfImporter
{
    public class ImportURDF : MonoBehaviour
    {

        private static int protocolNumber = 1; //WebSocketNetProtocol
        private static string address = "ws://192.168.56.101:9090";
        //private static string assetPath = "C:\\Users\\Mischa\\Documents\\Unity\\RosSharp-UWP-Master\\Assets\\Urdf";
        private static string assetPath = Path.Combine(Path.Combine(Path.GetFullPath("."), "Assets"), "Urdf");

        private static int timeout = 10;

        private bool imported = false;

        Thread rosSocketConnectThread;

        bool _threadRunning;
        Thread _thread;


        //private RosImportHandler importHandler;

        // Use this for initialization
        void Start()
        {
            /*
            importHandler = new RosImportHandler();


            Debug.Log("Starting the import Thread");
            rosSocketConnectThread = new Thread(() => importHandler.BeginRosImport(protocolNumber, address, timeout, assetPath));
            rosSocketConnectThread.Start();

           // Begin our heavy work on a new thread.
            _thread = new Thread(ThreadedWork);
            _thread.Start();

            Debug.Log("It should be finished...");
            */

        }
        // Update is called once per frame
        void Update()
        {
           /*
            if (importHandler.statusEvents["resourceFilesReceived"].WaitOne(0) && !importHandler.statusEvents["importComplete"].WaitOne(0))
            {
                importHandler.GenerateModelIfReady();
            }
            
            */
        }

        void ThreadedWork()
        {
            _threadRunning = true;
            bool workDone = false;
            int i = 0;

            // This pattern lets us interrupt the work at a safe point if neeeded.
            while (_threadRunning && !workDone)
            {
                Debug.Log("Thread Priiiinnnnttt:" + i);
                i++;

                if (i == 10) ;
                workDone = true;
            }
            _threadRunning = false;
        }
    }
}
