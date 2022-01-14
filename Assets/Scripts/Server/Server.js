const WebSocket = require('ws');

const server = new WebSocket.Server({port: 8080}, () => {
    console.log("Server started")    
});


let sockets = [];
server.on('connection', function connection(socket) {
    sockets.push(socket);
    socket.on('message', function (msg) {
        console.log(msg.toString());
        socket.send(msg.toString());
    })
    socket.on('close', function (){
        sockets = sockets.filter(s => s !== socket);
    });
});

