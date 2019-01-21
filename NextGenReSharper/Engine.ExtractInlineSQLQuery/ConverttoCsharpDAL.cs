using System;
using NextGen.Models.NGReSharper;
using System.IO;
using System.Diagnostics;
namespace NextGen.Engine.ExtractInlineSQLQuery
{
    public class ProcessConverttoCsharpDAL
    {
        IntermediateModel _itermediateModel;
        string strDAL;
        public ProcessConverttoCsharpDAL(IntermediateModel intermediateModel)
        {
            _itermediateModel = intermediateModel;
        }

        public IntermediateModel intermediateModel { get { return _itermediateModel; } }
        public string Convert()
        {
            strDAL = strDAL + "//This class has been created by Nextgen Code Editor";
            strDAL = strDAL + "\r" + "//Created on : " + DateTime.Now;
            strDAL = strDAL + "\r" + "//Source Class on : " + _itermediateModel.BLClassName + ".vb";
            strDAL = strDAL + "\r" + "using NextGen.Data;";
            strDAL = strDAL + "\r"+ "using NextGen.Core;";
            strDAL = strDAL + "\r" + "using System.Data;";
            strDAL = strDAL + "\r" + "using System.Collections.Generic;";
            
            strDAL = strDAL + "\r\t" + "//Data Access class ";

            strDAL = strDAL + "\r\t" + "public class " + _itermediateModel.BLClassName + "DAL";

            strDAL = strDAL + "\r" + "{";

            strDAL = strDAL + "\r" + "private DatabaseClient _dbClient = null;";
            
            strDAL = strDAL + "\r" + "//Constructor";
            strDAL = strDAL + "\r" + "public " + _itermediateModel.BLClassName + "DAL()";
            strDAL = strDAL + "\r\t" + "{";


            strDAL = strDAL + "\r\t" + "}";

            int counter = 1;
            foreach (var line in _itermediateModel.lslLineDetail)
            {
                if (line?.transType != null)
                {
                    string sSQL = "";
                    string sParameters = "";
                    strDAL = strDAL + "\r\t" + "/// <summary>";
                    strDAL = strDAL + "\r\t" + "/// Source file code Line # : " + line.linenumber;
                    strDAL = strDAL + "\r\t" + "/// </summary>";
                    strDAL = strDAL + "\r\t" + "/// <returns></returns>";
                    strDAL = strDAL + "\r\t" + "/// ";
                    if (!(line.transType.ParameterList == null))
                    {
                        sParameters = "";
                        for (int i = 0; i <= line.transType.ParameterList.Count - 1; i++)
                        {
                            if (string.IsNullOrEmpty(sParameters))
                            {
                                sParameters = "string " +  line.transType.ParameterList[i].Value;
                            }
                            else
                            {
                                if(sParameters.IndexOf(line.transType.ParameterList[i].Value) < 0)
                                    sParameters = sParameters + ", string "  + line.transType.ParameterList[i].Value;
                            }
                        }
                    }
                    strDAL = strDAL + "\r\t" + "public bool " + line?.transType.MethodName + counter + "(" + sParameters + ")";
                    strDAL = strDAL + "\r\t" + "{";

                    if (line?.transType.Trans != null)
                    {
                        if (line?.transType.Trans != null && (line.transType.Trans.Contains("SELECT") || line.transType.Trans.Contains("select")))
                        {
                            sSQL = "\"Select " + line.transType.Columns + " from " + line.transType.Table + " " + line.transType.Where + " " + line.transType.OrderedBy + "\"";
                        }
                        else if (line.transType.Trans.Contains("insert"))
                        {
                            sSQL = "\"insert " + line.transType.Columns + " from " + line.transType.Table + " " + line.transType.Where + "\"";
                        }
                        else if (line.transType.Trans.Contains("update"))
                        {
                            sSQL = "\"update " + line.transType.Columns + " from " + line.transType.Table + " " + line.transType.Where + "\"";
                        }
                        else
                            sSQL = "\"\"";
                    }
                    else
                        sSQL = "\"\"";

                    strDAL = strDAL + "\r\t\t" + "string sql =" + sSQL + ";";

                    //strDAL = strDAL + "\r\t\t" + "if (string.IsNullOrEmpty(encounterID)) return false;";

                    
                    if (!(line.transType.ParameterList == null))
                    {
                        strDAL = strDAL + "\r\t\t" + "List<SmartParam> parameters = new List<SmartParam>();";
                        for (int i = 0; i <= line.transType.ParameterList.Count-1; i++)
                        {
                            strDAL = strDAL + "\r\t\t" + @"parameters.Add(new SmartParam( " + "\"@" + line.transType.ParameterList[i].Name + "\"" + ", " + line.transType.ParameterList[i].Value + ", " + "DBCommand.DbDataTypes." + line.transType.ParameterList[i].Datatype + "," + "ParameterDirection." + line.transType.ParameterList[i].Direction + ")) ; ";
                        }
                    }
                    strDAL = strDAL + "\r\t\t" + "if (_dbClient == null)";
                    strDAL = strDAL + "\r\t\t" + "{";

                    strDAL = strDAL + "\r\t\t" + "using (DatabaseClient client = new DatabaseClient(InstanceMgr.ProcessInstance))";
                    strDAL = strDAL + "\r\t\t" + "{";
                    strDAL = strDAL + "\r\t\t\t" + "using (SmartDataReader reader = new SmartDataReader(sql, client, parameters.ToArray()))";
                    strDAL = strDAL + "\r\t\t\t" + "{";
                    strDAL = strDAL + "\r\t\t" + "if (reader.Read())";
                    strDAL = strDAL + "\r\t\t" + "{";
                    //strDAL = strDAL + "\r" + @"if (reader[\"enc_status\"].GetChar() == 'Y')";
                    strDAL = strDAL + "\r\t\t" + "return true;";
                    strDAL = strDAL + "\r\t\t" + "}";
                    strDAL = strDAL + "\r\t\t" + "}";
                    strDAL = strDAL + "\r\t\t" + "}";
                    strDAL = strDAL + "\r\t\t" + "}";
                    strDAL = strDAL + "\r\t\t" + "return false;"; 
                    strDAL = strDAL + "\r\t" + "}";
                }
                counter++;
            }
            strDAL = strDAL + "\r" + "}";
            return strDAL;
        }


    }
}
