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
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Threading.Tasks
{
    using static Utilities;
    using static TestUtilities;

#if !NET40
    [TestFixture]
    public sealed class ValueTaskTest
    {
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task CompletedTask()
        {
            ValueTask<Unit> task = ValueTask.CompletedTask;

            Assert.IsTrue(task.IsCompletedSuccessfully);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task FromResult()
        {
            ValueTask<int> task = ValueTask.FromResult(123);

            Assert.IsTrue(task.IsCompletedSuccessfully);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task DelayMillisecond()
        {
            var begin = DateTime.Now;

            await ValueTask.Delay(1000);

            var end = DateTime.Now;

            Assert.IsTrue(200 > Math.Abs((end - begin).TotalMilliseconds - 1000));
        }

        [Test]
        public async Task DelayTimespan()
        {
            var begin = DateTime.Now;

            await ValueTask.Delay(TimeSpan.FromMilliseconds(1000));

            var end = DateTime.Now;

            Assert.IsTrue(200 > Math.Abs((end - begin).TotalMilliseconds - 1000));
        }

        [Test]
        public async Task DelayMillisecondWithCT()
        {
            var cts = new CancellationTokenSource();

            var task = ValueTask.Delay(1000, cts.Token);
            Assert.IsFalse(task.IsCompleted);

            cts.Cancel();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCanceled);

            await AssertCaughtAsync<TaskCanceledException, Unit>(task);
        }

        [Test]
        public async Task DelayTimespanWithCT()
        {
            var cts = new CancellationTokenSource();

            var task = ValueTask.Delay(TimeSpan.FromMilliseconds(1000), cts.Token);
            Assert.IsFalse(task.IsCompleted);

            cts.Cancel();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCanceled);

            await AssertCaughtAsync<TaskCanceledException, Unit>(task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task WhenAll()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(args);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            tcs2.SetResult(456);
            Assert.IsFalse(task.IsCompleted);

            tcs3.SetResult(789);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var rs = await task;

            Assert.IsTrue(rs.SequenceEqual(new[] { 123, 456, 789 }));
        }

        [Test]
        public async Task WhenAllWithRaise1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(args);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs2.SetException(ex);
            Assert.IsFalse(task.IsCompleted);

            tcs3.SetResult(789);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsFalse(task.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task WhenAllWithRaise2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(args);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            var ex1 = new InvalidOperationException();
            tcs2.SetException(ex1);
            Assert.IsFalse(task.IsCompleted);

            var ex2 = new InvalidOperationException();
            tcs3.SetException(ex2);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsFalse(task.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex1, task);
        }

        [Test]
        public async Task WhenAllIEnumerable()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(enumerable);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            tcs2.SetResult(456);
            Assert.IsFalse(task.IsCompleted);

            tcs3.SetResult(789);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var rs = await task;

            Assert.IsTrue(rs.SequenceEqual(new[] { 123, 456, 789 }));
        }

        [Test]
        public async Task WhenAllIEnumerableWithRaise1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(enumerable);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs2.SetException(ex);
            Assert.IsFalse(task.IsCompleted);

            tcs3.SetResult(789);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsFalse(task.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task WhenAllIEnumerableWithRaise2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAll(enumerable);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsFalse(task.IsCompleted);

            var ex1 = new InvalidOperationException();
            tcs2.SetException(ex1);
            Assert.IsFalse(task.IsCompleted);

            var ex2 = new InvalidOperationException();
            tcs3.SetException(ex2);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsFalse(task.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex1, task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task WhenAny1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(args);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsTrue(vt.IsCompletedSuccessfully);

            var r = await vt;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task WhenAny2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(args);
            Assert.IsFalse(task.IsCompleted);

            tcs2.SetResult(456);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsTrue(vt.IsCompletedSuccessfully);

            var r = await vt;

            Assert.AreEqual(456, r);
        }

        [Test]
        public async Task WhenAnyWithRaise1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(args);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs1.SetException(ex);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsFalse(vt.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, vt);
        }

        [Test]
        public async Task WhenAnyWithRaise2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            var args = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(args);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs2.SetException(ex);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsFalse(vt.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, vt);
        }

        [Test]
        public async Task WhenAnyIEnumerable1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(enumerable);
            Assert.IsFalse(task.IsCompleted);

            tcs1.SetResult(123);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsTrue(vt.IsCompletedSuccessfully);

            var r = await vt;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task WhenAnyIEnumerable2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(enumerable);
            Assert.IsFalse(task.IsCompleted);

            tcs2.SetResult(456);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsTrue(vt.IsCompletedSuccessfully);

            var r = await vt;

            Assert.AreEqual(456, r);
        }

        [Test]
        public async Task WhenAnyIEnumerableWithRaise1()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(enumerable);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs1.SetException(ex);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsFalse(vt.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, vt);
        }

        [Test]
        public async Task WhenAnyIEnumerableWithRaise2()
        {
            var tcs1 = new TaskCompletionSource<int>();
            var tcs2 = new TaskCompletionSource<int>();
            var tcs3 = new TaskCompletionSource<int>();

            IEnumerable<ValueTask<int>> enumerable = new[] { tcs1.Task.AsValueTask(), tcs2.Task.AsValueTask(), tcs3.Task.AsValueTask() };
            var task = ValueTask.WhenAny(enumerable);
            Assert.IsFalse(task.IsCompleted);

            var ex = new InvalidOperationException();
            tcs2.SetException(ex);
            Assert.IsTrue(task.IsCompleted);
            Assert.IsTrue(task.IsCompletedSuccessfully);

            var vt = await task;
            Assert.IsTrue(vt.IsCompleted);
            Assert.IsFalse(vt.IsCompletedSuccessfully);

            await AssertCaughtAsync(ex, vt);
        }
    }
#endif
}
