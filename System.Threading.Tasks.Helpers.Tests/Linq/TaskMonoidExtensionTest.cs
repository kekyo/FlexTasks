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
using System.Linq;

namespace System.Threading.Tasks.Linq
{
    using static Utilities;
    using static TestUtilities;

    [TestFixture]
    public sealed class TaskMonoidExtensionTest
    {
        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task Aggregate()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate((a, b) => a + b);

            Assert.AreEqual(123 + 456 + 789, r);
        }

        [Test]
        public async Task AggregateByBind()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(async (a, b) =>
                {
                    await Delay(100);
                    return a + b;
                });

            Assert.AreEqual(123 + 456 + 789, r);
        }

        [Test]
        public async Task AggregateWithSeed()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100.0, (a, b) => a + b);

            Assert.AreEqual(100.0 + 123 + 456 + 789, r);
        }

        [Test]
        public async Task AggregateWithSeedByBind()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100.0, async (a, b) =>
                {
                    await Delay(100);
                    return a + b;
                });

            Assert.AreEqual(100.0 + 123 + 456 + 789, r);
        }

        [Test]
        public async Task AggregateWithSeedFinalizeMapper()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100, (a, b) => a + b, rt => rt + 1000.0);

            Assert.AreEqual(100 + 123 + 456 + 789 + 1000.0, r);
        }

        [Test]
        public async Task AggregateWithSeedByBindFinalizeMapper()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100, async (a, b) =>
                {
                    await Delay(100);
                    return a + b;
                },
                rt => rt + 1000.0);

            Assert.AreEqual(100 + 123 + 456 + 789 + 1000.0, r);
        }

        [Test]
        public async Task AggregateWithSeedFinalizeBind()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100, (a, b) => a + b,
                async rt =>
                {
                    await Delay(100);
                    return rt + 1000.0;
                });

            Assert.AreEqual(100 + 123 + 456 + 789 + 1000.0, r);
        }

        [Test]
        public async Task AggregateWithSeedByBindFinalizeBind()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Aggregate(100, async (a, b) =>
                {
                    await Delay(100);
                    return a + b;
                },
                async rt =>
                {
                    await Delay(100);
                    return rt + 1000.0;
                });

            Assert.AreEqual(100 + 123 + 456 + 789 + 1000.0, r);
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ToEnumerableFromArray()
        {
            var numbers = new[] { 123, 456, 789 };
            var rst = FromResult(numbers).
                ToEnumerable();

            foreach (var entry in rst.Zip(numbers, (tr, e) => Tuple.Create(tr, e)))
            {
                var r = await entry.Item1;
                Assert.AreEqual(entry.Item2, r);
            }
        }

        [Test]
        public async Task ToEnumerableFromIEnumerable()
        {
            var numbers = new[] { 123, 456, 789 }.AsEnumerable();
            var rst = FromResult(numbers).
                ToEnumerable();

            foreach (var entry in rst.Zip(numbers, (tr, e) => Tuple.Create(tr, e)))
            {
                var r = await entry.Item1;
                Assert.AreEqual(entry.Item2, r);
            }
        }

        //////////////////////////////////////////////////////////////////

        [Test]
        public async Task ToTaskFromArray()
        {
            var numbers = new[] { FromResult(123), FromResult(456), FromResult(789) };
            var trs = numbers.
                ToTask();

            foreach (var entry in (await trs).Zip(numbers, (r, te) => Tuple.Create(r, te)))
            {
                var e = await entry.Item2;
                Assert.AreEqual(e, entry.Item1);
            }
        }

        [Test]
        public async Task ToTaskFromIEnumerable()
        {
            var numbers = new[] { FromResult(123), FromResult(456), FromResult(789) }.AsEnumerable();
            var trs = numbers.
                ToTask();

            foreach (var entry in (await trs).Zip(numbers, (r, te) => Tuple.Create(r, te)))
            {
                var e = await entry.Item2;
                Assert.AreEqual(e, entry.Item1);
            }
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

            var r = await WhenAll(new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Select(value => value.ToString()));

            Assert.IsTrue(r.SequenceEqual(new[] { "123", "456", "789" }));
        }

        [Test]
        public async Task SelectWithAsyncMapper()
        {
            async Task<int> funcAsync(int delay, int result)
            {
                await Delay(delay);
                return result;
            }

            var r = await WhenAll(new[] { funcAsync(300, 123), funcAsync(500, 456), funcAsync(700, 789) }.
                Select(value => FromResult(value.ToString())));

            Assert.IsTrue(r.SequenceEqual(new[] { "123", "456", "789" }));
        }
    }
}
