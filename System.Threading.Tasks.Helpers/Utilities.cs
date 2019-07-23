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
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal static class Utilities
    {
        private static readonly MethodInfo castMethod =
#if NETSTANDARD1_0
            typeof(Utilities).GetTypeInfo().DeclaredMethods.
#else
            typeof(Utilities).GetMethods().
#endif
            First(m => (m.Name == "Cast") && (m.GetGenericArguments().Length == 2));

        public static Task<T> Cast<T>(Task task)
        {
#if NETSTANDARD1_0
            var argType = task.GetType().GetTypeInfo().GenericTypeArguments[0];
#else
            var argType = task.GetType().GetGenericArguments()[0];
#endif
            var dlg = MakeCastOperator<T>(argType);
            return dlg(task);
        }

        public static async Task<U> Cast<T, U>(Task task) =>
            (U)(object)await ((Task<T>)task).ConfigureAwait(false);

        public static Func<Task, Task<T>> MakeCastOperator<T>(Type from) =>
#if NET40
            (Func<Task, Task<T>>)Delegate.CreateDelegate(typeof(Func<Task, Task<T>>), castMethod.MakeGenericMethod(from, typeof(T)));
#else
            (Func<Task, Task<T>>)castMethod.MakeGenericMethod(from, typeof(T)).CreateDelegate(typeof(Func<Task, Task<T>>));
#endif

#if NET40
        public static Task<T> FromResult<T>(T value) =>
            TaskEx.FromResult(value);

        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) =>
            TaskEx.WhenAll(tasks);
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T> FromResult<T>(T value) =>
            Task.FromResult(value);

        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) =>
            Task.WhenAll(tasks);
#endif

#if NET40 || NET45 || NETSTANDARD1_0
        public static Task<T> FromCanceled<T>(CancellationToken ct)
        {
            var tcs = new TaskCompletionSource<T>();
            ct.Register(() => tcs.SetCanceled());
            return tcs.Task;
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T> FromCanceled<T>(CancellationToken ct) =>
            Task.FromCanceled<T>(ct);
#endif

#if NET40 || NET45 || NETSTANDARD1_0
        public static Task<T> FromException<T>(Exception ex)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(ex);
            return tcs.Task;
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<T> FromException<T>(Exception ex) =>
            Task.FromException<T>(ex);
#endif

        public static IEnumerable<T> Collect<T>(this IEnumerable<T> enumerable, Func<T, Tuple<bool, T>> collector)
        {
            foreach (var value in enumerable)
            {
                var entry = collector(value);
                if (entry.Item1)
                {
                    yield return entry.Item2;
                }
            }
        }
    }
}
