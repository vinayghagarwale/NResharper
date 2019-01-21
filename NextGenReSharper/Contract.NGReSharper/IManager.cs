using NextGen.Models.NGReSharper;

namespace NextGen.Contract.NGReSharper
{
    public interface IManager
    {
        bool ConvertStoredProcedureToCSharpCode(string sStoreProc, SptoCSRules _SPtoCSrulemodel);
        string EngineClass { get; }

        string DataAccessClass { get; }

        string ModelClass { get; }

        string ContractClass { get; }

        string DALAUTClass { get; }

        string BLAUTClass { get; }        
    }
}
