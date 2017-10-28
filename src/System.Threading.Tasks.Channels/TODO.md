## CURRENT TODO ITEMS

- *Reduced locking in SingleConsumerUnboundedChannel*: It should be possible to remove the locking done in ReadAsyncCore/GetAwaiterCore
so as to make reads entirely lock-free (right now they're lock-free when data is available, but not when the queue is empty).
- *SingleProducerSingleConsumerUnboundedChannel*: We can clone the code for SCUnboundedChannel to an SPSCUnboundedChannel that's used
when both ChannelOptimizations.SingleReader and ChannelOptimizations.SingleWriter are true in Channel.CreateUnbounded. We should then
be able to remove most of the locking used in the write operation implementations.
- *ChannelOptimizations in CreateBounded*: Right now ChannelOptimizations is minimially used CreateBounded.  AllowSynchronousContinuations is
used, but currently only for the completion tasks and read tasks, not for write tasks. We may also be able to specialize implementations
based on SingleReader/SingleWriter, either individually or in combination. If nothing else, GetAwaiter should be optimizable to use a cached
awaiter when SingleReader.
- *ChannelOptimizations in CreateUnbuffered*: CreateUnbuffered currently doesn't use ChannelOptimizations at all.
- *Continued perf measuring and tweaks*: There's likely a good deal more that can be done to get more throughput.  Currently the
biggest contributor to overheads is locking.
- *More tests*: Code coverage is currently very high, ~99% line coverage and ~98% branch coverage, but we need more concurrency
testing.  For now this can be done with specialized stress tests on specific situations in order to try to force interesting interleavings.
