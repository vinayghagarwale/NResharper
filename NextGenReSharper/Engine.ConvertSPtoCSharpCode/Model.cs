using System;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class Model : ICreater
    {
        private readonly IntermediateModel _intermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;

        public Model(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _intermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;
        }
        public string CreateClass()
        {
            string sModelName = _intermediateModel.BLClassName + "Model";
            string sModelPrivateName = "_" + _intermediateModel.BLClassName + "Model";
            string sModel = "";
            int iTabCount = 0;
            bool CommentStarted = false;

            sModel = sModel + "//**********************************************************";
            sModel = sModel + "\r" + "//    This Model class has been created by Nextgen ReSharper (NG RE#)";
            sModel = sModel + "\r" + "//    Created on : " + DateTime.Now;
            sModel = sModel + "\r" + "//    Source Stored Procedure Name : " + _intermediateModel.BLClassName + ".SQL";
            sModel = sModel + "\r" + "//**********************************************************";


            _logger.Log("Creating Model started");

            if (_rulesModel.AddNamespace)
            {
                sModel = sModel + "\r" + "namespace " + _intermediateModel.configModel.NameSpacePrefix + _intermediateModel.BLClassName;
                sModel = sModel + "\r" + "{";
                iTabCount = 1;
            }

            //Create Model for the SP parameters
            sModel = sModel + "\r" + Helper.NoOfTab(iTabCount) + "public class " + sModelName;
            sModel = sModel + "\r" + Helper.NoOfTab(iTabCount) + "{";

            iTabCount++;
            //Extract Parameter from Stored Procedure
            foreach (var line in _intermediateModel.lslLineDetail)
            {
                if (string.IsNullOrEmpty(line.linetext.Trim()))
                {
                    _logger.CodeStatistics.SPInterpretedCodeCount++;
                    _logger.CodeStatistics.SPEmptyLineCount++;
                    continue;
                }

                //If line reaches AS keyword then exit
                if (line.linetext == "AS")
                {
                    _logger.CodeStatistics.SPInterpretedCodeCount++;
                    break;
                }

                if (line.SPLineType == SPlinetype.CommentStart) CommentStarted = true;
                if (line.SPLineType == SPlinetype.CommentEnd) CommentStarted = false;

                //If line is parameter then process it
                if (line.SPLineType == SPlinetype.Paramameters)
                {
                    if (CommentStarted == false)
                    {
                        line.linetext = line.linetext.Replace('\t', ' ');
                        sModel = sModel + "\r" + Helper.NoOfTab(iTabCount) + "public " + Helper.GetDatatype(line.linetext.Trim()) + " " + line.linetext.Trim().Split(' ')[0].Substring(1) + "{ get; set; }";
                        _logger.CodeStatistics.SPParameterCount++;
                        _logger.CodeStatistics.SPInterpretedCodeCount++;
                        _logger.Log("Creating Model Member");
                    }
                }
            }
            //Close Model
            iTabCount--;

            sModel = sModel + "\r" + Helper.NoOfTab(iTabCount) + "}";

            if (_rulesModel.AddNamespace)
            {
                sModel = sModel + "\r" + "}";
            }

            _logger.Log("Creating Model Completed");
            
            return sModel;
        }
    }
}
