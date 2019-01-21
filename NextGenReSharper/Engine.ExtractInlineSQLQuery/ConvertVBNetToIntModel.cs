using System.IO;
using System;
using System.Collections.Generic;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;

namespace NextGen.Engine.NextGenReSharper
{
    public class ConvertVBNetToIntermediateModel
    {
        private IntermediateModel interMediateModel;
        private string sourceFilePath;

        System.IO.StreamReader file;
        public ConvertVBNetToIntermediateModel(string _sourceFilePath, IntermediateModel _intermediateModel)
        {
            interMediateModel = _intermediateModel;
            sourceFilePath = _sourceFilePath;

            ReadSourceFile();
        }


        public void Process()
        {
            if(File.Exists(sourceFilePath))
            {
                long linecounter = 0;
                string linetext;

                // Read the each line file and display it line by line.  
                while ((linetext = file.ReadLine()) != null)
                {
                    linecounter++;
                    LineDetail linedetail = new LineDetail();
                    linedetail.linetext = linetext;
                    linedetail.LineType = GetLineType(linetext);
                    linedetail.linenumber = linecounter;

                    //Find SQL queries using transaction type instance
                    if (linetext.Contains("With cTrans") || linetext.Contains("With mcTrans") || linetext.Contains("With gcTrans"))
                    {
                        linedetail.transType = GetTransType(linecounter);
                    }

                    //Add each line details object to intermediate model
                    interMediateModel.lslLineDetail.Add(linedetail);                    
                }
                
                //Close File instance
                file.Close();
            }            
        }

        #region Private Methods
        /// <summary>
        /// Method to read source file
        /// </summary>
        private void ReadSourceFile()
        {
            if (!string.IsNullOrEmpty(@sourceFilePath))
            {
                file = new System.IO.StreamReader(@sourceFilePath);
                interMediateModel.BLClassName = GetClassName(sourceFilePath);
            }
        }
        /// <summary>
        /// Methos which convert a vb.net code to Intermediate file
        /// </summary>
        private TransType GetTransType(long linecounter)
        {
            string line;

            var transtype = new TransType();

            while ((line = file.ReadLine()) != null)
            {
                linecounter++;
                //Get Trans value Ex Select, Update, Insert, Modify
                if (line.Contains(".Trans"))
                {
                    transtype.Trans = line.ToUpper();
                }
                //Get column value, set of columns need arrange
                else if (line.Contains(".Columns"))
                {
                    //Remove Comments
                    line = RemoveComments(line);
                    line = RemoveExtraChar(line);
                    transtype.Columns = transtype.Columns + line;

                    //Get column for different lines
                    while ((line = file.ReadLine()) != null)
                    {
                        linecounter++;
                        if (!(line.Contains(".Trans") || line.Contains(".Where") || line.Contains(".Table") || line.Contains(".OrderBy")))
                        {
                            line = RemoveComments(line);
                            line = RemoveExtraChar(line);
                            transtype.Columns = transtype.Columns + line;
                        }
                        else
                        {
                            if (line.Contains(".Table"))
                            {
                                //Remove Commnets
                                line = RemoveComments(line);
                                line = RemoveExtraChar(line);
                                foreach (var c in line)
                                {
                                    if (c != 34)
                                        transtype.Table = transtype.Table + c;
                                }
                            }
                            break;
                        }
                        
                    }
                }
                else if (line.Contains(".Where"))
                {
                    line = RemoveExtraChar(line);
                    BuildWhereClause(line, transtype);

                    transtype.Where = line;
                }
                else if (line.Contains(".Table"))
                {
                    //Remove Commnets
                    line = RemoveComments(line);
                    line = RemoveExtraChar(line);
                    foreach (var c in line)
                    {
                        if(c != 34)
                            transtype.Table = transtype.Table + c;
                    }
                }
                else if (line.Contains(".OrderBy"))
                {
                    //Remove Commnets
                    line = RemoveComments(line);
                    line = RemoveExtraChar(line);

                    transtype.OrderedBy = line;
                }
                else if (line.Contains(".Sql"))
                {
                    transtype.Sql = line;
                }
                else if (line.Contains("End With"))
                {
                    return transtype;
                }
                else
                {

                }
            }
            return transtype;
        }

        private linetype GetLineType(string strCodeLine)
        {
            strCodeLine = RemoveComments(strCodeLine);

            if (strCodeLine.Contains("Imports"))
                return linetype.Directive;
            else if (strCodeLine.Contains("Public Class") || strCodeLine.Contains("Private Class") || strCodeLine.Contains("Friends Class"))
                return linetype.Class;
            else if (strCodeLine.Contains("Public Sub New"))
                return linetype.Constructor;
            else if (strCodeLine.Contains("Public Property"))
                return linetype.Property;
            else if (strCodeLine.Contains("Public Sub") || strCodeLine.Contains("Private Sub"))
                return linetype.Method;
            else
                return linetype.Other;
        }
        /// <summary>
        /// Method which remove comment code
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        private string RemoveComments(string strLine)
        {
            string sreturnvalue = "";
            foreach (var c in strLine)
            {
                if (c == 39) return sreturnvalue;

                sreturnvalue = sreturnvalue + c;
            }
            return sreturnvalue;
        }
        /// <summary>
        /// Method which renove extra charactor
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        private string RemoveExtraChar(string strLine)
        {
            string sreturnvalue = "";
            int i;
            for (i = 0; i <= strLine.Length - 1; i++)
            {
                if (strLine[i] == 34) break;
            }

            for (int j = i + 1; j <= strLine.Length - 1; j++)
            {
                if (j > strLine.Length - 1)
                {
                    if (strLine[j + 2] == 38 && strLine[j] == 95) break;
                }

                if (strLine[j] != 34 && strLine[j] != 38)
                    sreturnvalue = sreturnvalue + strLine[j];
            }
            return sreturnvalue;
        }
        /// <summary>
        /// Get class from file path
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        private string GetClassName(string strLine)
        {
            string sreturnvalue = "";

            for(int i = strLine.Length - 4; i >= 0; i--)
            {
                if (strLine[i] == 92) break;

                sreturnvalue = sreturnvalue + strLine[i];
            }
            char[] arr = sreturnvalue.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void BuildWhereClause(string strLine, TransType transtype)
        {
            List<Parameter> paralist = new List<Parameter>();
            if (string.IsNullOrEmpty(strLine)) return;
            var strsource = strLine.Split(' ');
            string[] strDest = new string[100];
            int k = 0;
            foreach (var s in strsource)
            {
                if (s == "order") break;
                if(s != " " && s != "'" && s != "" && s != "where" && s != "WHERE" && s != "and")
                 strDest[k++] = s;

            }
            for(int ii = 0; ii <= strDest.Length - 1; ii = ii + 3)
            {
                if (strDest[ii] == null || strDest[ii + 2] == null) break;
                Parameter p = new Parameter();
                p.Name = strDest[ii];
                p.Value = strDest[ii + 2].Replace(@".", string.Empty);
                p.Datatype = DbDataTypes.VarChar;
                p.Direction = ParameterDirection.Input;

                paralist.Add(p);
            }
            transtype.ParameterList = paralist;
        }



        #endregion

    }
}
