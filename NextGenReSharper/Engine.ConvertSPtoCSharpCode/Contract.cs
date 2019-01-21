using System;
using System.Collections.Generic;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class Contract : ICreater
    {
        private readonly IntermediateModel _itermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;

        public Contract(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _itermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;
        }
        public string CreateClass()
        {
            string sBLContractName = "I" + _itermediateModel.BLClassName + "Engine";
            string sDALContractName = "I" + _itermediateModel.BLClassName + "DataAccess";

            int iTabCount = 0;
            string sContract = "";

            sContract = sContract + "//**********************************************************";
            sContract = sContract + "\r" + "//    This Contract interface has been created by Nextgen ReSharper (NG RE#)";
            sContract = sContract + "\r" + "//    Created on : " + DateTime.Now;
            sContract = sContract + "\r" + "//    Source Stored Procedure Name : " + _itermediateModel.BLClassName + ".SQL";
            sContract = sContract + "\r" + "//**********************************************************";

            _logger.Log("Creating Interface started");

            if (_rulesModel.AddNamespace)
            {
                sContract = sContract + "\r" + "namespace " + _itermediateModel.configModel.NameSpacePrefix + _itermediateModel.BLClassName;
                sContract = sContract + "\r" + "{";
                iTabCount = 1;
            }

            //Create Model for the SP parameters
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "interface " + sBLContractName;
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "{";
            iTabCount++;
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "void Process();";
            iTabCount--;
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "}";

            //Create Model for the SP parameters
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "interface " + sDALContractName;
            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "{";

            BuildSqlMethodName(ref sContract, ref iTabCount);

            iTabCount--;

            sContract = sContract + "\r" + Helper.NoOfTab(iTabCount) + "}";

            if (_rulesModel.AddNamespace)
            {
                sContract = sContract + "\r" + "}";
            }

            _logger.Log("Creating Interface Completed");

            return sContract;
        }

        private void BuildSqlMethodName(ref string sDataAccessLogic, ref int iTabCount)
        {
            iTabCount++;
            foreach (var sql in _itermediateModel.lstSQLQueryModel)
            {
                if (sql != null)
                {
                    sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "bool " + sql.SourceMethodName + "(" + ");";
                }
            }
        }

    }
}
