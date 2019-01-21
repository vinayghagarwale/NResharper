using System.Collections.Generic;

namespace NextGen.Models.NGReSharper
{
    public class IntermediateModel
    {
        public string BLClassName { get; set; }
        public string SPName { get; set; }

        public List<LineDetail> lslLineDetail = new List<LineDetail>();

        public List<SQLQueryModel> lstSQLQueryModel = new List<SQLQueryModel>();

        public ConfigModel configModel = new ConfigModel();
    }
}
