using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using NextGen.Manager.NGReSharper;
using NextGen.Models.NGReSharper;
using NextGenReSharper.Resource;
using NextGenReSharper.UserControl;
using NextGen.Engine.Helpers;
namespace NextGenReSharper.ViewModel
{
    public class NextGenResharperViewmodel : INotifyPropertyChanged
    {
        private ConvertionType _convertType;
        private readonly SptoCSRules _SPtoCSrulemodel;
        NGReSharperContext _nGReSharperContext;

        public NextGenResharperViewmodel(NGReSharperContext nGReSharperContext)
        {
            _nGReSharperContext = nGReSharperContext;
            _SPtoCSrulemodel = new SptoCSRules();            
            _SPtoCSrulemodel.AddBusinessLogic = true;
            _convertType = ConvertionType.ConvertSPtoCS;
            ShowProgressRing = Visibility.Hidden;
            DisableTabsBasedonRules();
            ProcessIndicator = Visibility.Collapsed;
        }

        #region Properties
        private Visibility _processIndicator = Visibility.Collapsed;
        public Visibility ProcessIndicator
        {
            get { return _processIndicator; }
            set
            {
                _processIndicator = value;
                NotifyPropertyChanged("ProcessIndicator");
            }
        }

        private Visibility _Converted;
        public Visibility Converted
        {
            get
            {
                return _Converted;
            }
            set
            {
                _Converted = value;
                NotifyPropertyChanged("Converted");
            }
        }

        private Visibility _ShowEngineTab;
        public Visibility ShowEngineTab
        {
            get
            {
                return _ShowEngineTab;
            }
            set
            {
                if (value == Visibility.Visible)
                    Converted = Visibility.Collapsed;
                else
                    Converted = Visibility.Visible;
                _ShowEngineTab = value;
                NotifyPropertyChanged("ShowEngineTab");
            }
        }

        private string _SourceData;
        public string SourceData
        {
            get
            {
                return _SourceData;
            }
            set
            {
                _SourceData = value;
                NotifyPropertyChanged("SourceData");
            }
        }

        private string _SourceEngineData;
        public string SourceEngineData
        {
            get
            {
                return _SourceEngineData;
            }
            set
            {
                _SourceEngineData = value;
                NotifyPropertyChanged("SourceEngineData");
            }
        }

        private Visibility _ShowDataAccessTab;
        public Visibility ShowDataAccessTab
        {
            get
            {
                return _ShowDataAccessTab;
            }
            set
            {
                _ShowDataAccessTab = value;
                NotifyPropertyChanged("ShowDataAccessTab");
            }
        }

        private string _SourceDataAccessData;
        public string SourceDataAccessData
        {
            get
            {
                return _SourceDataAccessData;
            }
            set
            {
                _SourceDataAccessData = value;
                NotifyPropertyChanged("SourceDataAccessData");
            }
        }

        private Visibility _ShowModelTab;
        public Visibility ShowModelTab
        {
            get
            {
                return _ShowModelTab;
            }
            set
            {
                _ShowModelTab = value;
                NotifyPropertyChanged("ShowModelTab");
            }
        }

        private string _SourceModelData;
        public string SourceModelData
        {
            get
            {
                return _SourceModelData;
            }
            set
            {
                _SourceModelData = value;
                NotifyPropertyChanged("SourceModelData");
            }
        }

        private Visibility _ShowContractTab;
        public Visibility ShowContractTab
        {
            get
            {
                return _ShowContractTab;
            }
            set
            {
                _ShowContractTab = value;
                NotifyPropertyChanged("ShowContractTab");
            }
        }

        private string _SourceContractData;
        public string SourceContractData
        {
            get
            {
                return _SourceContractData;
            }
            set
            {
                _SourceContractData = value;
                NotifyPropertyChanged("SourceContractData");
            }
        }

        private Visibility _ShowAUTforDALTab;
        public Visibility ShowAUTforDALTab
        {
            get
            {
                return _ShowAUTforDALTab;
            }
            set
            {
                _ShowAUTforDALTab = value;
                NotifyPropertyChanged("ShowAUTforDALTab");
            }
        }

        private string _SourceAUTforDALData;
        public string SourceAUTforDALData
        {
            get
            {
                return _SourceAUTforDALData;
            }
            set
            {
                _SourceAUTforDALData = value;
                NotifyPropertyChanged("SourceAUTforDALData");
            }
        }

        private Visibility _ShowAUTforBLTab;
        public Visibility ShowAUTforBLTab
        {
            get
            {
                return _ShowAUTforBLTab;
            }
            set
            {
                _ShowAUTforBLTab = value;
                NotifyPropertyChanged("ShowAUTforBLTab");
            }
        }

