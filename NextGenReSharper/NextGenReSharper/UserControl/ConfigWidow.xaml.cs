using NextGen.Models.NGReSharper;

namespace NextGenReSharper.UserControl
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class ConfigWidow
    {
        private ConfigModel _config;

        public ConfigModel Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public ConfigWidow()
        {
            InitializeComponent();

            
        }

        private void btnRules_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            _config.NameSpacePrefix = txtNamespaceName.Text;
            _config.SelectMethodName = txtSelectMethodName.Text;
            _config.InsertMethodName = txtInsertMethodName.Text;
            _config.UpdateMethodName = txtUpdateMethodName.Text;
            _config.DeleteMethodName = txtDeleteMethodName.Text;
            _config.ExecuteMethodName = txtExecuteMethodName.Text;

             this.Close();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_config.NameSpacePrefix))
                txtNamespaceName.Text = "NextGen.";
            else
                txtNamespaceName.Text = _config.NameSpacePrefix;

            if (string.IsNullOrEmpty(_config.SelectMethodName))
                txtSelectMethodName.Text = "GetDataMethod";
            else
                txtSelectMethodName.Text = _config.SelectMethodName;

            if (string.IsNullOrEmpty(_config.InsertMethodName))
                txtInsertMethodName.Text = "InsertDataMethod";
            else
                txtInsertMethodName.Text = _config.InsertMethodName;

            if (string.IsNullOrEmpty(_config.UpdateMethodName))
                txtUpdateMethodName.Text = "UpdateDataMethod";
            else
                txtUpdateMethodName.Text = _config.UpdateMethodName;

            if (string.IsNullOrEmpty(_config.DeleteMethodName))
                txtDeleteMethodName.Text = "DeleteDataMethod";
            else
                txtDeleteMethodName.Text = _config.DeleteMethodName;

            if (string.IsNullOrEmpty(_config.ExecuteMethodName))
                txtExecuteMethodName.Text = "ExecuteStoredProcMethod";
            else
                txtExecuteMethodName.Text = _config.ExecuteMethodName;
        }
    }
}
