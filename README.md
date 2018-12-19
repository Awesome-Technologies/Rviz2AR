This document describes the Rviz2AR Project as well as how to prepare your evironment to use Rviz2AR in your Unity 3D project with ROS.

# Rviz2AR

## What is Rviz2AR
The goal of this Project is to provide a stable communication for data exchange between ROS and the Microsoft Hololense as well as an integration for the game engine Unity for the creation and visualization of Robot data. The Project should be upscaleable for different hardware, allowing the use of AR and VR devices for different tasks. An Interface toolkit for robot data visualization should be developed and the capabilities of the application should be demonstrated with a showcase using industrial robot hardware running ROS.

![Rviz2ARStruct](/External/ReadMeImages/images/Rviz2AR.JPG "Rviz2AR structure")

The project is currently in Milestone 1 where we are working on the software module for a stable communication link between the Unity application running on AR/VR gear and ROS, enabling to send and receive live data.

##How does Rviz2AR work?

The communication infrastructure of Rviz2AR is based on the [ROS-Sharp(ROS#)](https://github.com/siemens/ros-sharp) module. [ROS#](https://github.com/siemens/ros-sharp) is using [rosbridge_suite ](https://github.com/RobotWebTools/rosbridge_suite) to provides ROS-Unity communication via JASON API using [ROSBridge Server](http://wiki.ros.org/rosbridge_server?distro=kinetic)




It should be mentioned that, even though our Rvis2AR now provides a working data communication between Unity and ROS it is not without shortcomings. While the underlying  JASON API is sufficient in supporting the exchange of small data, larger data files like camera feeds or point cloud data are not handled efficiently. To compensate those limitations we plan to incorporate another communication link between Unity and ROS based on ROS2. To this end, we are in contact with the team of ARViz: Augmented Reality Visualizer for ROS2 by Francisco Martin Rico. Our goal would be to enhance our currently implementation with a ROS2 DDS or larger data files. Since the ARViz Project is still in active development, we will incorporate it into our project in the near future, when it is available.

##What is Rviz2AR capable of right now?
Rviz2AR is still in active developmentvand and is not feature complete at the moment.

The current build serves the purpose of demonstrating the general communication infrastructue for ROS-Unity data exchange running on the Microsoft Hololense. 

A preview for the visualization toolbox, that gives the user the option to interact with the ROS data in AR-space is also implemented.

# 1. Setting up your development environment

## Check your Windows 10 version
It is recommended to be running the Windows 10 Fall Creators update for modern Windows Universal apps targeting Mixed Reality Headsets

This can be verified by running the "WinVer" application from the Windows Run command

`Windows Key + R -> WinVer`

Which will display a new window as follows:

![Windows Version dialog](/External/ReadMeImages/images/WindowsVersionFCU.png)

If you are not running the Windows 10 Fall Creators update, then you will need to Update your version of Windows.

## Set up your development environment

Be sure to enable Developer mode for Windows 10 via:

`Action Center -> All Settings -> Update & Security -> For Developers -> Enable Developer mode`

![Enable Developer Mode](/External/ReadMeImages/EnableDevModeWin10.PNG "Enable Developer Mode for Windows 10")

If you have not already, download and install [Visual Studio 2017](https://www.visualstudio.com/vs/) and these required components:

- Windows Universal Platform Development Workload
- Windows SDK 10.16299.10
- Visual Studio Tools for Unity
- msbuild
- Nuget Package Manager

![Visual Studio Components](/External/ReadMeImages/VisualStudioComponents.PNG)

You can install more components and UWP SDK's as you wish.

Make sure you are running the appropriate version of Unity 3D on your machine. You should [download and install the latest version](https://unity3d.com/get-unity/download/archive) this project says it supports on the [main readme page](/README.md).

[unity-release]:             https://unity3d.com/unity/qa/patch-releases/2017.2.1p2
[unity-version-badge]:       https://img.shields.io/badge/Unity%20Editor-2017.2.1p2-green.svg

> The Mixed Reality Toolkit now recommends the following Unity 3D version:
> [![Github Release][unity-version-badge]][unity-release] 

_Note: Be sure to include the Windows Store .NET scripting backend components._

![Unity Installer](/External/ReadMeImages/UnityInstaller.PNG "Unity Installer")

# 3. Ubuntu on Oracle VM VirtualBox
If you want to use ROS from a gazebo Simulator tis chapter explains how to set up Oracle VM VirtualBox to run ROS in a Gazebo simulation

Download the [latest version of Virtual Box from here](https://www.virtualbox.org/wiki/Downloads) from here and install it

## Setting up the Network
In Oracle VirtualBox, make sure the Ubuntu VM is powered off. Then open the settings for the Ubuntu VM. In the Network tab, add two new network adapters:


* Host-only Adapter
![](https://raw.githubusercontent.com/wiki/siemens/ros-sharp/img/User_Inst_UbuntuOnOracleVM_HostOnlyAdapter.PNG)

* Network Bridge
![](https://raw.githubusercontent.com/wiki/siemens/ros-sharp/img/User_Inst_UbuntuOnOracleVM_BridgedAdapter.PNG)
In both set the Promiscuous mode to allow VMs and set the Cable Connected tag.

In Ubuntu type `$ ifconfig` to check the network configuration and to verify your Ethernet connection to the Windows OS. The IP address (enp0s8) of the Ubuntu system will be used by [rosbridge_suite ](https://github.com/RobotWebTools/rosbridge_suite) and RosBridgeClient.

These settings are needed so that the RosBridgeClient running in Windows, and the  [ROSBridge Server](http://wiki.ros.org/rosbridge_server?distro=kinetic) running on Ubuntu can communicate.

## Setting up ROS on Ubuntu VM

* Follow this [tutorial](http://wiki.ros.org/kinetic/Installation/Ubuntu) to install ROS Kinetic
* Follow [this tutorial](http://wiki.ros.org/ROS/Tutorials/InstallingandConfiguringROSEnvironment) to install and configure the ROS environment
* Install rosbridge-suite via
 `$ sudo apt-get install ros-kinetic-rosbridge-server`

See [http://wiki.ros.org/rosbridge_suite](http://wiki.ros.org/rosbridge_suite) for further information.

<a name="setupVM"></a>
# 4.[Gazebo](http://gazebosim.org/) Setup for VM with Turtlebot2
This tutorial shows how to install and setup [Gazebo](http://gazebosim.org/), which is used for robot simulation in ROS.

Using the following commands, setup your Ubuntu VM to accept software from packages.osrfoundation.org and install the Gazebo7 packages

* ``$ sudo sh -c 'echo "deb http://packages.osrfoundation.org/gazebo/ubuntu-stable `lsb_release -cs` main" > /etc/apt/sources.list.d/gazebo-stable.list'``
* ``$ wget http://packages.osrfoundation.org/gazebo.key -O - | sudo apt-key add -``
* ``$ sudo apt-get update``
* ``$ sudo apt-get install gazebo7``
 

Install the required packages

* ``$ sudo apt-get install libgazebo7-*``


Note: Visit the [gazebosim](http://gazebosim.org/tutorials/?tut=ros_wrapper_versions) webpage and its [tutorial](http://gazebosim.org/tutorials?tut=install_ubuntu) page for more details.

## Install TurtleBot2
This tutorial explains how to set up the example robot [TurtleBot2](https://www.turtlebot.com/turtlebot2/).

* Install the required TurtleBot2 sources on the Ubuntu VM
`$ sudo apt-get install ros-kinetic-turtlebot ros-kinetic-turtlebot-apps ros-kinetic-turtlebot-interactions ros-kinetic-turtlebot-simulator ros-kinetic-kobuki-ftdi ros-kinetic-ar-track-alvar-msgs ros-kinetic-turtlebot-gazebo`


## Gazebo Simulation Example Setup

* create a new Catkin workspace

```
$ mkdir -p ~/catkin_ws/src
$ cd ~/catkin_ws/src
$ catkin_init_workspace
$ cd ..
$ catkin_make
```

* Place the folder `gazebo_simulation_scene` (from [here](https://github.com/siemens/ros-sharp/tree/master/ROS)) inside the src folder of your Catkin workspace and rebuild your workspace.

* In the directory g`gazebo_simulation_scene/scripts` make the file `joy_to_twist.py` executable by running

``chmod +x joy_to_twist.py
``

* rebuild your Catkin workspace

```
$ catkin_make
```

## Run Rviz2Ar with Gazebo Simulation Example

* Run the following command in the Ubuntu terminal

```
$ roslaunch gazebo_simulation_scene gazebo_simulation_scene.launch
``` 

This will launch `rosbridge_websocket`, `file_server`, `joy_to_twist`, `rqt_graph` and a [Gazebo](http://gazebosim.org/) simulation of the [TurtleBot2](https://www.turtlebot.com/turtlebot2/). in the default `turtlebot_world`

* Open a ner tab in the termina and type 

`$ ifconfig`

* Look for the line `enp0s9`
* There ´, look for the line `inet addr`
* Copy the ip displayed there (for example: 192.168.2.198)

![Ifconfig](/External/ReadMeImages/images/ifconfig.JPG "ifconfig enp0s9")

Open the Rviz2AR Project in your Unity Editor

* In the Unity scene look for the RosConnector
* In the RosConnector look for the script Ros Connector wih the field `Ros Bridge Server Url`
* Past your coppied IP there so it has the form: `ws://yourIP:9090` (so if your coppied IP was `192.168.2.198` it would be `ws://192.168.2.198:9090`)


![Ros Connector](/External/ReadMeImages/images/RosConnector.JPG " Ros Connector")

* Press play in the inspector to start the application

## Run Rviz2Ar with Gazebo Simulation on Hololense

* Make sure, that the Hololense is conencted to the same network as the Computer/VM

In Unity Editor go to `File` -> `Build Settings`

* Make sure that Universal WIndoes Platform is selected

![BuildSettings](/External/ReadMeImages/images/buildSettings.JPG " Build Settings")

* Click on `Player Settings`
* Go to `Other Setting` and make sure, that in the tab `Configuration` the options `Scripting Runtime version`, `Scripting backend` and `Api Compatibility Level` are selected with `.NET 4.x Equivaent`, `.NET` and `.NET 4.x`

![Config](/External/ReadMeImages/images/config.JPG " Configuration")

* Go to `Publish Setting` and make sure, that in the tab `Capabilities` the options `InternetClientServer`, `PrivateNetworkClientServer` and `SpatialPerception` are selected

![Capa](/External/ReadMeImages/images/capabilities.JPG " Capabilities")

Go Back to the Build Settings and press `Build` and select a Folder where the Project should be build

Go the the build Folder and select the `.sln` File to open the Project in Visual Studio

Make sure your Hololense is connected and select `Release`, `x86` and `Device` in the Visual Studio deploy settings

![deploy settings](/External/ReadMeImages/images/holoSettings.JPG " VS deploy settings")


Click on the `> Device`- Button to deploy the project to the Hololense


# 5.[Gazebo](http://gazebosim.org/) Setup for VM with UR5 Robot

Install the VM and Gazebo as described in [chapter 4.](#abcd)
GO to [The ROS UR5 page](http://wiki.ros.org/universal_robot)) and install the UR5 in your VM according to the description.

In your catkin workspace go to  `src -> universal_robot -> ur_gazebo -> launch` and open the `ur5.launch` file

In the lauch file, add the following lines, which will start the rosbridge websocket:





	<include file="$(find rosbridge_server)/launch rosbridge_websocket.launch">
		<param name="port" value="9090"/>
	</include>

