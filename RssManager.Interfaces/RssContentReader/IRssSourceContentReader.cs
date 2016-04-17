
namespace RssManager.Interfaces.RssContentReader
{
    public interface IRssSourceContentReader
    {
        string Uri { get; /*set;*/ }
        string Read();
    }
}
