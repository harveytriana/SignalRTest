// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Newtonsoft.Json;
using System;

namespace SignalRTest.Shared
{
    public class Measurement
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        public override string ToString()
        {
            return $"Measurement (Timestamp = {Timestamp:HH:mm:ss}, Value = {Value:0.00000})";
        }
    }
}
