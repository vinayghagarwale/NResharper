using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Windows;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class ConvertModeltoCSharp : IConverter
    {
        private readonly IntermediateModel _intermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;

        private readonly Contract _contract;
        private readonly Model _model;
        private readonly DataAccess _dataAccess;
        private readonly Engine _engine;
        private readonly DataAccessUnitTest _dataAccessUnitTest;

        public ConvertModeltoCSharp(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _intermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;

            _contract = new Contract(_intermediateModel, _rulesModel, _logger);
            _model = new Model(_intermediateModel, _rulesModel, _logger);
            _dataAccess = new DataAccess(_intermediateModel, _rulesModel, _logger);
            _engine = new Engine(_intermediateModel, _rulesModel, _logger);
            _dataAccessUnitTest = new DataAccessUnitTest(_intermediateModel, _rulesModel, _logger);

        }

        #region Properties
        public IntermediateModel ItermediateModel
        {
            get { return _intermediateModel; }
        }


        private string _sBusinessClass;

        public string BusinessClass
        {
            get { return _sBusinessClass; }
            set { _sBusinessClass = value; }
        }

        private string _sDataAccessClass;

        public string DataAccessClass
        {
            get { return _sDataAccessClass; }
            set { _sDataAccessClass = value; }
        }

        private string _sModel;

        public string ModelClass
        {
            get { return _sModel; }
            set { _sModel = value; }
        }
        private string _sContract;

        public string ContractClass
        {
            get { return _sContract; }
            set { _sContract = value; }
        }

        private string _sDataAccessUnitTest;

        public string DataAccessUnitTest
        {
            get { return _sDataAccessUnitTest; }
            set { _sDataAccessUnitTest = value; }
        }

        private string _sEngineUnitTest;
        public string EngineUnitTest
        {
            get { return _sEngineUnitTest; }
            set { _sEngineUnitTest = value; }
        }

        #endregion

        #region Public Methods
        public void Convert()
        {
            AddLogHeader();

            if (_rulesModel.AddModelLogic)
                _sModel = _model.CreateClass();

            if (_rulesModel.AddBusinessLogic)
               _sBusinessClass = _engine.CreateClass();

            if (_rulesModel.AddDataAccessLogic)
                _sDataAccessClass = _dataAccess.CreateClass();

            if (_rulesModel.AddContractLogic)
               _sContract = _contract.CreateClass();

            if (_rulesModel.AddAUTforDALLogic )
                _sDataAccessUnitTest = _dataAccessUnitTest.CreateClass();
            
        }

        #endregion

        #region Private Methods

        private void AddLogHeader()
        {
            _logger.Log("*****************************************");
            _logger.Log("Rules Configured");
            _logger.Log("*****************************************");
            _logger.Log("Convert Type                      : Convert Stored Procedure to C# Code");
            _logger.Log("Add Business Logic                : " + _rulesModel.AddBusinessLogic);
            _logger.Log("Add Data Access Logic             : " + _rulesModel.AddDataAccessLogic);
            _logger.Log("Add Model                         : " + _rulesModel.AddModelLogic);
            _logger.Log("Add Contract                      : " + _rulesModel.AddContractLogic);
            _logger.Log("Add Namespace                     : " + _rulesModel.AddNamespace);
            _logger.Log("Create C# Project                 : " + _rulesModel.CreateCSharpProject);
            _logger.Log("Convert Stored Procedure comments : " + _rulesModel.ConvertSPCommnet);
            _logger.Log("*****************************************");
        }

        #endregion

    }
}
