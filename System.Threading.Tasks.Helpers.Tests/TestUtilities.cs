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

using NUnit.Framework;

namespace System.Threading.Tasks
{
    internal static class TestUtilities
    {
        public static async Task AssertCaughtAsync(Exception expected, Task task)
        {
            try
            {
                await task;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreSame(expected, ex);
            }
        }

        public static async Task AssertCaughtAsync<TExpectedException>(Task task)
            where TExpectedException : Exception
        {
            try
            {
                await task;
                Assert.Fail();
            }
            catch (TExpectedException)
            {
            }
        }

#if NET40
        public static Task Delay(int milliseconds) =>
            TaskEx.Delay(milliseconds);

        public static Task Delay(int milliseconds, CancellationToken ct) =>
            TaskEx.Delay(milliseconds, ct);
#else
        public static Task Delay(int milliseconds) =>
            Task.Delay(milliseconds);

        public static Task Delay(int milliseconds, CancellationToken ct) =>
            Task.Delay(milliseconds, ct);

        public static async Task AssertCaughtAsync<T>(Exception expected, ValueTask<T> task)
        {
            try
            {
                await task;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreSame(expected, ex);
            }
        }

        public static async Task AssertCaughtAsync<TExpectedException, T>(ValueTask<T> task)
            where TExpectedException : Exception
        {
            try
            {
                await task;
                Assert.Fail();
            }
            catch (TExpectedException)
            {
            }
        }
#endif
    }
}
