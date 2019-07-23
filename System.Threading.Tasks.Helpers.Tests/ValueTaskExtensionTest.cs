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

namespace System.Threading.Tasks
{
    using static Utilities;
    using static TestUtilities;

#if !NET40
    [TestFixture]
    public sealed class ValueTaskExtensionTest
    {
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsValueTaskT()
        {
            async Task<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            ValueTask<int> task = t().AsValueTask();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task AsValueTaskTWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<int> task = t().AsValueTask();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsValueTask()
        {
            async Task t()
            {
                await Task.Delay(500);
            };

            ValueTask<Unit> task = t().AsValueTask();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        [Test]
        public async Task AsValueTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<Unit> task = t().AsValueTask();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromValueTask()
        {
            async ValueTask<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            Task<object> task = t().AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task AsValueTaskObjectFromTask()
        {
            async Task<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            ValueTask<object> task = t().AsValueTaskObject();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        [Test]
        public async Task AsValueTaskObjectFromValueTask()
        {
            async ValueTask<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            ValueTask<object> task = t().AsValueTaskObject();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        //////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromValueTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async ValueTask<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            Task<object> task = t().AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task AsValueTaskObjectFromTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<object> task = t().AsValueTaskObject();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task AsValueTaskObjectFromValueTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async ValueTask<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<object> task = t().AsValueTaskObject();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsTaskUnitFromValueTask()
        {
            async ValueTask<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            Task<Unit> task = t().AsTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        [Test]
        public async Task AsValueTaskUnitFromTask()
        {
            async Task<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            ValueTask<Unit> task = t().AsValueTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        [Test]
        public async Task AsValueTaskUnitFromValueTask()
        {
            async ValueTask<int> t()
            {
                await Task.Delay(500);
                return 123;
            };

            ValueTask<Unit> task = t().AsValueTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        //////////////////////////////////

        [Test]
        public async Task AsTaskUnitFromValueTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async ValueTask<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            Task<Unit> task = t().AsTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task AsValueTaskUnitFromTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<Unit> task = t().AsValueTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        [Test]
        public async Task AsValueTaskUnitFromValueTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async ValueTask<int> t()
            {
                await Task.Delay(500);
                throw ex;
            };

            ValueTask<Unit> task = t().AsValueTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }
    }
#endif
}
