using System;
using System.Collections.Generic;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class DataAccess : ICreater
    {
        private readonly IntermediateModel _intermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;

        public DataAccess(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _intermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;
        }
        public string CreateClass()
        {
            string sDataAccessLogic = "";
            int iTabCount = 0;

            try
            {

                sDataAccessLogic = sDataAccessLogic + "//**********************************************************";
                sDataAccessLogic = sDataAccessLogic + "\r" + "//    This Data Access class has been created by Nextgen ReSharper (NG RE#)";
                sDataAccessLogic = sDataAccessLogic + "\r" + "//    Created on : " + DateTime.Now;
                sDataAccessLogic = sDataAccessLogic + "\r" + "//    Source Class on : " + _intermediateModel.BLClassName + ".SQL";
                sDataAccessLogic = sDataAccessLogic + "\r" + "//**********************************************************";

                sDataAccessLogic = sDataAccessLogic + "\r" + "using NextGen.Data;";
                sDataAccessLogic = sDataAccessLogic + "\r" + "using NextGen.Core;";
                sDataAccessLogic = sDataAccessLogic + "\r" + "using System.Data;";
                sDataAccessLogic = sDataAccessLogic + "\r" + "using System;";
                sDataAccessLogic = sDataAccessLogic + "\r" + "using System.Collections.Generic;";

                sDataAccessLogic = sDataAccessLogic + "\r" + "//Data Access class ";
                if (_rulesModel.AddNamespace)
                {
                    sDataAccessLogic = sDataAccessLogic + "\r" + "namespace " + _intermediateModel.configModel.NameSpacePrefix + _intermediateModel.BLClassName;
                    sDataAccessLogic = sDataAccessLogic + "\r" + "{";
                    iTabCount = 1;
                }
                if (!_rulesModel.AddContractLogic)
                    sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public class " + _intermediateModel.BLClassName + "DataAccess";
                else
                    sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public class " + _intermediateModel.BLClassName + "DataAccess : " + "I" + _intermediateModel.BLClassName + "DataAccess";

                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                iTabCount++;
                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "private DatabaseClient _dbClient = null;";

                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//Constructor";
                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public " + _intermediateModel.BLClassName + "DataAccess()";
                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";


                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                iTabCount--;


                BuildQueryMethod(ref sDataAccessLogic, ref iTabCount);

                iTabCount--;
                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                
                return sDataAccessLogic;
            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message.ToString());
                _logger.Log(Ex.Message.ToString());
                return "";
            }
        }

        public void BuildQueryMethod(ref string sDataAccessLogic, ref int _iTabCount)
        {
            foreach (var sql in _intermediateModel.lstSQLQueryModel)
            {
                if (sql != null)
                {
                    string sSQL = "";
                    string sParameters = "";
                    int iTabCount = _iTabCount + 1;
                    switch (sql.SQLQueryType)
                    {
                        case QueryType.Select:
                            {
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// Source file code Line # : ";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// </summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <returns></returns>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// ";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public bool " + sql.SourceMethodName + "(" + sParameters + ")";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sSQL = "\"" + sql.SQLQueryText + "\"";

                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "string sql =" + sSQL + ";";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "List<SmartParam> parameters = new List<SmartParam>();";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (_dbClient == null)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (DatabaseClient client = new DatabaseClient(InstanceMgr.ProcessInstance))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (SmartDataReader reader = new SmartDataReader(sql, client, parameters.ToArray()))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (reader.Read())";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return true;";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return false;";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                            }
                            break;
                        case QueryType.Update:
                            {
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// Source file code Line # : ";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// </summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <returns></returns>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// ";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public void " + sql.SourceMethodName + "(" + sParameters + ")";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "try";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sSQL = "\"" + sql.SQLQueryText + "\"";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "string sql =" + sSQL + ";";


                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "List<SmartParam> lstParams = new List<SmartParam>();";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"//Write code to build parameters";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (_dbClient == null)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (DatabaseClient client = new DatabaseClient(InstanceMgr.ProcessInstance))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"client.ExecuteNonQuery(sql.ToString(), lstParams.ToArray());";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "catch (Exception ex)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "throw new Exception(string.Format(\"App failed to store audit log for ePCS logical access change.\"), ex);";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                            }
                            break;

                        case QueryType.Insert:
                            {
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// Source file code Line # : ";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// </summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <returns></returns>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// ";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public void " + sql.SourceMethodName + "(" + sParameters + ")";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "try";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sSQL = "\"" + sql.SQLQueryText + "\"";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "string sql =" + sSQL + ";";


                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "List<SmartParam> lstParams = new List<SmartParam>();";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"//Write code to build parameters";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (_dbClient == null)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (DatabaseClient client = new DatabaseClient(InstanceMgr.ProcessInstance))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"client.ExecuteNonQuery(sql.ToString(), lstParams.ToArray());";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "catch (Exception ex)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "throw new Exception(string.Format(\"App failed to store audit log for ePCS logical access change.\"), ex);";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                            }
                            break;
                        case QueryType.Execute:
                            {
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// Source file code Line # : ";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// </summary>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <returns></returns>";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// ";

                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public bool " + sql.SourceMethodName + "(" + sParameters + ")";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                sSQL = "\"" + sql.SQLQueryText + "\"";

                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "string sql =" + sSQL + ";";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "List<SmartParam> parameters = new List<SmartParam>();";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (_dbClient == null)";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (DatabaseClient client = new DatabaseClient(InstanceMgr.ProcessInstance))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "using (SmartDataReader reader = new SmartDataReader(sql, client, parameters.ToArray()))";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if (reader.Read())";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                                iTabCount++;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return true;";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return false;";
                                iTabCount--;
                                sDataAccessLogic = sDataAccessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

                            }

                            break;
                        case QueryType.Create:


                            break;
                        case QueryType.Delete:


                            break;
                    }
                }
            }
        }
    }
}
