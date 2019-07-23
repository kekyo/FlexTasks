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

namespace System.Threading.Tasks.Linq
{
    using static Utilities;
    using static TestUtilities;

    [TestFixture]
    public sealed class TaskExtensionTest
    {
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task Cast()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            int r = await ((Task)funcAsync(100, 123)).
                Cast<int>();

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task CastWithHint()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            IConvertible r = await funcAsync(100, 123).
                Cast<int, IConvertible>();

            Assert.AreEqual(123, r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task Select()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await funcAsync(100, 123).
                Select(v => v.ToString());

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectWithAsyncMapper()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await funcAsync(100, 123).
                Select(v => FromResult(v.ToString()));

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectInQuery()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await from v in funcAsync(100, 123)
                    select v.ToString();

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectWithAsyncMapperInQuery()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await from v in funcAsync(100, 123)
                          select FromResult(v.ToString());

            Assert.AreEqual("123", r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task SelectMany()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await funcAsync(100, 123).
                SelectMany(async v =>
                {
                    await Delay(100);
                    return v.ToString();
                });

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectManyWithMapper()
        {
            async Task<double> funcAsync(int delay, double result)
            {
                await Delay(delay);
                return result;
            }

            var r = await funcAsync(100, 123.456).
                SelectMany(async v =>
                {
                    await Delay(100);
                    return (int)v;
                },
                (v1, v2) =>
                {
                    Assert.AreEqual(123.456, v1);
                    return v2.ToString();
                });

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectManyWithAsyncMapper()
        {
            async Task<double> funcAsync(int delay, double result)
            {
                await Delay(delay);
                return result;
            }

            var r = await funcAsync(100, 123.456).
                SelectMany(async v =>
                {
                    await Delay(100);
                    return (int)v;
                },
                async (v1, v2) =>
                {
                    Assert.AreEqual(123.456, v1);
                    await Delay(100);
                    return v2.ToString();
                });

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task SelectManyInQuery()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await from v1 in funcAsync(100, 123)
                          from v2 in funcAsync(100, 456)
                          from v3 in funcAsync(100, 789)
                          select v1 + v2 + v3;

            Assert.AreEqual(123 + 456 + 789, r);
        }

        [Test]
        public async Task SelectManyWithAsyncMapperInQuery()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await from v1 in funcAsync(100, 123)
                          from v2 in funcAsync(100, 456)
                          from v3 in funcAsync(100, 789)
                          select FromResult(v1 + v2 + v3);

            Assert.AreEqual(123 + 456 + 789, r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ContinueWith()
        {
            var tcs = new TaskCompletionSource<Unit>();

            var _ = Delay(500).ContinueWith(tcs);
            Assert.IsFalse(tcs.Task.IsCompleted);

            await tcs.Task;

            Assert.IsTrue(tcs.Task.IsCompleted);
            Assert.IsTrue(!tcs.Task.IsCanceled && !tcs.Task.IsFaulted);
        }

        [Test]
        public async Task ContinueWithCauseCancel()
        {
            var cts = new CancellationTokenSource();

            var tcs = new TaskCompletionSource<Unit>();

            var _ = Delay(500, cts.Token).ContinueWith(tcs);
            Assert.IsFalse(tcs.Task.IsCompleted);

            cts.Cancel();

            await AssertCaughtAsync<TaskCanceledException>(tcs.Task);
        }

        [Test]
        public async Task ContinueWithCauseException()
        {
            async Task funcAsync(int delay, Exception ex2)
            {
                await Delay(delay);
                throw ex2;
            }

            var ex = new InvalidOperationException();

            var tcs = new TaskCompletionSource<Unit>();

            var _ = funcAsync(500, ex).ContinueWith(tcs);
            Assert.IsFalse(tcs.Task.IsCompleted);

            await AssertCaughtAsync(ex, tcs.Task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ContinueWithRelay()
        {
            var tcs = new TaskCompletionSource<int>();

            var _ = FromResult(123).ContinueWith(tcs);
            Assert.IsTrue(tcs.Task.IsCompleted);

            var r = await tcs.Task;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task ContinueWithRelayCauseCancel()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var tcs = new TaskCompletionSource<int>();

            var _ = FromCanceled<int>(cts.Token).ContinueWith(tcs);
            Assert.IsTrue(tcs.Task.IsCanceled);

            await AssertCaughtAsync<TaskCanceledException>(tcs.Task);
        }

        [Test]
        public async Task ContinueWithRelayCauseException()
        {
            async Task<int> funcAsync(int delay, Exception ex2)
            {
                await Delay(delay);
                throw ex2;
            }

            var ex = new InvalidOperationException();

            var tcs = new TaskCompletionSource<int>();

            var _ = funcAsync(500, ex).ContinueWith(tcs);
            Assert.IsFalse(tcs.Task.IsCompleted);

            await AssertCaughtAsync(ex, tcs.Task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ContinueWithRelayAndMapper()
        {
            var tcs = new TaskCompletionSource<string>();

            var _ = FromResult(123).ContinueWith(tcs, v => v.ToString());
            Assert.IsTrue(tcs.Task.IsCompleted);

            var r = await tcs.Task;

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task ContinueWithRelayCauseCancelAndMapper()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var tcs = new TaskCompletionSource<string>();

            var _ = FromCanceled<int>(cts.Token).ContinueWith(tcs, v => v.ToString());
            Assert.IsTrue(tcs.Task.IsCanceled);

            await AssertCaughtAsync<TaskCanceledException>(tcs.Task);
        }

        [Test]
        public async Task ContinueWithRelayCauseExceptionAndMapper()
        {
            async Task<int> funcAsync(int delay, Exception ex2)
            {
                await Delay(delay);
                throw ex2;
            }

            var ex = new InvalidOperationException();

            var tcs = new TaskCompletionSource<string>();

            var _ = funcAsync(500, ex).ContinueWith(tcs, v => v.ToString());
            Assert.IsFalse(tcs.Task.IsCompleted);

            await AssertCaughtAsync(ex, tcs.Task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ContinueWithRelayAndBinder()
        {
            var tcs = new TaskCompletionSource<string>();

            var _ = FromResult(123).ContinueWith(tcs, v => FromResult(v.ToString()));
            Assert.IsTrue(tcs.Task.IsCompleted);

            var r = await tcs.Task;

            Assert.AreEqual("123", r);
        }

        [Test]
        public async Task ContinueWithRelayCauseCancelAndBinder()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var tcs = new TaskCompletionSource<string>();

            var _ = FromCanceled<int>(cts.Token).ContinueWith(tcs, v => FromResult(v.ToString()));
            Assert.IsTrue(tcs.Task.IsCanceled);

            await AssertCaughtAsync<TaskCanceledException>(tcs.Task);
        }

        [Test]
        public async Task ContinueWithRelayCauseExceptionAndBinder()
        {
            async Task<int> funcAsync(int delay, Exception ex2)
            {
                await Delay(delay);
                throw ex2;
            }

            var ex = new InvalidOperationException();

            var tcs = new TaskCompletionSource<string>();

            var _ = funcAsync(500, ex).ContinueWith(tcs, v => FromResult(v.ToString()));
            Assert.IsFalse(tcs.Task.IsCompleted);

            await AssertCaughtAsync(ex, tcs.Task);
        }
    }
}
