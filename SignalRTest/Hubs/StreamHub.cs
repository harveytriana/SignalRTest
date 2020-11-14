// ===============================
// VISIONARY S.A.S.
// ===============================
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRTest.Shared;

//! SOURCES
// https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-3.1
// https://csharp.christiannagel.com/2019/10/08/signalrstreaming/
// review
// https://github.com/dotnet/AspNetCore.Docs/issues/20562

namespace SignalRTest.Hubs
{
    public class StreamHub : Hub
    {
        Tracer _tracer;

        public StreamHub(Tracer tracer)
        {
            _tracer = tracer;
            _tracer.Start("SignalRTest_StreamHub");
        }

        #region Server-to-client streaming
        // hub method becomes a streaming hub method when it returns IAsyncEnumerable<T>, ChannelReader<T>
        // or async versions
        // first approach, ChannelReader<T>
        //
        // NOTE
        // 2020 - can? public async Task<ChannelReader<int>> CounterChannel( 
        // NOT (Exception thrown: System.Reflection.TargetInvocationException)
        //
        public ChannelReader<int> CounterChannel(
                   int count,
                   int delay,
                   CancellationToken cancellationToken)
        {
            _tracer.Log($"Run ChannelReader<int> CounterChannel(count: {count}, delay: {delay})");

            var channel = Channel.CreateUnbounded<int>();

            // We don't want to await WriteItemsAsync, otherwise we'd end up waiting 
            // for all the items to be written before returning the channel back to
            // the client.
            //- _ = WriteItemsAsync(channel.Writer, count, delay, cancellationToken);

            Exception localException = null;

            Task.Run(async () => {
                try {
                    for (var i = 0; i < count; i++) {
                        // Use the cancellationToken in other APIs that accept cancellation
                        // tokens so the cancellation can flow down to them.
                        // cancellationToken.ThrowIfCancellationRequested();

                        // ChannelWriter sends data to the client
                        await channel.Writer.WriteAsync(i, cancellationToken);

                        await Task.Delay(delay, cancellationToken);
                    }
                    channel.Writer.Complete();
                }
                catch (Exception exception) {
                    localException = exception;
                }
                finally {
                    channel.Writer.Complete(localException);
                }
            });
            return channel.Reader;
        }

        // Second apprach. IAsyncEnumerable<T> ... Server Application using Async Streams
        //! requires C# 8.0 or later.
        public async IAsyncEnumerable<int> CounterEnumerable(
            int count,
            int delay,
            [EnumeratorCancellation]
            CancellationToken cancellationToken)
        {
            _tracer.Log($"Run IAsyncEnumerable<int> CounterEnumerable(count: {count}, delay: {delay})");

            for (int i = 0; i < count; i++) {
                // 
                // Check the cancellation token regularly so that the server will stop
                // producing items if the client disconnects.
                cancellationToken.ThrowIfCancellationRequested();

                yield return i; // T instance;

                // Use the cancellationToken in other APIs that accept cancellation
                // tokens so the cancellation can flow down to them.
                await Task.Delay(delay, cancellationToken);
            }
        }
        #endregion

        #region Client-to-server streaming
        // Receiving Streams on the Server

        // first approach, ChannelReader<T>
        public async Task UploadStreamChannel(ChannelReader<string> stream)
        {
            _tracer.Log($"Run UploadStreamChannel(ChannelReader stream: {stream})", true);

            while (await stream.WaitToReadAsync()) {
                while (stream.TryRead(out var item)) {
                    // do something with the stream item
                    _tracer.Log($"From client: {item}", true);
                }
            }
        }

        // Second apporach. IAsyncEnumerable<T>
        //! requires C# 8.0 or later.
        public async Task UploadStreamEnumerable(IAsyncEnumerable<string> stream)
        {
            _tracer.Log($"UploadStreamEnumerable(IAsyncEnumerable stream: {stream})", true);

            await foreach (var item in stream) {
                // do something with the stream item
                _tracer.Log($"From client: {item}", true);
            }
        }
        #endregion
    }
}
