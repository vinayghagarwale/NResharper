namespace NextGen.Models.NGReSharper
{
    public enum QueryType
    {
        Select,
        Insert,
        Update,
        Delete,
        Create,
        Execute
    }

    public enum ReturnType
    {
        ibool,
        iint,
        istring,
        ivoid,        
    }

    public class SQLQueryModel
    {
        public string SourceMethodName { get; set; }
        public QueryType SQLQueryType { get; set; }
        public string SQLQueryText { get; set; }
        public ReturnType SQLReturnType { get; set; }
    }
}
