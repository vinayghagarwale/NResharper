
using System.Windows;

using NextGen.Models.NGReSharper;
namespace NextGenReSharper.UserControl
{
    /// <summary>
    /// Interaction logic for Rules.xaml
    /// </summary>
    public partial class RulesWindow
    {
        public RulesWindow()
        {
            InitializeComponent();
            chkAUTforDAL.Visibility = Visibility.Collapsed;
        }

        private SptoCSRules rules;

        public SptoCSRules Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        private void chkDAL_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chkDAL.IsChecked)
            {
                chkAUTforDAL.Visibility = Visibility.Visible;
            }
            else
            {
                chkAUTforDAL.IsChecked = false;
                chkAUTforDAL.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRules_Click(object sender, RoutedEventArgs e)
        {
            rules.AddBusinessLogic = true;

            if (chkDAL.IsChecked == true)
                rules.AddDataAccessLogic = true;
            else
                rules.AddDataAccessLogic = false;

            if (chkSPComment.IsChecked == true)
                rules.ConvertSPCommnet = true;
            else
                rules.ConvertSPCommnet = false;

            if (chkModel.IsChecked == true)
                rules.AddModelLogic = true;
            else
                rules.AddModelLogic = false;

            if (chkContract.IsChecked == true)
                rules.AddContractLogic = true;
            else
                rules.AddContractLogic = false;

            if (chkAddNameSpace.IsChecked == true)
                rules.AddNamespace = true;
            else
                rules.AddNamespace = false;

            if (chkCreateProject.IsChecked == true)
                rules.CreateCSharpProject = true;
            else
                rules.CreateCSharpProject = false;

            if (chkAUTforBL.IsChecked == true)
                rules.AddAUTforBLLogic = true;
            else
                rules.AddAUTforBLLogic = false;

            if (chkAUTforDAL.IsChecked == true)
                rules.AddAUTforDALLogic = true;
            else
                rules.AddAUTforDALLogic = false;

            if (chkCreateProject.IsChecked == true)
                rules.CreateCSharpProject = true;
            else
                rules.CreateCSharpProject = false;
            this.Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (rules.AddDataAccessLogic == true) chkDAL.IsChecked = true;
            if (rules.ConvertSPCommnet == true) chkSPComment.IsChecked = true;
            if (rules.AddModelLogic == true) chkModel.IsChecked = true;
            if (rules.AddContractLogic == true) chkContract.IsChecked = true;
            if (rules.AddNamespace == true) chkAddNameSpace.IsChecked = true;
            if (rules.CreateCSharpProject == true) chkCreateProject.IsChecked = true;
            if (rules.AddAUTforBLLogic == true) chkAUTforBL.IsChecked = true;
            if (rules.AddAUTforDALLogic == true) chkAUTforDAL.IsChecked = true;
        }
    }
}
