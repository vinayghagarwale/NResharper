using System;

using NextGen.Engine.Converter;
using NextGen.Models.NGReSharper;
using NextGen.Contract.NGReSharper;

namespace NextGen.Manager.NGReSharper
{
    public class NGResharperManager : IManager
    {
        private ConvertStoredProctoModel _convertSPtoIM;
        private ConvertModeltoCSharp _convertIMtoCS;

        private IntermediateModel InterModelGet;
        private readonly ILogger _logger;

        #region Properties



        public string EngineClass
        {
            get { return _convertIMtoCS.BusinessClass; }
        }

        public string DataAccessClass
        {
            get { return _convertIMtoCS.DataAccessClass; }
        }

        public string ModelClass
        {
            get { return _convertIMtoCS.ModelClass; }
        }

        public string ContractClass
        {
            get { return _convertIMtoCS.ContractClass; }
        }

        public string DALAUTClass
        {
            get { return _convertIMtoCS.DataAccessUnitTest; }
        }

        public string BLAUTClass
        {
            get { return _convertIMtoCS.EngineUnitTest; }
        }
        #endregion

        public NGResharperManager(IntermediateModel _InterModelGet, ILogger logger)
        {
            InterModelGet = _InterModelGet;
            _logger = logger;
        }
        public bool ConvertStoredProcedureToCSharpCode(string sStoreProc, SptoCSRules _SPtoCSrulemodel)
        {
            try
            {
                //code to convert Stored Proc to Intermediate Model
                _convertSPtoIM = new ConvertStoredProctoModel(sStoreProc, InterModelGet, _logger);
                _convertSPtoIM.Convert();

                //Code to convert Intermediate Model to C# code
                _convertIMtoCS = new ConvertModeltoCSharp(InterModelGet, _SPtoCSrulemodel, _logger);
                _convertIMtoCS.Convert();

                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
