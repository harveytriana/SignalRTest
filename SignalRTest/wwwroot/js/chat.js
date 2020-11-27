'use strict';
// ===============================
// SignalR Client Sample
// ===============================
var SimpleChat = function (hubUrl) {
    const messageInput = document.getElementById('messageInput');
    const userInput = document.getElementById('userInput');
    const button = document.getElementById('sendButton');
    const list = document.getElementById('messagesList');
    const status = document.getElementById('status');

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect()
        .build();

    var _user = userInput.value;
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

    connection.on('ConnectedClients', (clientsCount) => {
        status.textContent = 'Conected lientes: ' + clientsCount;
    });

    connection.start().then(() => {
        connection.invoke('Subscribe', _user).then(result => {
            _subscribed = result;
            if (_subscribed) {
                button.disabled = false;
            } else {
                alert(`user ${_user} is being used by someone else. Set new user and press Enter.`)
            }
        }).catch(error => handleError(error));
    }).catch(error => handleError(error));

    button.addEventListener('click', function (event) {
        event.preventDefault();
        connection.invoke('SendMessage', _user, messageInput.value).catch(function (err) {
            return console.error(err.toString());
        });
    });

    messageInput.addEventListener('keyup', function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            button.click();
        }
    });
    userInput.addEventListener('keyup', function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            changeUser();
        }
    });

    function changeUser() {
        if (_user !== userInput.nodeValue) {
            connection.invoke('Subscribe', userInput.value).then(result => {
                _subscribed = result;
                if (_subscribed) {
                    _user = userInput.value;
                    button.disabled = false;
                } else {
                    alert(`user ${_user} is being used by someone else.`)
                }
            }).catch(error => handleError(error));
        }
    }

    // sure closing page
    window.onunload = window.onbeforeunload = function () {
        // it is fired two times, _connected fix that
        if (connection !== null && _subscribed) {
            connection.invoke('Unsubscribe', _user);
        }
    };

    function handleError(error) {
        console.error('Exception', error.toString());
    }
}