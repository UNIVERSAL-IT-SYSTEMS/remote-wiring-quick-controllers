var WebSocketServer = require('ws').Server
  , wss = new WebSocketServer({ port: 8080 });

var robot;
var controller;

// scope or die mf
wss.on('connection', function connection(ws) {
  ws.on('message', function incoming(message) {
    console.log('received: %s', message);
    
    try {
      var msg = JSON.parse(message);
      
      if (msg.type === 'robot') {
        robot = ws;
        console.log('I am a robot!');
        
        // set listener specifically for robot messages
        robot.addEventListener('message', robotListener);
      }
      else if (msg.type === 'controller') {
        controller = ws;    
        console.log('I am a controller!');
        
        // set listener specifically for controller messages
        controller.addEventListener('message', controllerListener);
      }
    } catch (error) {
      console.log('parse failed for' + message);
    } 
  });
    
  ws.send('Connected');
});

function robotListener(data) {
  if (controller !== undefined) {
    // send video data robot -> controller
    controller.send(data.data);
  }
  else {
    console.log('Connect controller to server!');
  }
}

function controllerListener(data) {
  if (robot !== undefined) {
    // send commands controller -> robot
    robot.send(data.data);
  } else {
    console.log('Connect robot to server!');
  }
}


