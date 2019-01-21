using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using NextGen.Contract.NGReSharper;
using NextGen.Models.NGReSharper;

namespace NextGen.Engine.Logger
{
    public class Logger : ILogger
    {
        List<LogModel> lstLogData;
        CodeStatistics _codeStatistics;

        private string strDirectoryPath = @"C:\NextgenReshrper\";

        private string strFileName = @"Log.txt";

        private string strFileFullPath;

        public Logger()
        {
            lstLogData = new List<LogModel>();
            _codeStatistics = new CodeStatistics();
            strFileFullPath = strDirectoryPath + strFileName;
        }
        public CodeStatistics CodeStatistics { get { return _codeStatistics; } }
        public void Log(string log)
        {
            LogModel logmodel = new LogModel();
            logmodel.Description = log;
            logmodel.LogDateTime = DateTime.Now.ToString();
            lstLogData.Add(logmodel);
        }

        private void WriteIntoFile(string strFilePath)
        {
            if (!Directory.Exists(strDirectoryPath))
                Directory.CreateDirectory(strDirectoryPath);

            if (File.Exists(strFileFullPath))
                File.Delete(strFileFullPath);

            using (FileStream file = File.Create(strFileFullPath))
            {
            }
            using (StreamWriter file = new StreamWriter(strFileFullPath))
            {
                file.WriteLine("Nextgen ReSharper (NG RE#) Log File");

                file.WriteLine("***********************************************************");
                file.WriteLine("Total While Loops       : " + _codeStatistics.SPWhileCount);
                file.WriteLine("Total IF Statement      : " + _codeStatistics.SPIfCount);
                file.WriteLine("Total SQL Queries       : " + _codeStatistics.SPSQLQueryCount);
                file.WriteLine("Parameter Count         : " + _codeStatistics.SPParameterCount);
                file.WriteLine("Comments Count          : " + _codeStatistics.SPCommentCount);
                file.WriteLine("Local Variable Count    : " + _codeStatistics.SPLocalVariableCount);
                file.WriteLine("Print statement Count   : " + _codeStatistics.SPPrintCount);
                file.WriteLine("***********************************************************");

                file.WriteLine("***********************************************************");
                file.WriteLine("Interpreted Code        : " + _codeStatistics.SPInterpretedCodeCount);
                file.WriteLine("Uninterpreted Code      : " + _codeStatistics.SPUnInterpretedCodeCount);
                file.WriteLine("Empty Line Code         : " + _codeStatistics.SPEmptyLineCount);
                decimal abc = _codeStatistics.SPEmptyLineCount + _codeStatistics.SPUnInterpretedCodeCount + _codeStatistics.SPInterpretedCodeCount;
                decimal dTotalUnIdentifiedCode = 0;
                if (abc < _codeStatistics.SPTotalLineCount)
                    dTotalUnIdentifiedCode = _codeStatistics.SPTotalLineCount - abc;

                file.WriteLine("Unidentified Line Code  : " + (dTotalUnIdentifiedCode).ToString());

                file.WriteLine("                     ----------------------------");
                file.WriteLine("Total Line Count        : " + _codeStatistics.SPTotalLineCount);
                file.WriteLine("                     ----------------------------");
                file.WriteLine("***********************************************************");

                decimal dbl = ((_codeStatistics.SPInterpretedCodeCount + _codeStatistics.SPEmptyLineCount) / _codeStatistics.SPTotalLineCount);
                dbl = Math.Round(dbl, 2);
                file.WriteLine("Convert Percentage (%)  : " + dbl * 100 + "%");
                file.WriteLine("***********************************************************");

                foreach (var _logdata in lstLogData)
                {
                    file.WriteLine(_logdata.LogDateTime+ " " + _logdata.Description);
                }
                
            }
        }

        public void OpenLogFile()
        {
            WriteIntoFile(strFileFullPath);

            if (File.Exists(strFileFullPath))
            {
                Process.Start("notepad.exe", strFileFullPath);
            }
            else
            {
                throw new Exception("The log file does not exist in " + strFileFullPath + " Path");
            }
        }
    }
}
