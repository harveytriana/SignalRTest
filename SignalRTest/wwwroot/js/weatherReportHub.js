// VISIONARY S.A.S.
// 
function MessagePackTest(hubUrl) {
    const _connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
        .build();

    const list = document.getElementById('messages-list');

    const _countries = [
        'Afghanistan',
        'Armenia',
        'Brazil',
        'Colombia',
        'Grenada',
        'Ireland',
        'Seychelles',
        'Swaziland',
        'Zimbabwe'
    ];
    const _userId = _newGuid();

    _connection.start().then(function () {
        document.getElementById('send-button').addEventListener('click', function () {
            for (var i = 1; i <= 12; i++) {
                var data = {
                    UserId: _userId, 
                    Temperature: getTemperature(),
                    Country: getCountry()
                };
                _connection.invoke('Send', data);
            }
        });
    });

    // N: notify from hub
    _connection.on('Receive', function (data) {
        let s = `<li>Sender: ${data.UserId.substring(0, 8)}    ${data.Country}: ${Number(data.Temperature).toFixed(2)} ºF</li>`;
        let li = document.createElement('li');
        li.innerHTML = s;
        list.appendChild(li);
    });

    document.getElementById('clear-button').addEventListener('click', function () {
        list.innerHTML = '';
    });

    // utilities
    function getTemperature() {
        return Number(60) + Math.floor(Math.random() * 1000 + 1) / 700;
    }

    function getCountry() {
        return _countries[Math.floor(Math.random() * _countries.length)];
    }
}
