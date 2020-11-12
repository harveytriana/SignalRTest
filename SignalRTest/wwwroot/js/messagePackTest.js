// ===============================
// VISIONARY S.A.S.
// ===============================
var MessagePackTest = function (hubUrl) {
    var _h = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
        .build();

    var list = document.getElementById('messages-list');

    _h.start().then(function () {
        document.getElementById('send-button').addEventListener('click', function () {
            for (var i = 1; i <= 12; i++) {
                var data = {
                    ConnectionId: '', // the server assigns it
                    Temperature: getTemperature()
                };
                _h.invoke('Send', data);
            }
        });
    });

    // N: notify from hub
    _h.on('Receive', function (data) {
        let s = '<li>User: ' + data.ConnectionId + '&nbsp;&nbsp;&nbsp;&nbsp;Temperature: ' + data.Temperature + 'º</li>';
        let li = document.createElement('li');
        li.innerHTML = s;
        list.appendChild(li);
    });

    document.getElementById('clear-button').addEventListener('click',function () {
        list.innerHTML = '';
    });

    // utility by sample
    function getTemperature() {
        return Number(60) + Math.floor(Math.random() * 1000 + 1) / 700;
    }
}
