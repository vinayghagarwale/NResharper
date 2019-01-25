using NextGen.Models.NGReSharper;

namespace NextGen.Contract.NGReSharper
{
    public interface IManager
    {
        bool ConvertStoredProcedureToCSharpCode(string sStoreProc, SptoCSRules _SPtoCSrulemodel);
        bool ExtractInLineCodeFromVBNetCode(string SourceFilePath);
        string EngineClass { get; }
        string DataAccessClass { get; }
        string ModelClass { get; }
        string ContractClass { get; }
        string DALAUTClass { get; }
        string BLAUTClass { get; }
        string VBNETDataAccessClass { get; set; }
        
    }
}
