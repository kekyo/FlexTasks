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

    [TestFixture]
    public sealed class TaskExtensionTest
    {
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromTask()
        {
            async Task<int> t()
            {
                await Delay(500);
                return 123;
            };

            Task<object> task = t().AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        //////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Delay(500);
                throw ex;
            };

            Task<object> task = t().AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromNonGenericTypedTask()
        {
            async Task<int> t()
            {
                await Delay(500);
                return 123;
            };

            Task<object> task = ((Task)t()).AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(123, r);
        }

        //////////////////////////////////

        [Test]
        public async Task AsTaskObjectFromNonGenericTypedTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Delay(500);
                throw ex;
            };

            Task<object> task = ((Task)t()).AsTaskObject();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task AsTaskUnitFromTask()
        {
            async Task<int> t()
            {
                await Delay(500);
                return 123;
            };

            Task<Unit> task = t().AsTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            var r = await task;

            Assert.AreEqual(Unit.Value, r);
        }

        //////////////////////////////////

        [Test]
        public async Task AsTaskUnitFromTaskWithRaise()
        {
            var ex = new InvalidOperationException();

            async Task<int> t()
            {
                await Delay(500);
                throw ex;
            };

            Task<Unit> task = t().AsTaskUnit();
            Assert.IsFalse(task.IsCompleted);

            await AssertCaughtAsync(ex, task);
        }
    }
}
