using System;
using System.IO;

using NextGen.Models.NGReSharper;
using NextGen.Engine.Helpers;
using NextGen.Contract.NGReSharper;

namespace NextGen.Engine.Converter
{
    public class ConvertStoredProctoModel : IConverter
    {
        private readonly IntermediateModel interMediateModel;
        private string sourceFilePath;
        private readonly ILogger _logger;

        StreamReader file;
        bool bCommentStarted = false;

        public ConvertStoredProctoModel(string _sourceFilePath, IntermediateModel _intermediateModel, ILogger logger)
        {
            interMediateModel = _intermediateModel;
            sourceFilePath = _sourceFilePath;
            _logger = logger;

            if (!string.IsNullOrEmpty(@sourceFilePath))
            {
                if (!File.Exists(@sourceFilePath))
                    throw new Exception("Stored Procedure does not exist");

                interMediateModel.BLClassName = Helper.GetClassName(sourceFilePath);
            }
            else
            {
                if (!File.Exists(@sourceFilePath))
                    throw new Exception("Stored Procedure does not exist");
            }
        }

        public void Convert()
        {
            long linecounter = 0;
            string linetext;
            using (file = new System.IO.StreamReader(@sourceFilePath))
            {
                interMediateModel.lslLineDetail.Clear();
                // Read the each line file and display it line by line.  
                while ((linetext = file.ReadLine()) != null)
                {
                        
                    //Add each line details object to intermediate model
                    interMediateModel.lslLineDetail.Add(new LineDetail
                                                                    {
                                                                        linenumber = ++linecounter,
                                                                        linetext = linetext,
                                                                        SPLineType = GetLineType(linetext)
                                                                    }
                                                        );
                }
                UpdateModel();
            }
        }

        #region Private Methods

        private void UpdateModel()
        {
            for (int i = 0; i <= interMediateModel.lslLineDetail.Count - 1; i++)
            {
                var line = interMediateModel.lslLineDetail[i];

                if (line.SPLineType == SPlinetype.SQLquery)
                {
                    for (int j = i + 1; j <= interMediateModel.lslLineDetail.Count - 1; j++)
                    {
                        var line1 = interMediateModel.lslLineDetail[j];

                        if (line1.linetext.Trim().Length > 0 &&
                        !line1.linetext.Trim().StartsWith("IF") &&
                        (!line1.linetext.Trim().StartsWith("SET") || line.linetext.Trim().StartsWith("UPDATE")) &&
                        !line1.linetext.Trim().StartsWith("BEGIN"))
                        {
                            interMediateModel.lslLineDetail[i].linetext = interMediateModel.lslLineDetail[i].linetext +
                                                                          interMediateModel.lslLineDetail[j].linetext;
                            interMediateModel.lslLineDetail[j].linetext = "";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (line.SPLineType == SPlinetype.Comment)
                {

                }
            }
        }
        private SPlinetype GetLineType(string linetext)
        {
            linetext = linetext.Trim();
            linetext = linetext.Replace('\t', ' ');

            //Remove commnets if it is has executable code
            if (linetext.Trim().ToUpper().Contains("/*") && linetext.ToUpper().Contains("*/"))
            {
                linetext = Helper.RemoveComments(linetext);
            }

            if (linetext.Trim().ToUpper().Contains("*/"))
            {
                bCommentStarted = false;
                return SPlinetype.CommentEnd;
            }

            if (bCommentStarted == true)
            {
                return SPlinetype.Comment;
            }

            if (linetext.Contains("ALTER PROCEDURE") || linetext.Contains("CREATE PROCEDURE"))
            {
                return SPlinetype.SPName;
            }
            else if (linetext.Trim().StartsWith("@"))
            {
                return SPlinetype.Paramameters;
            }
            else if (linetext.Trim().ToUpper().StartsWith("IF"))
            {
                return SPlinetype.IFStatement;
            }
            else if (linetext.Trim().ToUpper().StartsWith("PRINT"))
            {
                return SPlinetype.Print;
            }
            else if (linetext.Trim().ToUpper().StartsWith("DECLARE"))
            {
                return SPlinetype.LocalVariable;
            }
            else if (linetext.Trim().ToUpper().StartsWith("RETURN"))
            {
                return SPlinetype.return1;
            }
            else if (linetext.Trim().ToUpper().StartsWith("WHILE"))
            {
                return SPlinetype.While;
            }
            else if (linetext.Trim().ToUpper().StartsWith("SELECT"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().StartsWith("UPDATE"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().StartsWith("DELETE"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().Contains("INSERT INTO"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().StartsWith("DROP"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().StartsWith("EXECUTE"))
            {
                return SPlinetype.SQLquery;
            }
            else if (linetext.Trim().ToUpper().StartsWith("--"))
            {
                return SPlinetype.Comment;
            }
            else if (linetext.Trim().ToUpper().StartsWith("/*"))
            {
                bCommentStarted = true;
                return SPlinetype.CommentStart;
            }
            else if (linetext.Trim().ToUpper().StartsWith("SET"))
            {
                return SPlinetype.SET;
            }
            else if (linetext.Trim().ToUpper().Contains("ELSE"))
            {
                return SPlinetype.ELSE;
            }
            else if (linetext.Trim().ToUpper().StartsWith("GO"))
            {
                return SPlinetype.GO;
            }
            else if (linetext.Trim().ToUpper().StartsWith("USE"))
            {
                return SPlinetype.USE;
            }
            else if (linetext.Trim().ToUpper().StartsWith("RAISERROR"))
            {
                return SPlinetype.RAISERROR;
            }
            else if (string.IsNullOrEmpty(linetext.Trim()))
            {
                return SPlinetype.EmptyLine;
            }
            else
            {
                return SPlinetype.Other;
            }
        }
        #endregion  
    }
}
