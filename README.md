
## Description

The idea of this repo is to demonstrate that `Task<T>` is a monad by comparing it to `IEnumerable<T>`.

I am aiming to cover the following:

- Monad's `return` function (aka `Unit`) for `Task<T>` and `IEnumerable<T>`
- Monad's `bind` function (aka `>>=` aka `SelectMany` aka `FlatMap`/`flatMap`) for `Task<T>` and `IEnumerable<T>`
- Functor's `fmap` function (aka `Select`)
- A look at a couple of monad combinators:
    - `sequence` - given a sequence of monads, return a single monad containing a sequence of the values obtained from the sequence of monads.
        - For example, given an `IEnumerable<Task<T>>`, return a `Task<IEnumerable<T>>`.
    - `liftM2` - given two monads, apply a function to the two values obtained from the two monads and return the result inside a monad.
        - For example, given a `Task<int>` and a `Task<string>` and a `Func<int, string, bool>`, return a `Task<bool>`.
    - In C#, we need separate implementations of these monad combinators for `Task<T>` and `IEnumerable<T>` but they are identical apart from the types involved. In Haskell, only a single implementation of the monad combinators is needed because the type system is more powerful (see [Kind (type theory)](https://en.wikipedia.org/wiki/Kind_%28type_theory%29)).
- Use of LINQ query syntax with `Task<T>` instead of `IEnumerable<T>`.

## Links

- [The task monad in C#](https://ruudvanasseldonk.com/2013/05/01/the-task-monad-in-csharp)
- [Processing Sequences of Asynchronous Operations with Tasks](http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx)
- [Implementing Then with Await](http://blogs.msdn.com/b/pfxteam/archive/2012/08/15/implementing-then-with-await.aspx)
