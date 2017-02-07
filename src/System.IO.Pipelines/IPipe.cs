using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public interface IPipe
    {
        IPipeReader Reader { get; }
        IPipeWriter Writer { get; }
    }
}