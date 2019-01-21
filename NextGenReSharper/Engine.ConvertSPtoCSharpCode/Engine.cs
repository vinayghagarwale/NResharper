using System;
using System.Collections.Generic;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class Engine : ICreater
    {
        private readonly IntermediateModel _intermediateModel;
        private readonly SptoCSRules _rulesModel;
        private readonly ILogger _logger;
        private readonly string sModelName;
        private readonly string sDALName;
        private readonly string sModelPrivateName;
        private readonly string sDALPrivateName;

        List<string> PrivateMemmberList;

        public Engine(IntermediateModel intermediateModel, SptoCSRules rulesModel, ILogger logger)
        {
            _intermediateModel = intermediateModel;
            _rulesModel = rulesModel;
            _logger = logger;

            sModelName = _intermediateModel.BLClassName + "Model";
            sDALName = _intermediateModel.BLClassName + "DataAccess";
            sModelPrivateName = "_" + _intermediateModel.BLClassName + "Model";
            sDALPrivateName = "_" + _intermediateModel.BLClassName + "DataAccess";

            PrivateMemmberList = new List<string>();
        }

        public string CreateClass()
        {
            try
            {
                _logger.Log("Converting Stored Procedure to Engine Started");

                string sBusinessLogic = "";
                bool CommentStarted = false, cannotconvert = false;
                bool startprocessing = false;

                int iTabCount = 0;

                //Code to add Class Header Comments
                AddBusinessLogicClassHeaderComment(ref sBusinessLogic);

                //Code to add using references
                AddBusinessLogicClassReference(ref sBusinessLogic);

                //code to add Namespace 
                AddNamespaceStart(ref sBusinessLogic, ref iTabCount);

                //code to add model class
                AddBusinessLogicModel(ref sBusinessLogic, ref CommentStarted);
                
                //Code to add Class name
                AddClassName(ref sBusinessLogic, ref iTabCount);

                //Code to add Private members
                AddPrivateMembers(ref sBusinessLogic, ref iTabCount);

                CommentStarted = false;

                AddBusinessLogicPrimaryVariables(ref sBusinessLogic, ref iTabCount, ref CommentStarted);

                //Code to add Constructor
                AddConstructor(ref sBusinessLogic, ref iTabCount);

                AddProcessMethodStart(ref sBusinessLogic, ref iTabCount);

                CommentStarted = false;
                    
                for(int i = 0; i <= _intermediateModel.lslLineDetail.Count - 1; i++)
                {
                    var line = _intermediateModel.lslLineDetail[i];

                    _logger.CodeStatistics.SPTotalLineCount++;

                    if (string.IsNullOrEmpty(line.linetext.Trim())) continue;

                    line.linetext = Helper.RemoveTabs(line.linetext);
                    line.linetext = Helper.RemoveComments(line.linetext);

                    //Process starts from AS
                    if (!startprocessing && line.linetext != "AS")
                    {
                        continue;
                    }

                    if (line.linetext == "AS")
                    {
                        _logger.CodeStatistics.SPInterpretedCodeCount++;
                        startprocessing = true;
                        continue;
                    }

                    switch (line.SPLineType)
                    {
                       case SPlinetype.CommentStart:
                            AddCommentsCode(ref sBusinessLogic, ref iTabCount, line.linetext.Trim(), ref i);
                            break;

                        case SPlinetype.Comment:
                            if (_rulesModel.ConvertSPCommnet)
                            {
                                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//Comments";
                                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//" + line.linetext.Trim();
                                _logger.CodeStatistics.SPCommentCount++;
                                _logger.Log("Converting Stored Procedure Comment");
                            }
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.IFStatement:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "if " + GetStatementCondition(line.linetext, ref sBusinessLogic, StatementType.IF) + " ";
                            cannotconvert = false;
                            _logger.CodeStatistics.SPIfCount++;
                            _logger.Log("Converting IF Statement");
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            break;
                        case SPlinetype.While:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "while " + GetStatementCondition(line.linetext, ref sBusinessLogic, StatementType.WHILE) + "";
                            cannotconvert = false;
                            _logger.CodeStatistics.SPWhileCount++;
                            _logger.CodeStatistics.SPInterpretedCodeCount++;

                            _logger.Log("Converting While Loop");
                            
                            break;
                        case SPlinetype.SQLquery:
                            AddSQLQueryCode(ref sBusinessLogic, ref iTabCount, line.linetext.Trim());
                            cannotconvert = false;
                            break;
                        case SPlinetype.Print:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"throw new Exception(" + "\"" + line.linetext.Trim().Substring(7) + "\"" + @");";
                            sBusinessLogic = sBusinessLogic + "\r";
                            _logger.Log("Converting Stored Procedure Print Statement");
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.return1:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return;";
                            sBusinessLogic = sBusinessLogic + "\r";
                            _logger.Log("Converting Return Statement");
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.SET:
                            AddSetCode(ref sBusinessLogic, ref iTabCount, line.linetext.Trim());
                            cannotconvert = false;
                            break;

                        case SPlinetype.ELSE:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "else";
                            iTabCount++;
                            _logger.Log("Converting Else");
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.GO:
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.USE:
                            iTabCount++;
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.RAISERROR:
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + @"throw new Exception(" + "\"" + line.linetext.Trim() + "\"" + @");";
                            sBusinessLogic = sBusinessLogic + "\r";
                            _logger.Log("Converting Stored Procedure Print Statement");
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            cannotconvert = false;
                            break;
                        case SPlinetype.Other:
                            
                            if (!(line.linetext.Trim().ToUpper().StartsWith("END") ||
                                                                     line.linetext.Trim().ToUpper().StartsWith("BEGIN") ||
                                                                     line.linetext.Trim().ToUpper().StartsWith("AS") ||
                                                                     line.linetext.Trim().ToUpper().StartsWith("GO") ||
                                                                     line.linetext.Trim().ToUpper().StartsWith("USE")))
                            {
                                if (!cannotconvert)
                                {
                                    _logger.Log("Adding.. //CAN NOT CONVERT BELOW CODE");
                                    sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//CAN NOT CONVERT BELOW CODE";
                                }

                                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//" + line.linetext.Trim();
                                _logger.CodeStatistics.SPUnInterpretedCodeCount++;
                                
                                cannotconvert = true;
                            }
                            break;
                        default:

                            break;
                    }

                    if (line.linetext.Trim().ToUpper().StartsWith("END"))
                    {
                        iTabCount--;
                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
                        _logger.CodeStatistics.SPInterpretedCodeCount++;
                        _logger.Log("Converting END.");
                        cannotconvert = false;
                    }
                    else if (line.linetext.Trim().ToUpper().Contains("BEGIN"))
                    {
                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
                        iTabCount++;
                        _logger.CodeStatistics.SPInterpretedCodeCount++;
                        _logger.Log("Converting BEGIN.");
                        cannotconvert = false;
                    }
                }

                AddProcessMethodEnd(ref sBusinessLogic, ref iTabCount);

                AddDataAccessCode(ref sBusinessLogic, ref iTabCount);

                AddNamespaceEnd(ref sBusinessLogic, ref iTabCount);

                _logger.Log("Converting Stored Procedure to Business Logic Completed");

                return sBusinessLogic;
            }
            catch (Exception Ex)
            {
                _logger.Log(Ex.Message.ToString());
                throw Ex;
            }

        }
        #region Private Members

        private void AddSetCode(ref string sBusinessLogic, ref int iTabCount, string SetText)
        {
            if (!(SetText.Trim().Contains("ANSI_NULLS") || SetText.Trim().Contains("QUOTED_IDENTIFIER ")))
            {
                string svalue = SetText.Trim().Replace("\t", "").Substring(4).Replace("'", "\"");

                foreach (var sVar in svalue.Split(' '))
                {
                    if (sVar.Trim().StartsWith("@"))
                    {
                        if (!(PrivateMemmberList.Find(x => x == sVar.Substring(1)) != null))
                            svalue = svalue.Replace(sVar, "_" + _intermediateModel.BLClassName + "Model." + sVar.Substring(1));
                    }

                }

                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + svalue + ";";
                _logger.Log("Converting SET");                
            }
            _logger.CodeStatistics.SPInterpretedCodeCount++;
        }
        private void AddDataAccessCode(ref string sBusinessLogic, ref int iTabCount)
        {
            if (!_rulesModel.AddDataAccessLogic)
            {
                iTabCount--;
                iTabCount--;
                (new DataAccess(_intermediateModel, _rulesModel, _logger)).BuildQueryMethod(ref sBusinessLogic, ref iTabCount);
            }

            iTabCount--;
        }
        private void AddProcessMethodStart(ref string sBusinessLogic, ref int iTabCount)
        {
            //Start creating a process to execute business logic
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <summary>";
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// Method which starts processing the business logic ";
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// </summary>";
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// <returns></returns>";
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "/// ";

            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public bool Process()";
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
            iTabCount++;
        }
        private void AddProcessMethodEnd(ref string sBusinessLogic, ref int iTabCount)
        {
            iTabCount--;
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "return true;";
            iTabCount--;
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";

            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
        }

        private void AddConstructor(ref string sBusinessLogic, ref int iTabCount)
        {
            //Create Constructor and inject Model
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//Constructor";

            if (_rulesModel.AddDataAccessLogic)
            {
                if (!_rulesModel.AddContractLogic)
                    sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public " + _intermediateModel.BLClassName + "Engine( " + sModelName + " " + _intermediateModel.BLClassName + "Model" + ", " + sDALName + " " + _intermediateModel.BLClassName + "DataAccess" + ")";
                else
                    sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public " + _intermediateModel.BLClassName + "Engine( " + sModelName + " " + _intermediateModel.BLClassName + "Model" + ", " + "I" + sDALName + " " + _intermediateModel.BLClassName + "DataAccess" + ")";
            }
            else
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public " + _intermediateModel.BLClassName + "Engine( " + sModelName + " " + _intermediateModel.BLClassName + "Model" + ")";
            }
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
            iTabCount++;
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + sModelPrivateName + " = " + _intermediateModel.BLClassName + "Model ;";
            if (_rulesModel.AddDataAccessLogic)
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + sDALPrivateName + " = " + _intermediateModel.BLClassName + "DataAccess ;";
            }
            iTabCount--;
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "}";
        }
        private void AddPrivateMembers(ref string sBusinessLogic, ref int iTabCount)
        {

            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "private " + sModelName + " " + sModelPrivateName + "; ";

            if (_rulesModel.AddDataAccessLogic)
            {
                if (!_rulesModel.AddContractLogic)
                    sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "private " + sDALName + " " + sDALPrivateName + "; ";
                else
                    sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "private " + "I" + sDALName + " " + sDALPrivateName + "; ";
            }
            else
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "private DatabaseClient _dbClient = null;";
            }
        }
        private void AddClassName(ref string sBusinessLogic, ref int iTabCount)
        {
            //create Business Logic Class
            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//Business Logic Class ";
            if (!_rulesModel.AddContractLogic)
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public class " + _intermediateModel.BLClassName + "Engine";
            else
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "public class " + _intermediateModel.BLClassName + "Engine : " + "I" + _intermediateModel.BLClassName + "Engine";

            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "{";
            iTabCount++;
        }
        private void AddBusinessLogicClassHeaderComment(ref string sBusinessLogic)
        {
            sBusinessLogic = sBusinessLogic + "//**********************************************************";
            sBusinessLogic = sBusinessLogic + "\r" + "//    This Business Logic class has been created by Nextgen ReSharper (NG RE#)";
            sBusinessLogic = sBusinessLogic + "\r" + "//    Created on : " + DateTime.Now;
            sBusinessLogic = sBusinessLogic + "\r" + "//    Source Stored Procedure Name : " + _intermediateModel.BLClassName + ".SQL";
            sBusinessLogic = sBusinessLogic + "\r" + "//**********************************************************";
        }
        private void AddNamespaceStart(ref string sBusinessLogic, ref int iTabCount)
        {
            if (_rulesModel.AddNamespace)
            {
                sBusinessLogic = sBusinessLogic + "\r" + "namespace " + _intermediateModel.configModel.NameSpacePrefix + _intermediateModel.BLClassName;
                sBusinessLogic = sBusinessLogic + "\r" + "{";
                iTabCount = 1;
            }
        }

        private void AddNamespaceEnd(ref string sBusinessLogic, ref int iTabCount)
        {
            iTabCount--;
            if (_rulesModel.AddNamespace)
            {
                sBusinessLogic = sBusinessLogic + "\r" + "}";
            }
        }
        private void AddBusinessLogicClassReference(ref string sBusinessLogic)
        {
            //Create reference Dlls
            sBusinessLogic = sBusinessLogic + "\r" + "using System;";
            sBusinessLogic = sBusinessLogic + "\r" + "using System.Collections.Generic;";
            if (!_rulesModel.AddDataAccessLogic)
            {
                sBusinessLogic = sBusinessLogic + "\r" + "using NextGen.Data;";
                sBusinessLogic = sBusinessLogic + "\r" + "using NextGen.Core;";
                sBusinessLogic = sBusinessLogic + "\r" + "using System.Data;";
            }

        }
        private void AddBusinessLogicModel(ref string sBusinessLogic, ref bool CommentStarted)
        {
            if (!_rulesModel.AddModelLogic)
            {
                _logger.Log("Creating Model started");
                //Create Model for the SP parameters
                sBusinessLogic = sBusinessLogic + "\r" + "public class " + sModelName;
                sBusinessLogic = sBusinessLogic + "\r" + "{";

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
                            sBusinessLogic = sBusinessLogic + "\r\t" + "public string " + line.linetext.Trim().Split(' ')[0].Substring(1) + "{ get; set; }";
                            _logger.CodeStatistics.SPParameterCount++;
                            _logger.CodeStatistics.SPInterpretedCodeCount++;
                            _logger.Log("Creating Model Member");
                        }
                    }
                }
                //Close Model
                sBusinessLogic = sBusinessLogic + "\r" + "}";
                _logger.Log("Creating Model Completed");
            }
        }
        private void AddBusinessLogicPrimaryVariables(ref string sBusinessLogic, ref int iTabCount, ref bool CommentStarted)
        {
            string sDeclare = "//Declare Local variable";
            //Create Private members
            foreach (var line in _intermediateModel.lslLineDetail)
            {
                if (string.IsNullOrEmpty(line.linetext.Trim())) continue;

                if (line.SPLineType == SPlinetype.LocalVariable)
                {
                    if (line.SPLineType == SPlinetype.CommentStart) CommentStarted = true;
                    if (line.SPLineType == SPlinetype.CommentEnd) CommentStarted = false;

                    if (CommentStarted == false)
                    {
                        if (!string.IsNullOrEmpty(sDeclare))
                        {
                            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + sDeclare;
                            sDeclare = "";
                        }

                        line.linetext = line.linetext.Replace("\t", " ");
                        string sLocal = line.linetext.Trim().Split(' ')[1].Substring(1).Replace("\t", " ").Split(' ')[0];
                        string[] dt = line.linetext.Trim().Split(' ')[1].Substring(1).Replace("\t", " ").Split(' ');

                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + " private " + Helper.GetDatatype(line.linetext.Trim()) + " " + sLocal + ";";

                        _logger.Log("Creating Local Members");

                        PrivateMemmberList.Add(sLocal);
                        _logger.CodeStatistics.SPLocalVariableCount++;
                        _logger.CodeStatistics.SPInterpretedCodeCount++;
                    }
                }
            }
        }

        private void AddCommentsCode(ref string sBusinessLogic, ref int iTabCount, string Commentsdata,ref int i)
        {
            int j;

            if (_rulesModel.ConvertSPCommnet)
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//SP Comments started";
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + Commentsdata;
            }
            _logger.CodeStatistics.SPInterpretedCodeCount++;

            for (j = i + 1; j <= _intermediateModel.lslLineDetail.Count - 1; j++)
            {
                var line1 = _intermediateModel.lslLineDetail[j];

                if (line1.SPLineType == SPlinetype.CommentEnd)
                {
                    if (_rulesModel.ConvertSPCommnet)
                    {
                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + line1.linetext.Trim();
                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//SP Comments Ended";
                    }
                    _logger.CodeStatistics.SPCommentCount++;
                    _logger.CodeStatistics.SPInterpretedCodeCount++;
                    break;
                }
                else
                {
                    if (_rulesModel.ConvertSPCommnet)
                    {
                        sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + line1.linetext.Trim();
                        _logger.Log("Converting SP Commneted Code");
                    }
                    _logger.CodeStatistics.SPCommentCount++;
                    _logger.CodeStatistics.SPInterpretedCodeCount++;
                    continue;
                }
            }
            i = j;
        }
        private void AddSQLQueryCode(ref string sBusinessLogic, ref int iTabCount,string sqlquerydata)
        {
            SQLQueryModel sql = new SQLQueryModel();

            sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + "//SQL queries";

            if (sqlquerydata.ToUpper().Contains("SELECT"))
                sql.SQLQueryType = QueryType.Select;
            else if (sqlquerydata.ToUpper().Contains("UPDATE"))
                sql.SQLQueryType = QueryType.Update;
            else if (sqlquerydata.ToUpper().Contains("INSERT"))
                sql.SQLQueryType = QueryType.Insert;
            else if (sqlquerydata.ToUpper().Contains("DELETE"))
                sql.SQLQueryType = QueryType.Delete;
            else if (sqlquerydata.ToUpper().Contains("CREATE"))
                sql.SQLQueryType = QueryType.Create;
            else if (sqlquerydata.ToUpper().Contains("EXECUTE"))
                sql.SQLQueryType = QueryType.Execute;

            if (sql.SQLQueryType == QueryType.Select)
                sql.SourceMethodName = _intermediateModel.configModel.SelectMethodName + _logger.CodeStatistics.SPSQLQueryCount;
            else if (sql.SQLQueryType == QueryType.Update)
                sql.SourceMethodName = _intermediateModel.configModel.UpdateMethodName + _logger.CodeStatistics.SPSQLQueryCount;
            else if (sql.SQLQueryType == QueryType.Insert)
                sql.SourceMethodName = _intermediateModel.configModel.InsertMethodName + _logger.CodeStatistics.SPSQLQueryCount;
            else if (sql.SQLQueryType == QueryType.Delete)
                sql.SourceMethodName = _intermediateModel.configModel.DeleteMethodName + _logger.CodeStatistics.SPSQLQueryCount;
            else if (sql.SQLQueryType == QueryType.Create)
                sql.SourceMethodName = "CreateDataMethod" + _logger.CodeStatistics.SPSQLQueryCount;
            else if (sql.SQLQueryType == QueryType.Execute)
                sql.SourceMethodName = _intermediateModel.configModel.ExecuteMethodName + _logger.CodeStatistics.SPSQLQueryCount;
            else
                sql.SourceMethodName = _intermediateModel.configModel.SelectMethodName + _logger.CodeStatistics.SPSQLQueryCount;


            if (_rulesModel.AddDataAccessLogic)
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + sDALPrivateName + "." + sql.SourceMethodName + "();";
            }
            else
            {
                sBusinessLogic = sBusinessLogic + "\r" + Helper.NoOfTab(iTabCount) + sql.SourceMethodName + "();";
            }


            sql.SQLQueryText = sqlquerydata;

            _intermediateModel.lstSQLQueryModel.Add(sql);

            _logger.CodeStatistics.SPSQLQueryCount++;
            _logger.Log("Converting SQL Query");
            _logger.CodeStatistics.SPInterpretedCodeCount++;
        }
        private string GetStatementCondition(string line, ref string sLogic, StatementType StatementType)
        {
            if (line == "END")
            {
                sLogic = sLogic + "\r\t" + "}";
                return "";
            }

            string sIfStatement = "";
            string strStatementType = StatementType.ToString();

            for (int i = 0; i < line.Trim().Split(' ').Length; i++)
            {
                var cIfChar = line.Trim().Split(' ')[i];

                if (cIfChar.ToUpper().StartsWith(strStatementType.ToUpper().ToString()))
                {
                    foreach (var l1 in cIfChar.Trim().Split('('))
                    {
                        sIfStatement = sIfStatement + "(";

                        if ((l1.ToUpper().StartsWith(strStatementType.ToUpper().ToString()) || l1.StartsWith("LEN")) || (l1.StartsWith("RTRIM")) || (l1.StartsWith("LTRIM"))) continue;

                        if (l1.Contains(")"))
                        {
                            foreach (var l2 in l1.Trim().Split(')'))
                            {
                                if (string.IsNullOrEmpty((l2)))
                                {
                                    sIfStatement = sIfStatement + ")";
                                    continue;
                                }

                                if (l2.Contains("@"))
                                {
                                    if (PrivateMemmberList.Find(x => x == l2.Substring(l2.IndexOf('@')).Substring(1)) != null)
                                        sIfStatement = sIfStatement + l2.Replace(l2.Substring(l2.IndexOf('@')), l2.Substring(l2.IndexOf('@')).Substring(1));
                                    else
                                        sIfStatement = sIfStatement + l2.Replace(l2.Substring(l2.IndexOf('@')), "_" + _intermediateModel.BLClassName + "Model." + l2.Substring(l2.IndexOf('@')).Substring(1));
                                }

                                sIfStatement = sIfStatement + ")";
                            }
                        }
                        else
                        {
                            sIfStatement = sIfStatement + l1;
                        }
                    }
                    continue;
                }

                if (cIfChar.Contains("@"))
                {
                    foreach (var l2 in cIfChar.Trim().Split(')'))
                    {
                        if (string.IsNullOrEmpty((l2)))
                        {
                            sIfStatement = sIfStatement + ")";
                            continue;
                        }

                        if (PrivateMemmberList.Find(x => x == l2.Substring(cIfChar.IndexOf('@')).Substring(1)) != null)
                            sIfStatement = sIfStatement + l2.Replace(l2.Substring(l2.IndexOf('@')), l2.Substring(l2.IndexOf('@')).Substring(1));
                        else
                            sIfStatement = sIfStatement + l2.Replace(l2.Substring(l2.IndexOf('@')), "_" + _intermediateModel.BLClassName + "Model." + l2.Substring(l2.IndexOf('@')).Substring(1));

                    }
                }
                else if (cIfChar.StartsWith("="))
                {
                    sIfStatement = sIfStatement + " == ";
                }
                else if (cIfChar.ToUpper().StartsWith("AND"))
                {
                    sIfStatement = sIfStatement + " && ";
                }
                else if (cIfChar.ToUpper().StartsWith("OR"))
                {
                    sIfStatement = sIfStatement + " || ";
                }
                else if (cIfChar.ToUpper().StartsWith("NULL"))
                {
                    if (cIfChar.ToUpper().Contains(")"))
                        sIfStatement = sIfStatement + " null) ";
                    else
                        sIfStatement = sIfStatement + " null ";
                }
                else if (cIfChar.ToUpper().StartsWith("IS"))
                {
                    int j = i + 1;
                    if (cIfChar.ToUpper().StartsWith("IS") && line.Trim().Split(' ')[j].ToUpper().StartsWith("NOT") && line.Trim().Split(' ')[++j].ToUpper().StartsWith("NULL"))
                    {
                        sIfStatement = sIfStatement + " != null ";
                        i = i + 2;
                    }
                    else
                    {
                        sIfStatement = sIfStatement + " == ";
                    }
                }
                else if (cIfChar.StartsWith("'"))
                {
                    sIfStatement = sIfStatement + " " + cIfChar.Replace("'", "\"");
                }
                else if (cIfChar.StartsWith("BEGIN"))
                {
                    sIfStatement = sIfStatement + " ) " + "";
                }
                else if (cIfChar.StartsWith("<>"))
                {
                    sIfStatement = sIfStatement + " != " + "";
                }
                else
                {
                    sIfStatement = sIfStatement + " " + cIfChar.Replace("'", "\"");
                }
            }
            if(sIfStatement.Split('(').Length > sIfStatement.Split(')').Length)
            {
                int iCountLeft = sIfStatement.Split('(').Length - sIfStatement.Split(')').Length;

                while(iCountLeft > 0)
                {
                    sIfStatement = sIfStatement + ")";
                    iCountLeft--;
                }
            }
                

            return sIfStatement;
        }

        #endregion
    }
}
