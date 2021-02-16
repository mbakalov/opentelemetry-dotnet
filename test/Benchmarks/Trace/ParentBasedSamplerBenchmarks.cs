// <copyright file="ParentBasedSamplerBenchmarks.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using OpenTelemetry.Trace;

namespace Benchmarks.Trace
{
    public class ParentBasedSamplerBenchmarks
    {
        private ParentBasedSampler sampler;

        private SamplingParameters remoteParentSampledParams;
        private SamplingParameters remoteParentNotSampledParams;
        private SamplingParameters localParentSampledParams;
        private SamplingParameters localParentNotSampledParams;

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.remoteParentSampledParams = new SamplingParameters(
                parentContext: new ActivityContext(
                    ActivityTraceId.CreateRandom(),
                    ActivitySpanId.CreateRandom(),
                    ActivityTraceFlags.Recorded,
                    null,
                    true),
                traceId: default,
                name: "Span",
                kind: ActivityKind.Client);
            this.remoteParentNotSampledParams = new SamplingParameters(
                parentContext: new ActivityContext(
                    ActivityTraceId.CreateRandom(),
                    ActivitySpanId.CreateRandom(),
                    ActivityTraceFlags.None,
                    null,
                    true),
                traceId: default,
                name: "Span",
                kind: ActivityKind.Client);
            this.localParentSampledParams = new SamplingParameters(
                parentContext: new ActivityContext(
                    ActivityTraceId.CreateRandom(),
                    ActivitySpanId.CreateRandom(),
                    ActivityTraceFlags.Recorded,
                    null,
                    false),
                traceId: default,
                name: "Span",
                kind: ActivityKind.Client);
            this.localParentNotSampledParams = new SamplingParameters(
                parentContext: new ActivityContext(
                    ActivityTraceId.CreateRandom(),
                    ActivitySpanId.CreateRandom(),
                    ActivityTraceFlags.None,
                    null,
                    false),
                traceId: default,
                name: "Span",
                kind: ActivityKind.Client);

            this.sampler = new ParentBasedSampler(new AlwaysOnSampler());
        }

        [Benchmark(Baseline = true)]
        public void ShouldSample_Original()
        {
            this.sampler.ShouldSample_Original(this.remoteParentSampledParams);

            // this.sampler.ShouldSample_Original(this.localParentSampledParams);
            // this.sampler.ShouldSample_Original(this.remoteParentNotSampledParams);
            // this.sampler.ShouldSample_Original(this.localParentNotSampledParams);
        }

        [Benchmark]
        public void ShouldSample_Delegating()
        {
            this.sampler.ShouldSample(this.remoteParentSampledParams);

            // this.delegatingSampler.ShouldSample_Delegating(this.localParentSampledParams);
            // this.delegatingSampler.ShouldSample_Delegating(this.remoteParentNotSampledParams);
            // this.delegatingSampler.ShouldSample_Delegating(this.localParentNotSampledParams);
        }
    }
}
