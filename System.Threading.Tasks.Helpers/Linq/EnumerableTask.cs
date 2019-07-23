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

using System.Collections;
using System.Collections.Generic;

namespace System.Threading.Tasks.Linq
{
    internal sealed class EnumerableTask<T> : IEnumerable<Task<T>>
    {
        internal readonly TaskCompletionSource<IEnumerable<T>> tcs =
            new TaskCompletionSource<IEnumerable<T>>();

        public IEnumerator<Task<T>> GetEnumerator()
        {
            // Will cause blocking
            foreach (var value in tcs.Task.Result)
            {
                yield return Utilities.FromResult(value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }
}
