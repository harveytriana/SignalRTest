﻿// ===============================
// VISIONARY S.A.S.
// ===============================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

//! SOURCES
// https://docs.microsoft.com/en-us/aspnet/core/signalr/streaming?view=aspnetcore-3.1
// https://csharp.christiannagel.com/2019/10/08/signalrstreaming/

namespace SignalRTest.Hubs
{
    public class StreamHub : Hub
    {
        #region Server-to-client streaming
        // hub method becomes a streaming hub method when it returns IAsyncEnumerable<T>, ChannelReader<T>
        // or async versions
        // first approach, ChannelReader<T>
        //! 2020 - can? public async Task<ChannelReader<int>> Counter1(
        public ChannelReader<int> Counter1(
            int count,
            int delay,
            CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<int>();

            // We don't want to await WriteItemsAsync, otherwise we'd end up waiting 
            // for all the items to be written before returning the channel back to
            // the client.
            //- _ = WriteItemsAsync(channel.Writer, count, delay, cancellationToken);
            //+ i prefer a single block

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
        public async IAsyncEnumerable<int> Counter2(
            int count,
            int delay,
            [EnumeratorCancellation]
            CancellationToken cancellationToken)
        {
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
        public async Task UploadStream(ChannelReader<string> stream)
        {
            Trace.WriteLine($"UploadStream {stream}");

            while (await stream.WaitToReadAsync()) {
                while (stream.TryRead(out var item)) {
                    // do something with the stream item
                    Trace.WriteLine($"From client: {item}");
                }
            }
        }

        // Second apporach. IAsyncEnumerable<T>
        //! requires C# 8.0 or later.
        public async Task UploadStream2(IAsyncEnumerable<string> stream)
        {
            await foreach (var item in stream) {
                Console.WriteLine(item);
            }
        }
        #endregion
    }
}