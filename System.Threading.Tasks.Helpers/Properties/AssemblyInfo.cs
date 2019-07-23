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

#if NET40
[assembly: InternalsVisibleTo("System.Threading.Tasks.Helpers.Tests.net40")]
#endif

#if NET45
[assembly: InternalsVisibleTo("System.Threading.Tasks.Helpers.Tests.net45")]
#endif

#if NETSTANDARD1_0
[assembly: InternalsVisibleTo("System.Threading.Tasks.Helpers.Tests.netcoreapp1.0")]
#endif

#if NETSTANDARD2_0
[assembly: InternalsVisibleTo("System.Threading.Tasks.Helpers.Tests.netcoreapp2.0")]
#endif
