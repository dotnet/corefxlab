# High-Performance Data Pipelines

Our goal is to optimize .NET much more for scenarios in which inefficiences are
directly tied to your monthly billing. With ASP.NET Core, we've already
[improved significantly][TechEmpower13] and are now in the top 10 for the plain
text benchmark. But we believe there is still a lot more potential that we could
tap into.

![](./img/techempower.png)

## Roadmap

For the next six to twelve months our primary area of focus for CoreFxLab will be
to improve performance for data pipeline oriented apps. This is pretty much any
cloud app as the request-response pattern is fundamentally a data pipeline.

Consider an example for a typical web request:

![](./img/pipeline.png)

Most cloud apps are parellelized by running multiple requests at the same time
while each requests is often executed as a single chain. This results in the
picture above where all components are daisy-chained. That means slowing down
one component will slow down the entire request.

An important metric for a cloud app is how many requests per second (RPS) it can
handle. That's important because the load is (usually) outside the control of
the app author. So the fewer requests your app can handle, the more instances of
your app you need in order to satisfy the demand, which basically means the more
machines you need to pay for.

Also, consider the role of the framework. For the most part, your app code is
represented by the green box above, while the blue and red parts are usually
components provided by the framework. While you can optimize your app code, your
ability to reduce the overhead in the framework provided pieces is limited.

That's why many people rely on benchmarks to assess the potential of a given web
framework. It's important to keep in mind that benchmarks are by definition
gross simplifications of real-world workloads; but they are often considered to
be good at providing an indicator for the theoretical best a given framework can
do for you if you remove virtually all overhead that is specific to your app.

A widely referred to benchmark is [TechEmpower]. Here is how they
[describe][TechEmpower-Quote] why performance considerations for frameworks are
important:

> Application performance can be directly mapped to hosting dollars, and for
> companies both large and small, hosting costs can be a pain point.
>
> What if building an application on one framework meant that at the very best
> your hardware is suitable for one tenth as much load as it would be had you
> chosen a different framework?
>
> --- [TechEmpower][TechEmpower-Quote]

## What does high-performance mean?

It might not be the best term, but for the context of cloud apps the property
we're seeking to improve is scalability:

> Scalability is the capability of a system, network, or process to handle a
> growing amount of work.
>
> --- [Wikipedia](https://en.wikipedia.org/wiki/Scalability)

Many areas affect scale but an efficient request/response pipeline is key as
it's the backbone for all cloud solutions.

Other investments (faster GC, better JIT, AOT) aren't a replacement but will
provide additional benefits.

## Current areas of concerns

If we look at the .NET Stack, in particular the BCL, there are a few areas where
we could do much better:

1. Many primitive operations require allocations, causing GC pressure
2. `String` is UTF16 but networking is UTF8, forcing translations
3. Buffers are often defensively copied, slowing down operations and increasing
   allocations
4. Buffers are often not pooled, causing fragmentation and GC pressure
5. Interop with native code often creates additional buffers to avoid passing
   around raw pointers
6. Async streaming forces pre-allocation of buffers, causing excessive memory
   usage
7. ...

Our goal is to reduce the number of allocations for the basic operations, such
as parsing and encoding, having a more efficient buffer management that can
handle managed and native memory uniformly, and providing a programming model
that makes the result easy to use while not losing efficiency.

Other components of the .NET stack (such as MVC and Serializtion) will rewire
their implementation in order to take advantage of the efficiency gains provided
by these new APIs.

![](./img/areas.png)

## New APIs

Not all components of this stack are fully designed yet. The key components we
have spiked so far are:

* **Primitives**
  - [Span\<T>][span-speclet]
      + Represents contiguous regions of arbitrary memory (managed & native)
      + Performance on par with `T[]`, understood by runtime and code gen
      + Can only live on the stack
  - [Memory\<T>][memory-speclet]
      + Provides longer lifetime semantics to a `Span<T>`
      + Allows storing a reference to a `Span<T>` on the heap
* **Low-Alloc Transforms**
  - [Parsing][parsing-speclet]
  - [Formatting][formatting-speclet]
* **[Pipelines][pipelines-speclet]**
  - Leverages the primitives (`Span<T>` and `Memory<T>`) and low-allocation APIs
  - Provides an efficient programming model while freeing the developers from
    having to manage buffers

[TechEmpower]: https://www.techempower.com/benchmarks
[TechEmpower-Quote]: https://www.techempower.com/benchmarks/#section=motivation
[TechEmpower13]: https://www.techempower.com/blog/2016/11/16/framework-benchmarks-round-13/
[span-speclet]: ./specs/span.md
[memory-speclet]: ./specs/memory.md
[pipelines-speclet]: ./specs/pipelines.md
[parsing-speclet]: ./specs/parsing.md
[formatting-speclet]: ./specs/formatting.md
