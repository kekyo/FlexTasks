﻿// .NET Task helper library - https://github.com/kekyo/System.Threading.Tasks.Helpers
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 4014

namespace System.Threading.Tasks.Linq
{
    partial class TaskExtension
    {
        //////////////////////////////////////////////////////////////////

        public static Task<T> Aggregate<T>(this IEnumerable<Task<T>> tasks, Func<T, T, T> accumulator) =>
            Enumerable.Aggregate(tasks, async (ta, tb) =>
            {
                var ra = await ta.ConfigureAwait(false);
                var rb = await tb.ConfigureAwait(false);
                return accumulator(ra, rb);
            });

        public static Task<T> Aggregate<T>(this IEnumerable<Task<T>> tasks, Func<T, T, Task<T>> accumulator) =>
            Enumerable.Aggregate(tasks, async (ta, tb) =>
            {
                var ra = await ta.ConfigureAwait(false);
                var rb = await tb.ConfigureAwait(false);
                return await accumulator(ra, rb).ConfigureAwait(false);
            });

        public static Task<U> Aggregate<T, U>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, U> accumulator) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return accumulator(v, r);
            });

        public static Task<U> Aggregate<T, U>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, Task<U>> accumulator) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return await accumulator(v, r).ConfigureAwait(false);
            });

        public static Task<V> Aggregate<T, U, V>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, U> accumulator, Func<U, V> mapper) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return accumulator(v, r);
            }, async tv => mapper(await tv.ConfigureAwait(false)));

        public static Task<V> Aggregate<T, U, V>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, Task<U>> accumulator, Func<U, V> mapper) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return await accumulator(v, r).ConfigureAwait(false);
            }, async tv => mapper(await tv.ConfigureAwait(false)));

        public static Task<V> Aggregate<T, U, V>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, U> accumulator, Func<U, Task<V>> mapper) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return accumulator(v, r);
            }, async tv => await mapper(await tv.ConfigureAwait(false)).ConfigureAwait(false));

        public static Task<V> Aggregate<T, U, V>(this IEnumerable<Task<T>> tasks, U seed, Func<U, T, Task<U>> accumulator, Func<U, Task<V>> mapper) =>
            Enumerable.Aggregate(tasks, Utilities.FromResult(seed), async (tv, t) =>
            {
                var v = await tv.ConfigureAwait(false);
                var r = await t.ConfigureAwait(false);
                return await accumulator(v, r).ConfigureAwait(false);
            }, async tv => await mapper(await tv.ConfigureAwait(false)).ConfigureAwait(false));

        //////////////////////////////////////////////////////////////////

        public static IEnumerable<Task<T>> Cast<T>(this IEnumerable<Task> tasks) =>
            tasks.Select(task => task.Cast<T>());

        //////////////////////////////////////////////////////////////////

        public static IEnumerable<Task<T>> ToEnumerable<T>(this Task<T[]> task)
        {
            var et = new EnumerableTask<T>();
            task.ContinueWith(et.tcs, arr => arr.AsEnumerable());
            return et;
        }

        public static IEnumerable<Task<T>> ToEnumerable<T>(this Task<IEnumerable<T>> task)
        {
            var et = new EnumerableTask<T>();
            task.ContinueWith(et.tcs);
            return et;
        }

        //////////////////////////////////////////////////////////////////

        public static async Task<IEnumerable<T>> ToTask<T>(this IEnumerable<Task<T>> tasks) =>
            await Utilities.WhenAll(tasks);

        //////////////////////////////////////////////////////////////////

        public static IEnumerable<Task<U>> Select<T, U>(this IEnumerable<Task<T>> tasks, Func<T, U> mapper) =>
            tasks.Select(async task => mapper(await task));

        public static IEnumerable<Task<U>> Select<T, U>(this IEnumerable<Task<T>> tasks, Func<T, Task<U>> mapper) =>
            tasks.Select(async task => await mapper(await task));
    }
}