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

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Linq
{
    public static partial class TaskExtension
    {
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Task<T> Cast<T>(this Task task) =>
            Utilities.Cast<T>(task);

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Task<U> Cast<T, U>(this Task<T> task) =>
            Utilities.Cast<T, U>(task);

        public static async Task<U> Select<T, U>(this Task<T> task, Func<T, U> mapper) =>
            mapper(await task.ConfigureAwait(false));

        public static async Task<U> Select<T, U>(this Task<T> task, Func<T, Task<U>> mapper) =>
            await mapper(await task.ConfigureAwait(false));

        public static async Task<U> SelectMany<T, U>(this Task<T> task, Func<T, Task<U>> binder) =>
            await binder(await task.ConfigureAwait(false)).ConfigureAwait(false);

        public static async Task<V> SelectMany<T, U, V>(this Task<T> task, Func<T, Task<U>> binder, Func<T, U, V> mapper)
        {
            var result = await task.ConfigureAwait(false);
            var selected = await binder(result).ConfigureAwait(false);
            return mapper(result, selected);
        }

        public static async Task<V> SelectMany<T, U, V>(this Task<T> task, Func<T, Task<U>> binder, Func<T, U, Task<V>> mapper)
        {
            var result = await task.ConfigureAwait(false);
            var selected = await binder(result).ConfigureAwait(false);
            return await mapper(result, selected).ConfigureAwait(false);
        }
    }
}
