// .NET Task helper library - https://github.com/kekyo/System.Threading.Tasks.Helpers
// Copyright (c) 2019 Kouji Matsui
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
#if !NET40
    public static class ValueTask
    {
        public static ValueTask<Unit> CompletedTask
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ValueTask<Unit>(Unit.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<T> FromResult<T>(T value) =>
            new ValueTask<T>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<Unit> Delay(int millisecondDelay) =>
            Task.Delay(millisecondDelay).AsValueTask();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<Unit> Delay(TimeSpan delay) =>
            Task.Delay(delay).AsValueTask();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<Unit> Delay(int millisecondDelay, CancellationToken ct) =>
            Task.Delay(millisecondDelay, ct).AsValueTask();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<Unit> Delay(TimeSpan delay, CancellationToken ct) =>
            Task.Delay(delay, ct).AsValueTask();

        public static ValueTask<T[]> WhenAll<T>(IEnumerable<ValueTask<T>> tasks) =>
            Task.WhenAll(tasks.Select(task => task.AsTask())).AsValueTask();

        public static ValueTask<T[]> WhenAll<T>(params ValueTask<T>[] tasks) =>
            Task.WhenAll(tasks.Select(task => task.AsTask())).AsValueTask();

        public static async ValueTask<ValueTask<T>> WhenAny<T>(IEnumerable<ValueTask<T>> tasks)
        {
            var result = await Task.WhenAny(tasks.Select(t => t.AsTask()));
            return result.AsValueTask();
        }

        public static async ValueTask<ValueTask<T>> WhenAny<T>(params ValueTask<T>[] tasks)
        {
            var result = await Task.WhenAny(tasks.Select(t => t.AsTask()));
            return result.AsValueTask();
        }
    }
#endif
}
