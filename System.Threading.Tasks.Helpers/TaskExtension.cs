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

namespace System.Threading.Tasks
{
    public static partial class TaskExtension
    {
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Task<object> AsTaskObject(this Task task) =>
            Utilities.Cast<object>(task);

#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Task<object> AsTaskObject<T>(this Task<T> task) =>
            Utilities.Cast<T, object>(task);

        public static async Task<Unit> AsTaskUnit<T>(this Task<T> task)
        {
            await task.ConfigureAwait(false);
            return Unit.Value;
        }

        public static async Task<Unit> ContinueWith(this Task task, TaskCompletionSource<Unit> tcs)
        {
            try
            {
                await task.ConfigureAwait(false);
                tcs.TrySetResult(Unit.Value);
            }
            catch (TaskCanceledException)
            {
                tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return Unit.Value;
        }

        public static async Task<Unit> ContinueWith<T>(this Task<T> task, TaskCompletionSource<T> tcs)
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.TrySetResult(result);
            }
            catch (TaskCanceledException)
            {
                tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return Unit.Value;
        }

        public static async Task<Unit> ContinueWith<T, U>(this Task<T> task, TaskCompletionSource<U> tcs, Func<T, U> mapper)
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.TrySetResult(mapper(result));
            }
            catch (TaskCanceledException)
            {
                tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return Unit.Value;
        }

        public static async Task<Unit> ContinueWith<T, U>(this Task<T> task, TaskCompletionSource<U> tcs, Func<T, Task<U>> binder)
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.TrySetResult(await binder(result));
            }
            catch (TaskCanceledException)
            {
                tcs.TrySetCanceled();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return Unit.Value;
        }
    }
}
