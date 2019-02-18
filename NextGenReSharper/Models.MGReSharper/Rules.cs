
namespace NextGen.Models.NGReSharper
{
    public enum ConvertionType
    {
        ConvertSPtoCS,
        CreateDALfromInline,
        ConvertLegacyMethodtoNewMethod
    }
    public class SptoCSRules
    {
        public bool AddBusinessLogic { get; set; }
        public bool AddDataAccessLogic { get; set; }
        public bool AddModelLogic { get; set; }
        public bool AddContractLogic { get; set; }
        public bool AddAUTforDALLogic { get; set; }
        public bool AddAUTforBLLogic { get; set; }

        public bool AddNamespace { get; set; }
        public bool CreateCSharpProject { get; set; }
        public bool ConvertSPCommnet { get; set; }

    }
    public class InlinetoDALRules
    {
        public bool AddDataAccessLogic { get; set; }
    }
}
