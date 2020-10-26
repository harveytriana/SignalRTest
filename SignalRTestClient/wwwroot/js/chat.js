'use strict';
// ===============================
// SignalR Client Sample
// ===============================
var SimpleChat = function (hubUrl) {
    const messageInput = document.getElementById('messageInput');
    const user = document.getElementById('userInput');
    const button = document.getElementById('sendButton');
    const list = document.getElementById('messagesList');

    const connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

    //Disable send button until connection is established
    button.disabled = true;

    connection.on('ReceiveMessage', function (user, message) {
        let msg = message.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        let encodedMsg = user + ' says ' + msg;
        let li = document.createElement('li');
        li.textContent = encodedMsg;
        list.appendChild(li);
    });

    connection.start().then(function () {
        button.disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    button.addEventListener('click', function (event) {
        event.preventDefault();
        connection.invoke('SendMessage', user.value, messageInput.value).catch(function (err) {
            return console.error(err.toString());
        });
    });

    messageInput.addEventListener('keyup', function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            button.click();
        }
    });
}