        private string _SourceAUTforBLData;
        public string SourceAUTforBLData
        {
            get
            {
                return _SourceAUTforBLData;
            }
            set
            {
                _SourceAUTforBLData = value;
                NotifyPropertyChanged("SourceAUTforBLData");
            }
        }

        private Visibility _ShowProgressRing;
        public Visibility ShowProgressRing
        {
            get
            {
                return _ShowProgressRing;
            }
            set
            {
                _ShowProgressRing = value;
                NotifyPropertyChanged("ShowProgressRing");
            }
        }

        
        private string _SourceFilePath;
        public string SourceFilePath
        {
            get
            {
                return _SourceFilePath;
            }
            set
            {
                _SourceFilePath = value;
                NotifyPropertyChanged("SourceFilePath");
            }
        }

        private string _SourceFileData;
        public string SourceFileData
        {
            get
            {
                return _SourceFileData;
            }
            set
            {
                _SourceFileData = value;
                NotifyPropertyChanged("SourceFileData");
            }
        }

        #endregion

        #region Relay Commands

        private RelayCommand _Convert;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand Convert
        {
            get
            {
                return _Convert ?? (_Convert = new RelayCommand(param => ProcessConvert(), param => true));
            }
        }

        private RelayCommand _OpenRulesEngine;

        /// <summary>
        /// Open Rules Engine
        /// </summary>
        public RelayCommand OpenRulesEngine
        {
            get
            {
                return _OpenRulesEngine ?? (_OpenRulesEngine = new RelayCommand(param => ProcessOpenRulesEngine(),param => true));
            }
        }

        private RelayCommand _OpenConfigEngine;

        /// <summary>
        /// Open Rules Engine
        /// </summary>
        public RelayCommand OpenConfigEngine
        {
            get
            {
                return _OpenConfigEngine ?? (_OpenConfigEngine = new RelayCommand(param => ProcessOpenConfigEngine(), param => true));
            }
        }

        private RelayCommand _ClearData;

        /// <summary>
        /// Clear Data
        /// </summary>
        public RelayCommand ClearData
        {
            get
            {
                return _ClearData ?? (_ClearData = new RelayCommand(param => ProcessClearData(), param => true));
            }
        }

        private RelayCommand _ClearDataInLine;

        /// <summary>
        /// Clear Data
        /// </summary>
        public RelayCommand ClearDataInLine
        {
            get
            {
                return _ClearDataInLine ?? (_ClearDataInLine = new RelayCommand(param => ProcessClearDataInLine(), param => true));
            }
        }

        private RelayCommand _OpenDialog;

        /// <summary>
        /// Open dialog to select source file
        /// </summary>
        public RelayCommand OpenDialog
        {
            get
            {
                return _OpenDialog ?? (_OpenDialog = new RelayCommand(param => ProcessOpenDialog(), param => true));
            }
        }

        private RelayCommand _ShowEngineData;

        /// <summary>
        /// Show engine class data
        /// </summary>
        public RelayCommand ShowEngineData
        {
            get
            {
                return _ShowEngineData ?? (_ShowEngineData = new RelayCommand(param => SourceData = SourceEngineData, param => true));
            }
        }

        private RelayCommand _ShowDataAccessData;

        /// <summary>
        /// Show Data Access class data
        /// </summary>
        public RelayCommand ShowDataAccessData
        {
            get
            {
                return _ShowDataAccessData ?? (_ShowDataAccessData = new RelayCommand(param => SourceData = SourceDataAccessData, param => true));
            }
        }


        private RelayCommand _ShowModelData;

        /// <summary>
        /// Show Model class data
        /// </summary>
        public RelayCommand ShowModelData
        {
            get
            {
                return _ShowModelData ?? (_ShowModelData = new RelayCommand(param => SourceData = SourceModelData, param => true));
            }
        }

        private RelayCommand _ShowContractData;

        /// <summary>
        /// Show Contract interface data
        /// </summary>
        public RelayCommand ShowContractData
        {
            get
            {
                return _ShowContractData ?? (_ShowContractData = new RelayCommand(param => SourceData = SourceContractData, param => true));
            }
        }

        private RelayCommand _ShowAutforDALData;

        /// <summary>
        /// Show Automated Unit test for Data Acccess class data
        /// </summary>
        public RelayCommand ShowAutforDALData
        {
            get
            {
                return _ShowAutforDALData  ?? (_ShowAutforDALData = new RelayCommand(param => SourceData = SourceAUTforDALData, param => true));
            }
        }

        private RelayCommand _ShowAutforBLData;

        /// <summary>
        //// Show Automated Unit test for Business Logic class data
        /// </summary>
        public RelayCommand ShowAutforBLData
        {
            get
            {
                return _ShowAutforBLData ?? (_ShowAutforBLData = new RelayCommand(param => SourceData = SourceAUTforBLData, param => true));
            }
        }

