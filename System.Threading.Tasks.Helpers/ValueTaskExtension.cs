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

using System;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
#if !NET40
    partial class TaskExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTask<T> AsValueTask<T>(this Task<T> task) =>
            new ValueTask<T>(task);

        public static async ValueTask<Unit> AsValueTask(this Task task)
        {
            await task.ConfigureAwait(false);
            return Unit.Value;
        }

        public static async Task<object> AsTaskObject<T>(this ValueTask<T> task) =>
            await task.ConfigureAwait(false);

        public static async ValueTask<object> AsValueTaskObject<T>(this Task<T> task) =>
            await task.ConfigureAwait(false);

        public static async ValueTask<object> AsValueTaskObject<T>(this ValueTask<T> task) =>
            await task.ConfigureAwait(false);

        public static async Task<Unit> AsTaskUnit<T>(this ValueTask<T> task)
        {
            await task.ConfigureAwait(false);
            return Unit.Value;
        }

        public static async ValueTask<Unit> AsValueTaskUnit<T>(this Task<T> task)
        {
            await task.ConfigureAwait(false);
            return Unit.Value;
        }

        public static async ValueTask<Unit> AsValueTaskUnit<T>(this ValueTask<T> task)
        {
            await task.ConfigureAwait(false);
            return Unit.Value;
        }
    }
#endif
}
