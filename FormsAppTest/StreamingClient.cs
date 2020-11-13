using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
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

        public async Task<bool> ConnectAsync()
        {
            try {
                _connection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .Build();

                await _connection.StartAsync(_cts.Token);

                Prompt?.Invoke($"Hub is Started. Waiting Signals.");

                _connected = true;
            }
            catch (Exception exception) {
                Prompt?.Invoke($"Exception: {exception.Message}");
                _connected = false;
            }

            return _connected;
        }

        #region Server to Client
        // the data is streamed from the server to the client.
        // Hub's method note:
        // Counter1: ChannelReader<T>
        // Counter2: IAsyncEnumerable<T>
        // Both are valid for these methods:
        //
        public async Task ReadStream()
        {
            if (!_connected) {
                return;
            }
            var channel = await _connection.StreamAsChannelAsync<int>("Counter2", 16, 333, _cts.Token);
            while (await channel.WaitToReadAsync()) {
                while (channel.TryRead(out int data)) {
                    Prompt?.Invoke($"Received {data}");
                }
            }
            Prompt?.Invoke("Complete");
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

        #region Client to Server
        // Basic sample of MS
        public async Task SendStreamBasicDemotration()
        {
            if (!_connected) {
                return;
            }
            var channel = Channel.CreateBounded<string>(10);
            await _connection.SendAsync("UploadStream", channel.Reader);
            await channel.Writer.WriteAsync("some data");
            await channel.Writer.WriteAsync("some more data");
            channel.Writer.Complete();

            Prompt?.Invoke("Complete");
        }

        // HUB
        // UploadStream: parameter is ChannelReader<string> stream
        // UploadStream2: parameter is IAsyncEnumerable<string> strea
        //
        public async Task SendStream()
        {
            if (!_connected) {
                return;
            }
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
        public async Task SendStream2()
        {
            await _connection.SendAsync("UploadStream2", ClientStreamData());
        }

        async IAsyncEnumerable<string> ClientStreamData()
        {
            for (var i = 0; i < 8; i++) {
                var s = $"Some data {i}";
                Prompt?.Invoke($"Sending -> {s}");
                await Task.Delay(333);
                yield return s;
            }
            Prompt?.Invoke("Complete");
        }
        #endregion

        public void Dispose()
        {
            if (_connected) {
                Prompt?.Invoke("Unsubscribed");
                //_h.InvokeAsync("Unsubscribe", _ItemId).Wait();

                _connection.StopAsync();
                _connection.DisposeAsync();
            }
        }
    }
}
