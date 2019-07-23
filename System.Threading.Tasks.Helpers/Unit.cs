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

using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Value = new Unit();

        public override int GetHashCode() =>
            0;
        public override string ToString() =>
            "()";
        public override bool Equals(object obj) =>
            obj is Unit;
        public bool Equals(Unit value) =>
            true;
        bool IEquatable<Unit>.Equals(Unit other) =>
            true;
    }
}
