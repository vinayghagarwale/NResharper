
namespace NextGen.Models.NGReSharper
{
    public class CodeStatistics
    {
        public decimal SPCommentCount { get; set; }
        public decimal SPWhileCount { get; set; }
        public decimal SPIfCount { get; set; }
        public decimal SPParameterCount { get; set; }
        public decimal SPInterpretedCodeCount { get; set; }
        public decimal SPTotalLineCount { get; set; }
        public decimal SPUnInterpretedCodeCount { get; set; }
        public decimal SPLocalVariableCount { get; set; }
        public decimal SPExceptionCount { get; set; }
        public decimal SPPrintCount { get; set; }
        public decimal SPSQLQueryCount { get; set; }
        public decimal SPEmptyLineCount { get; set; }

        public void Clear()
        {
            SPCommentCount = 0;
            SPWhileCount = 0;
            SPIfCount = 0;
            SPParameterCount = 0;
            SPInterpretedCodeCount = 0;
            SPTotalLineCount = 0;
            SPUnInterpretedCodeCount = 0;
            SPLocalVariableCount = 0;
            SPExceptionCount = 0;
            SPPrintCount = 0;
            SPSQLQueryCount = 0;
            SPEmptyLineCount = 0;
        }
    }
}
