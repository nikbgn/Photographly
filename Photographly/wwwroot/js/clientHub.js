"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chat/ConnectedHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var newMsg = document.createElement("p");
    document.getElementById("messagesList").appendChild(newMsg);

    newMsg.textContent = `[${new Date().toLocaleTimeString()}] ${user}: ${message}`;
    newMsg.id = "chatMessageText";
    newMsg.className = "text-light m-2";
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
