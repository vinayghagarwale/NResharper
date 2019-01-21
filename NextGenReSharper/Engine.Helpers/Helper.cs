using System.Diagnostics;
using System.Text.RegularExpressions;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
namespace NextGen.Engine.Helpers
{
    public static class Helper
    {
        public static string GetClassName(string strLine)
        {
            string sreturnvalue = "";

            for (int i = strLine.Length - 5; i >= 0; i--)
            {
                if (strLine[i] == 95) continue;

                if (strLine[i] == 92) break;

                sreturnvalue = sreturnvalue + strLine[i];
            }
            char[] arr = sreturnvalue.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string RemoveComments(string strLine)
        {
            string returnstring = strLine;

            if (strLine.Contains("--"))
            {
                returnstring = strLine.Replace(strLine.Substring(strLine.IndexOf("--")), "");
            }

            if (returnstring.Contains("/*") && returnstring.Contains("*/"))
            {
                int len = returnstring.IndexOf("*/") - returnstring.IndexOf("/*");

                returnstring = returnstring.Replace(strLine.Substring(strLine.IndexOf("/*"), len + 2), "");
            }

            return returnstring;
        }

        public static string RemoveTabs(string strLine)
        {
            return (strLine.Replace("\t", " ").Trim());
        }
        public static string GetDatatype(string spDataType)
        {
            if (spDataType.ToUpper().Contains("INT"))
            {
                return "int";
            }
            else if (spDataType.ToUpper().Contains("UNIQUEIDENTIFIER"))
            {
                return "string";
            }
            else if (spDataType.ToUpper().Contains("VARCHAR"))
            {
                return "string";
            }
            else if (spDataType.ToUpper().Contains("CHAR"))
            {
                return "string";
            }
            else
                return "string";
        }
        public static void OpenVisualStudioIDE(string strFilePath)
        {
            try
            {
                //string strVSPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\devenv.exe";
                string strVSPath = "";

                foreach(var lstvsvesrions in getversions())
                {
                    strVSPath = GetVisualStudioInstallationPath(lstvsvesrions);
                    if (!string.IsNullOrEmpty(strVSPath))
                    {
                        strVSPath = strVSPath + @"\devenv.exe";
                        Process.Start(strVSPath, strFilePath);
                        return;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }


        public static string NoOfTab(int n)
        {
            string sTab = "";
            while (n > 0)
            {
                sTab = sTab + "\t";
                n--;
            }
            return sTab;
        }
        private static string GetVisualStudioInstallationPath(string version)
        {
            string installationPath = null;
            if (Environment.Is64BitOperatingSystem)
            {
                installationPath = (string)Registry.GetValue(
                   "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\VisualStudio\\" + version + "\\",
                    "InstallDir",
                    null);
            }
            else
            {
                installationPath = (string)Registry.GetValue(
           "HKEY_LOCAL_MACHINE\\SOFTWARE  \\Microsoft\\VisualStudio\\" + version + "\\",
                  "InstallDir",
                  null);
            }
            return installationPath;

        }

        private static List<string> getversions()
        {
            var registry = Registry.ClassesRoot;
            var subKeyNames = registry.GetSubKeyNames();
            var regex = new Regex(@"^VisualStudio\.edmx\.(\d+)\.(\d+)$");
            List<string> lstvsver = new List<string>();
            foreach (var subKeyName in subKeyNames)
            {
                var match = regex.Match(subKeyName);
                if (match.Success)
                    lstvsver.Add(match.Groups[1].Value + "." + match.Groups[2].Value);
            }
            return lstvsver;
        }
    }
}
