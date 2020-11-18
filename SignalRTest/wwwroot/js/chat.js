'use strict';
// ===============================
// SignalR Client Sample
// ===============================
var SimpleChat = function (hubUrl) {
    const messageInput = document.getElementById('messageInput');
    const user = document.getElementById('userInput');
    const button = document.getElementById('sendButton');
    const list = document.getElementById('messagesList');

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect()
        .build();

    var _subscribed = false;

    //Disable send button until connection is established
    button.disabled = true;

    connection.on('ReceiveMessage', (user, message) => {
        let msg = message.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        let encodedMsg = user + ' says ' + msg;
        let li = document.createElement('li');
        li.textContent = encodedMsg;
        list.appendChild(li);
    });

    connection.start().then(() => {
        connection.invoke('Subscribe', user.value).then(result => {
            _subscribed = result;
            if (_subscribed) {
                button.disabled = false;
            } else {
                alert(`user ${user.value} is being used by someone else.`)
            }
        }).catch(error => handleError(error));
    }).catch(error => handleError(error));

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

    // sure closing page
    window.onunload = window.onbeforeunload = function () {
        // it is fired two times, _connected fix that
        if (connection !== null && _subscribed) {
            connection.invoke('Unsubscribe', user.value);
        }
    };

    function handleError(error) {
        console.error('Exception', error.toString());
    }
}