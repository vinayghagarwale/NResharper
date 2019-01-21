using System;
using System.Collections.Generic;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{    
    public class DataAccessUnitTest
    {
        private readonly IntermediateModel _intermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;


        string sDalUnitTest = "";
        int iTabCount = 0;

        public DataAccessUnitTest(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _intermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;
        }

        public string CreateClass()
        {
            sDalUnitTest = sDalUnitTest + "//**********************************************************";
            sDalUnitTest = sDalUnitTest + "\r" + "//    This Data Access NUnit Automated Unittest class has been created by Nextgen ReSharper (NG RE#)";
            sDalUnitTest = sDalUnitTest + "\r" + "//    Created on : " + DateTime.Now;
            sDalUnitTest = sDalUnitTest + "\r" + "//    Source Class on : " + _intermediateModel.BLClassName + ".SQL";
            sDalUnitTest = sDalUnitTest + "\r" + "//**********************************************************";

            sDalUnitTest = sDalUnitTest + "\r" + "using NextGen.Data;";
            sDalUnitTest = sDalUnitTest + "\r" + "using NextGen.Core;";
            sDalUnitTest = sDalUnitTest + "\r" + "using System.Data;";
            sDalUnitTest = sDalUnitTest + "\r" + "using System;";
            sDalUnitTest = sDalUnitTest + "\r" + "using System.Collections.Generic;";
            sDalUnitTest = sDalUnitTest + "\r" + "using NUnit.Framework;";

            if (_rulesModel.AddNamespace)
            {
                sDalUnitTest = sDalUnitTest + "\r" + "namespace " + _intermediateModel.BLClassName;
                sDalUnitTest = sDalUnitTest + "\r" + "{";
                iTabCount = 1;
            }
            sDalUnitTest = sDalUnitTest + "\r" + "[TestFixture]";
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "public class " + _intermediateModel.BLClassName + "DataAccessUnitTest";
            iTabCount++;
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "{";
            iTabCount++;
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "//Constructor";
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "public " + _intermediateModel.BLClassName + "DataAccessUnitTest(Inject reference)";
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "{";


            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "}";

            iTabCount++;
            BuildQueryMethod(ref sDalUnitTest, ref iTabCount);


            iTabCount--;
            sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "}";
            return sDalUnitTest;
        }
        public void BuildQueryMethod(ref string sDalUnitTest, ref int _iTabCount)
        {
            foreach (var sql in _intermediateModel.lstSQLQueryModel)
            {
                if (sql != null)
                {
                    string sParameters = "";
                    int iTabCount = _iTabCount + 1;
                    switch (sql.SQLQueryType)
                    {
                        case QueryType.Select:
                            {
                                sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "[Test]";
                                sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "public bool " + sql.SourceMethodName + "Unitest(" + sParameters + ")";
                                sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + @"Assert.IsFalse(result,1 should not be prime);";
                                
                                iTabCount--;
                                sDalUnitTest = sDalUnitTest + "\r" + Helper.NoOfTab(iTabCount) + "}";
                            }
                            break;
                    }
                }
            }
        }
    }
}
