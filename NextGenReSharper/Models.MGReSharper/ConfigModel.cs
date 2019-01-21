using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Models.NGReSharper
{
    public class ConfigModel
    {
        private string _nameSpacePrefix;

        public string NameSpacePrefix
        {
            get {
                if (string.IsNullOrEmpty(_nameSpacePrefix)) _nameSpacePrefix = "NextGen.";
                return _nameSpacePrefix;
            }
            set { _nameSpacePrefix = value; }
        }

        private string _selectMethodName;

        public string SelectMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_selectMethodName)) _selectMethodName = "GetDataMethod";
                return _selectMethodName;
            }
            set { _selectMethodName = value; }
        }

        private string _insertMethodName;

        public string InsertMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_insertMethodName)) _insertMethodName = "InsertDataMethod";
                return _insertMethodName;
            }
            set { _insertMethodName = value; }
        }

        private string _updateMethodName;

        public string UpdateMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_updateMethodName)) _updateMethodName = "UpdateDataMethod";
                return _updateMethodName;
            }
            set { _updateMethodName = value; }
        }


        private string _deleteMethodName;

        public string DeleteMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_deleteMethodName)) _deleteMethodName = "DeleteDataMethod";
                return _deleteMethodName;
            }
            set
            {
                _deleteMethodName = value;
            }
        }

        private string _executeMethodName;

        public string ExecuteMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_executeMethodName)) _executeMethodName = "ExecuteStoredProcMethod";
                return _executeMethodName;
            }
            set { _executeMethodName = value; }
        }

        private string _createMethodName;

        public string CreateMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_createMethodName)) _createMethodName = "CreateTableMethod";
                return _createMethodName;
            }
            set { _createMethodName = value; }
        }
    }
}