        private RelayCommand _OpenLog;

        /// <summary>
        /// Open log notepad here
        /// </summary>
        public RelayCommand OpenLog
        {
            get
            {
                return _OpenLog ?? (_OpenLog = new RelayCommand(param => ProcessOpenLog(), param => true));
            }
        }

        private RelayCommand _OpenVisualStudio;

        /// <summary>
        /// open Visual basic IDE
        /// </summary>
        public RelayCommand OpenVisualStudio
        {
            get
            {
                return _OpenVisualStudio ?? (_OpenVisualStudio = new RelayCommand(param => ProcessOpenVisualStudio(), param => true));
            }
        }

        private RelayCommand _OpenAbout;

        /// <summary>
        /// open Visual basic IDE
        /// </summary>
        public RelayCommand OpenAbout
        {
            get
            {
                return _OpenAbout ?? (_OpenAbout = new RelayCommand(param => ProcessOpenAbout(), param => true));
            }
        }


        #endregion

        #region Private Methods

        private void ProcessOpenLog()
        {
            try
            { 
                _nGReSharperContext._logger.OpenLogFile();
            }
            catch (Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }
        private void ProcessOpenVisualStudio()
        {
            try
            {
                Helper.OpenVisualStudioIDE(BuildFilesinVisualStudio());
            }
            catch (Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }

        private void ProcessConvert()
        {
            try
            {
                if (string.IsNullOrEmpty(SourceFileData))
                {
                    throw new Exception("Select Stored Procedure");
                }

                ProcessIndicator = Visibility.Visible;
                EnableTabsBasedonRules(_SPtoCSrulemodel);

                ProcessClearClassData();

                switch (_convertType)
                {
                    case ConvertionType.ConvertSPtoCS:

                        _nGReSharperContext.ngResharperManager.ConvertStoredProcedureToCSharpCode(SourceFilePath, _SPtoCSrulemodel);

                        if (_SPtoCSrulemodel.AddBusinessLogic)
                            SourceEngineData = _nGReSharperContext.ngResharperManager.EngineClass;

                        if (_SPtoCSrulemodel.AddDataAccessLogic)
                            SourceDataAccessData = _nGReSharperContext.ngResharperManager.DataAccessClass;

                        if (_SPtoCSrulemodel.AddModelLogic)
                            SourceModelData = _nGReSharperContext.ngResharperManager.ModelClass;

                        if (_SPtoCSrulemodel.AddContractLogic)
                            SourceContractData = _nGReSharperContext.ngResharperManager.ContractClass;

                        if (_SPtoCSrulemodel.AddAUTforDALLogic)
                            SourceAUTforDALData = _nGReSharperContext.ngResharperManager.DALAUTClass;

                        if (_SPtoCSrulemodel.AddAUTforBLLogic)
                            SourceAUTforBLData = _nGReSharperContext.ngResharperManager.BLAUTClass;

                        //default data
                        SourceData = SourceEngineData;

                        break;
                    case ConvertionType.CreateDALfromInline:

                        ShowEngineTab = Visibility.Collapsed;
                        ShowDataAccessTab = Visibility.Visible;

                        _nGReSharperContext.ngResharperManager.ExtractInLineCodeFromVBNetCode(SourceFilePath);

                        SourceDataAccessData = _nGReSharperContext.ngResharperManager.VBNETDataAccessClass;
                       // DestinationDALText.Document.Blocks.Add(new Paragraph(new Run(processConToDAL.Convert())));
                        break;
                }
                ProcessIndicator = Visibility.Collapsed;
            }
            catch(Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }
        
        private void ProcessOpenRulesEngine()
        {
            try
            {
                if (_convertType == ConvertionType.ConvertSPtoCS)
                {
                    RulesWindow rulewindow = new RulesWindow();
                    rulewindow.Rules = _SPtoCSrulemodel;

                    rulewindow.ShowDialog();

                }
                else if (_convertType == ConvertionType.CreateDALfromInline)
                {

                }
            }
            catch (Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }

        private void ProcessOpenAbout()
        {
            try
            {
                    AboutWindow awindow = new AboutWindow();

                    awindow.ShowDialog();
            }
            catch (Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }

        
        private void ProcessOpenConfigEngine()
        {
            try
            {

                if (_convertType == ConvertionType.ConvertSPtoCS)
                {
                    ConfigWidow configWidow = new ConfigWidow();

                    configWidow.Config = _nGReSharperContext._intermediateModel.configModel;
                    configWidow.ShowDialog();
                }
                else if (_convertType == ConvertionType.CreateDALfromInline)
                {

                }
            }
            catch (Exception Ex)
            {
                _nGReSharperContext.ShowMessage("Message", Ex.Message.ToString());
            }
        }
        
        private void ProcessClearData()
        {
            ProcessClearClassData();
            SourceFilePath = "";
            SourceFileData = "";
            DisableTabsBasedonRules();
            _nGReSharperContext._logger.CodeStatistics.Clear();
            _nGReSharperContext._intermediateModel.lstSQLQueryModel.Clear();
            _convertType = ConvertionType.ConvertSPtoCS;
        }

        private void ProcessClearDataInLine()
        {
            _convertType = ConvertionType.CreateDALfromInline;
            ProcessClearClassData();
            SourceFilePath = "";
            SourceFileData = "";
            DisableTabsBasedonRules();
            _nGReSharperContext._logger.CodeStatistics.Clear();
            _nGReSharperContext._intermediateModel.lstSQLQueryModel.Clear();
        }

        private void ProcessClearClassData()
        {
            SourceData = "";
            SourceDataAccessData = "";
            SourceModelData = "";
            SourceContractData = "";
            SourceAUTforDALData = "";
            SourceAUTforBLData = "";
        }

        private void ProcessOpenDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SourceFileData = File.ReadAllText(openFileDialog.FileName);
                SourceData = SourceFileData;
            }

            SourceFilePath = openFileDialog.FileName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void DisableTabsBasedonRules()
        {
            ShowEngineTab = Visibility.Collapsed;
            ShowDataAccessTab = Visibility.Collapsed;
            ShowModelTab = Visibility.Collapsed;
            ShowContractTab = Visibility.Collapsed;
            ShowAUTforBLTab = Visibility.Collapsed;
            ShowAUTforDALTab = Visibility.Collapsed;
        }

        private void EnableTabsBasedonRules(SptoCSRules LocalRules)
        {
            if (LocalRules.AddBusinessLogic == true)
                ShowEngineTab = Visibility.Visible;
            else
                ShowEngineTab = Visibility.Collapsed;

            if (LocalRules.AddDataAccessLogic == true)
                ShowDataAccessTab = Visibility.Visible;
            else
                ShowDataAccessTab = Visibility.Collapsed;

            if (LocalRules.AddModelLogic == true)
                ShowModelTab = Visibility.Visible;
            else
                ShowModelTab = Visibility.Collapsed;

            if (LocalRules.AddContractLogic == true)
                ShowContractTab = Visibility.Visible;
            else
                ShowContractTab = Visibility.Collapsed;

            if (LocalRules.AddAUTforBLLogic == true)
                ShowAUTforBLTab = Visibility.Visible;
            else
                ShowAUTforBLTab = Visibility.Collapsed;

            if (LocalRules.AddAUTforDALLogic == true)
                ShowAUTforDALTab = Visibility.Visible;
            else
                ShowAUTforDALTab = Visibility.Collapsed;
        }

        private string BuildFilesinVisualStudio()
        {
            string strFiles = "";

            if (_convertType == ConvertionType.ConvertSPtoCS)
            {
                string strBLPath = @"C:\CodeREsharper\" + _nGReSharperContext._intermediateModel.BLClassName + "Engine.cs";
                string strDALPath = @"C:\CodeREsharper\" + _nGReSharperContext._intermediateModel.BLClassName + "DataAccess.cs";
                string strModelPath = @"C:\CodeREsharper\" + _nGReSharperContext._intermediateModel.BLClassName + "Model.cs";
                string strContractPath = @"C:\CodeREsharper\" + "I" + _nGReSharperContext._intermediateModel.BLClassName + ".cs";
                string strDALUnitTestPath = @"C:\CodeREsharper\" +  _nGReSharperContext._intermediateModel.BLClassName + "DataAccessUnitTest.cs";

                if (!Directory.Exists(@"C:\CodeREsharper\"))
                    Directory.CreateDirectory(@"C:\CodeREsharper\");

                File.WriteAllText(@strBLPath, SourceEngineData);
                strFiles = strBLPath;
                if (_SPtoCSrulemodel.AddDataAccessLogic)
                {
                    File.WriteAllText(strDALPath, SourceDataAccessData);
                    strFiles = strFiles + " " + strDALPath;
                }

                if (_SPtoCSrulemodel.AddModelLogic)
                {
                    File.WriteAllText(strModelPath, SourceModelData);
                    strFiles = strFiles + " " + strModelPath;
                }

                if (_SPtoCSrulemodel.AddContractLogic)
                {
                    File.WriteAllText(strContractPath, SourceContractData);
                    strFiles = strFiles + " " + strContractPath;
                }
                if (_SPtoCSrulemodel.AddAUTforDALLogic)
                {
                    File.WriteAllText(strDALUnitTestPath, SourceAUTforDALData);
                    strFiles = strFiles + " " + strDALUnitTestPath;
                }
            }
            return strFiles;
        }
        #endregion
    }
}



