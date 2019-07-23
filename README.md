# .NET Task / ValueTask helpers for compositioning frustration free.

Still under construction...

[![NuGet](https://img.shields.io/nuget/v/FlexTasks.Core.svg?style=flat)](https://www.nuget.org/packages/FlexTasks.Core)

[![Azure pipelines](https://kekyo.visualstudio.com/System.Threading.Tasks.Helpers/_apis/build/status/System.Threading.Tasks.Helpers-master) ![Azure pipelines tests](https://img.shields.io/azure-devops/tests/kekyo/System.Threading.Tasks.Helpers/2.svg)](https://kekyo.visualstudio.com/System.Threading.Tasks.Helpers/_build?definitionId=2)

## What's this?

.NET Task and ValueTask types lack for primitive operators.

For example, Are you frustrating lacks set of complementary operators between Task and ValueTask types? And found lacking very primitive but useful operators?

* It's my hobby project, issue and PRs are welcome :)

## Design goals are

* Implements complementary sets of Task and ValueTask operators.
* Make stable with Unit type instead of void type.
* Make easier naturally expression in Linq term/expressions.

## Examples

### ValueTask.FromResult()

```csharp
// Make absolutely result value
var task = Task.FromResult(123);
//var valueTask = ValueTask.FromResult(123);   // lack for FromResult method.
var valueTask = new ValueTask(123);

// Helper solution:
var valueTask = ValueTask.FromResult(123);
```

### Convert to Task<object> / ValueTask<object>

```csharp
// Map real T to object
Task<int> task = Task.FromResult(123);
//Task<object> taskObject = task;    // cannot implicitly cast, because Task<T> isn't covariant at generic argument.
Task<object> taskObject = Task.FromResult(await Task.FromResult(123));

// Helper solution:
Task<object> taskObject = Task.FromResult(123).AsTaskObject();
```

```csharp
// Standard operator for converting to Task.
Task<int> task = ValueTask.FromResult(123).AsTask();

// Reverse operator:
ValueTask<int> valueTask = Task.FromResult(123).AsValueTask();
```

### Mapping from closed type Task/ValueTask to another type.

* Another way for conversion:

```csharp
// LINQ select:
Task<object> taskObject = Task.FromResult(123).Select(value => (object)value);
Task<string> taskString = Task.FromResult(123).Select(value => value.ToString());

// LINQ cast:
Task<IConvertible> taskConvertible = Task.FromResult(123).Cast<IConvertible>();
```

```csharp
// LINQ select (bind) operators:
Task<string> taskString = Task.FromResult(123).SelectMany(value => Task.FromResult(value));

// Same as:
// (Because supports easier usage for LINQ query expressions)
Task<string> taskString = Task.FromResult(123).Select(value => Task.FromResult(value));
```

### Swap between outer and argument types

```csharp
// From IEnumerable<Task<int>> to Task<IEnumerable<int>>
Task<IEnumerable<int>> taskSequence =
    new[] { Task.FromResult(123), Task.FromResult(456), Task.FromResult(789) }.
    ToTask();

// From Task<IEnumerable<int>> to IEnumerable<Task<int>>
IEnumerable<Task<int>> sequenceTasks = Task.FromResult(
    new[] { 123, 456, 789 }).
    ToEnumerable();
```

### Unit type

* System.Unit type is empty value type using instead of void type.

```csharp
// Cannot make non-generic (empty arguments) ValueTask instance.
Task task = Task.CompletedTask;
//ValueTask valueTask = ValueTask.CompletedTask;           // Non generic version ValueType didn't define.
//ValueTask<void> valueTask = ValueTask.CompletedTask;     // And cannot apply generic type with void.

// Helper solution:
ValueTask<Unit> valueTask = ValueTask.CompletedTask;
```

```csharp
// Async operation with non informational result.
static async Task FooAsync()
{
    await Bar.BazAsync();
}

// Complementary usage set of Unit.
static async ValueTask<Unit> FooAsync()
{
    await Bar.BazAsync();
    return Unit.Value;
}

// Both are correct:
static async Task<Unit> FooAsync()
{
    await Bar.BazAsync();
    return Unit.Value;
}
```

```csharp
// Task type narrowing casts are correct.
Task task = Task.FromResult(123);

// ValueTask type can't conversion:
//ValueTask<Unit> valueTask = ValueTask.FromResult(123);

// Make with operator (or we can use LINQ select operator.)
ValueTask<Unit> valueTask = ValueTask.FromResult(123).AsValueTaskUnit();

// Both are correct:
Task<Unit> task = Task.FromResult(123).AsTaskUnit();
```

### LINQ query expressions

```csharp
// Make bind operation using LINQ query:
int result1 = await from value1 in Task.FromResult(123)
                    from value2 in Task.FromResult(456)
                    from value3 in Task.FromResult(789)
                    select value1 + value2 + value3;

// Both are correct:
int result2 = await from value1 in Task.FromResult(123)
                    from value2 in Task.FromResult(456)
                    from value3 in Task.FromResult(789)
                    select Task.FromResult(value1 + value2 + value3);
```

### LINQ monoid operations

```csharp
// LINQ with Task sequence types
Task<int>[] source = new[] { Task.FromResult(123), Task.FromResult(456), Task.FromResult(789) };
// int result = await source.Aggregate((a, b) => a + b);      // too complex infer types and difficult for correcting.

// Helper solution:
int result = await source.Aggregate((a, b) => a + b);                     // can make naturally expression.
int result = await source.Aggregate((a, b) => Task.FromResult(a + b));    // stable expression if use async continuation.
```

## Licence

Under Apache v2.

## Limitation

The net40 target doesn't contain ValueTask infrastructures.
