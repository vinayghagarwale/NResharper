using System.Collections.Generic;

namespace NextGen.Models.NGReSharper
{
    public enum linetype
    {
        Class,
        Directive,
        Constructor,
        Desctructor,
        ClassBegin,
        ClassEnd,
        Method,
        MethodBegin,
        MethodEnd,
        Property,
        PropertyBegin,
        PropertyEnd,
        ClassMembers,
        Localvariable,
        TransactionType,
        TransactionTypeBegin,
        TransactionTypeend,
        SQLquery,
        Other,
        Comment
    }
    public enum SPlinetype
    {
        SPName,
        Paramameters,
        LocalVariable,
        IFStatement,
        While,
        Exception,
        SQLquery,
        Other,
        Print,
        return1,
        Comment,
        CommentStart,
        CommentEnd,
        SET,
        ELSE,
        GO,
        USE,
        RAISERROR,
        END,
        BEGIN,
        EmptyLine
    }

    public enum StatementType
    {
        IF,
        WHILE,
        FOR,
    }
    public class LineDetail
    {
        public string linetext { get; set; }
        public linetype LineType { get; set; }
        public SPlinetype SPLineType { get; set; }
        public TransType transType { get; set; }
        public List<string> TransList { get; set; }

        public long linenumber;

    }
    public class TransType
    {
        public string MethodName
        {
            get
            {
                    return "NewMethodCreated";

            }
        }
        public string Trans { get; set; }
        public string Table { get; set; }
        public string Where { get; set; }
        public string Columns { get; set; }
        public string OrderedBy { get; set; }
        public string Sql { get; set; }
        public List <Parameter> ParameterList  { get; set; }


    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DbDataTypes Datatype { get; set; }

        public ParameterDirection Direction { get; set; }
    }

    //
    // Summary:
    //     Specifies the type of a parameter within a query relative to the System.Data.DataSet.
    public enum ParameterDirection
    {
        //
        // Summary:
        //     The parameter is an input parameter.
        Input = 1,
        //
        // Summary:
        //     The parameter is an output parameter.
        Output = 2,
        //
        // Summary:
        //     The parameter is capable of both input and output.
        InputOutput = 3,
        //
        // Summary:
        //     The parameter represents a return value from an operation such as a stored procedure,
        //     built-in function, or user-defined function.
        ReturnValue = 6
    }
    public enum DbDataTypes
    {
        DBNull = 0,
        Bool = 1,
        Char = 2,
        VarChar = 3,
        Int16 = 4,
        Int32 = 5,
        Int64 = 6,
        Numeric = 7,
        UInt16 = 8,
        UInt32 = 9,
        UInt64 = 10,
        Double = 11,
        Guid = 12,
        DateTime = 13,
        LongRaw = 14,
        Blob = 15,
        Text = 16,
        VarBinary = 17,
        NVarChar = 18,
        Time = 19,
        SmallDateTime = 20,
        DateTime2 = 21,
        DateTimeOffset = 22,
        Bit = 23,
        BigInt = 24,
        Binary = 25
    }

    public class EditingTypeModel
    {
        public string EditingTypeName { get; set; }

    }
}
