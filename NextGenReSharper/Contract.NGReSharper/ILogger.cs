using NextGen.Models.NGReSharper;

namespace NextGen.Contract.NGReSharper
{
    public interface ILogger
    {
        void Log(string log);

        CodeStatistics CodeStatistics { get; }
    }
}
