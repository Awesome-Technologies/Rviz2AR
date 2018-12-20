﻿/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JointTrajectorySubscriber : Subscriber<Messages.Trajectory.JointTrajectory>
    {
        private string[] joint_names;
        private Messages.Trajectory.JointTrajectoryPoint[] points;
        private bool isMessageReceived;
        //public GameObject assignedButtonObject;

        protected override void Start()
        {
            base.Start();

            //assignedButtonObject = GameObject.Find("2");
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(Messages.Trajectory.JointTrajectory message)
        {
            Debug.Log("Wir kriegen WAAAASSSSSSSSSSSSSSSSSSS");

            joint_names = message.joint_names;
            points = message.points;

            /*
            position = GetPosition(message).Ros2Unity();
            rotation = GetRotation(message).Ros2Unity();
            */
            isMessageReceived = true;
        }
        private void ProcessMessage()
        {
            Debug.Log("JointNames: "+ joint_names.ToString());
            //assignedButtonObject.transform.Find("TopicInformation").transform.Find("Text").gameObject.GetComponent<TextMesh>().text = position.ToString();
            
        }
        /*
        private Vector3 GetPosition(Messages.Navigation.Odometry message)
        {
            return new Vector3(
                message.pose.pose.position.x,
                message.pose.pose.position.y,
                message.pose.pose.position.z);
        }

        private Quaternion GetRotation(Messages.Navigation.Odometry message)
        {
            return new Quaternion(
                message.pose.pose.orientation.x,
                message.pose.pose.orientation.y,
                message.pose.pose.orientation.z,
                message.pose.pose.orientation.w);
        }
        */
    }
}