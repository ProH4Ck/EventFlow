// The MIT License (MIT)
//
// Copyright (c) 2015-2018 Rasmus Mikkelsen
// Copyright (c) 2015-2018 eBay Software Foundation
// https://github.com/eventflow/EventFlow
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using EventFlow.Core;
using EventFlow.Sagas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventFlow.Commands
{
    public class CommandMetadata : MetadataContainer, ICommandMetadata
    {
        public static ICommandMetadata Empty { get; } = new CommandMetadata();

        public static ICommandMetadata With(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            return new CommandMetadata(keyValuePairs);
        }

        public static ICommandMetadata With(params KeyValuePair<string, string>[] keyValuePairs)
        {
            return new CommandMetadata(keyValuePairs);
        }

        public static ICommandMetadata With(IDictionary<string, string> keyValuePairs)
        {
            return new CommandMetadata(keyValuePairs);
        }

        [JsonIgnore]
        public ISagaId SagaId
        {
            get => GetMetadataValue(CommandMetadataKeys.SagaId, v => new SagaId(v));
            set => Add(CommandMetadataKeys.SagaId, value.Value);
        }

        public CommandMetadata(IDictionary<string, string> keyValuePairs)
            : base(keyValuePairs)
        {
        }

        public CommandMetadata(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
            : base(keyValuePairs.ToDictionary(kv => kv.Key, kv => kv.Value))
        {
        }

        public CommandMetadata(params KeyValuePair<string, string>[] keyValuePairs)
            : this((IEnumerable<KeyValuePair<string, string>>)keyValuePairs)
        {
        }

        public ICommandMetadata CloneWith(params KeyValuePair<string, string>[] keyValuePairs)
        {
            return CloneWith((IEnumerable<KeyValuePair<string, string>>)keyValuePairs);
        }

        public ICommandMetadata CloneWith(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var metadata = new CommandMetadata(this);
            foreach (var kv in keyValuePairs)
            {
                if (metadata.ContainsKey(kv.Key))
                {
                    throw new ArgumentException($"Key '{kv.Key}' is already present!");
                }
                metadata[kv.Key] = kv.Value;
            }
            return metadata;
        }
    }
}
