using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FormsAppTest
{
    class StreamingClient : IDisposable
    {
        readonly string _hubUrl;

        HubConnection _connection;
        readonly CancellationTokenSource _cts = new CancellationTokenSource();
        bool _connected;

        public delegate void PromptEventHandler(string text);
        public event PromptEventHandler Prompt;

        public StreamingClient(string hubUrl)
        {
            _hubUrl = hubUrl;
        }

        public async Task ConnectAsync()
        {
            try {
                _connection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .Build();

                await _connection.StartAsync(_cts.Token);

                Prompt?.Invoke($"Hub is Started. Waiting Signals.");

                //TODO whats for?
                //_h.Closed += async (exception) =>
                //{
                //    Prompt?.Invoke(exception.Message);
                //    Prompt?.Invoke("restart");
                //    await Task.Delay(new Random().Next(0, 5) * 1000);
                //    await _h.StartAsync();
                //};
                _connected = true;
            }
            catch (Exception exception) {
                Prompt?.Invoke($"Exception: {exception.Message}");
                _connected = false;
            }
        }

        #region Server to Client
        // * does not use _connected for simply sample

        // the data is streamed from the server to the client.
        // Hub's method note:
        // Counter1: ChannelReader<T>
        // Counter2: IAsyncEnumerable<T>
        // Both are valid for these methods:

        // StreamAsChannelAsync<T>
        public async Task ReadStream()
        {
            // read from the hub using ChannelReader
            var channel = await _connection.StreamAsChannelAsync<int>("Counter2", 16, 333, _cts.Token);
            while (await channel.WaitToReadAsync()) {
                while (channel.TryRead(out int data)) {
                    Prompt?.Invoke($"Received {data}");
                }
            }
            Prompt?.Invoke("Streaming completed");
        }

        // StreamAsync<T>
        // valid for C# 8 -> .NET Core 3.0+
        // testing in console app ... new StreamingTest().StreamingAsync().Wait();
        //public async Task ReadStream2()
        //{
        //    var stream = _h.StreamAsync<int>("Counter2", 16, 333, _cts.Token);
        //    await foreach (var count in stream) {
        //        Prompt?.Invoke($"Received {count}");
        //    }
        //    Prompt?.Invoke("Streaming completed");
        //}
        #endregion

        #region Client-to-server streaming
        // Basic sample of MS
        public async Task SendStreamBasicDemotration()
        {
            var channel = Channel.CreateBounded<string>(10);
            await _connection.SendAsync("UploadStream", channel.Reader);
            await channel.Writer.WriteAsync("some data");
            await channel.Writer.WriteAsync("some more data");
            channel.Writer.Complete();
        }

        public async Task SendStream()
        {
            Prompt?.Invoke("SendStream");

            var channel = Channel.CreateBounded<string>(10);
            await _connection.SendAsync("UploadStream", channel.Reader);

            for (int i = 1; i < 8; i++) {
                var s = $"Some data {i}";
                Prompt?.Invoke($"Sending -> {s}");

                await channel.Writer.WriteAsync(s);
                await Task.Delay(333);
            }

            channel.Writer.Complete();
            Prompt?.Invoke("Complete");
        }

        // C# 8
        //public async Task SendStream2()
        //{
        //    await _h.SendAsync("UploadStream", ClientStreamData());
        //}
        //async IAsyncEnumerable<int> ClientStreamData()
        //{
        //    for (var i = 0; i < 16; i++) {
        //        await Task.Delay(333);
        //        yield return i;
        //    }
        //}
        #endregion

        public void Dispose()
        {
            if (_connected) {
                Prompt?.Invoke("Unsubscribed");
                //_h.InvokeAsync("Unsubscribe", _ItemId).Wait();

                Task.Run(async () => {
                    await _connection.StopAsync();
                    await _connection.DisposeAsync();
                });
            }
        }
    }
}
