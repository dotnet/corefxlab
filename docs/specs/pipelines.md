# System.IO.Pipelines

* Leverages the primitives ([Span\<T>](span.md) and [Memory\<T>](memory.md)) and
  low-allocation APIs.
* Provides an efficient programming model while freeing the developers from
  having to manage buffers.
* A *pipeline* is like a `Stream` that pushes data to you rather than having you
  pull. One chunk of code feeds data into a pipeline, and another chunk of code
  awaits data to pull from the pipeline.
* When writing to a pipeline, the caller allocates memory from the pipeline
  directly.
* Pipelines formalize transferring ownership of buffers so that the readers can
  flow buffers up the stack without copying.
* Supports moving data without copying.
