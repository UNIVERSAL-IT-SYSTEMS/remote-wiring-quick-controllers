# cobra-rover
The cobra rover

## Overview
This is a robot project that uses a [DFRobot](http://www.dfrobot.com/) platform with a [Windows Phone](https://www.microsoft.com/en-us/mobile/phone/lumia635/?&dcmpid=bmc-src-google.brand) as the main controller. 
The robot can be controlled from another Windows 10 device such as your PC either through WiFi-Direct or a Node.js socket server.

## Getting Started
To get started you will need:
- Windows 10 PC
- [Windows 10 Phone](https://www.microsoft.com/en-us/mobile/phone/lumia635)
- [DFRobot robot platform](http://www.dfrobot.com/index.php?route=product/product&product_id=97#.VXvEOehViko)
- [DFRobot Romeo controller](http://www.dfrobot.com/index.php?route=product/product&product_id=1176&search=romeo&description=true#.VXvD7uhViko)

Software
- [Python](https://www.python.org/)
- [Node.js](https://nodejs.org)
- [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads/visual-studio-2015-downloads-vs.aspx)
- [Git](https://git-scm.com/)


## Building the App
Open the solution and press the build button.

## Hooking Everything Up
Get the code

```
git clone --recursive https://github.com/ms-iot/cobra-rover.git
```

### Using Sockets
- Navigate to the directory where you cloned this repository and then go to the `RoverServer` folder. This folder contains a small Node.js server that allows the robot and controller to connect.
  - To run this server from the command line type
  ```
  node server.js
  ```
  - You should see `Server listening on port 8080`
  
- On your PC run the UWPRobotController
  - Pick sockets in the drop down
  - Type in the correct socket address e.g.
  
    > ws://localhost:8080

  - You should now be able to press the forward button and see the server print out the command and tell you to connect the robot.
  
### Using WiFi Direct

