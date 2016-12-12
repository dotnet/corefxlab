namespace System.IO.Pipelines
{
    public class PipelineConnection : IPipelineConnection
    {
        public PipelineConnection(PipelineFactory factory)
        {
            Input = factory.Create();
            Output = factory.Create();
        }

        IPipelineReader IPipelineConnection.Input => Input;
        IPipelineWriter IPipelineConnection.Output => Output;

        public Pipe Input { get; }

        public Pipe Output { get; }

        public void Dispose()
        {
            Input.CompleteReader();
            Input.CompleteWriter();
            Output.CompleteReader();
            Output.CompleteWriter();
        }
    }
}