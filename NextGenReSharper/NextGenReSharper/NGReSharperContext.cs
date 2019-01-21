using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System;
using Autofac;
using NextGenReSharper.ViewModel;
using NextGen.Contract.NGReSharper;
using NextGen.Manager.NGReSharper;
using NextGen.Models.NGReSharper;
using NextGen.Engine.Logger;

namespace NextGenReSharper
{
    public class NGReSharperContext
    {
        IContainer _container;
        public IManager ngResharperManager;
        public IntermediateModel _intermediateModel;
        public Logger _logger;
        public ConfigModel configModel;
        public NGReSharperContext(IContainer container)
        {
            _container = container;
            _intermediateModel = new IntermediateModel();
            _logger = new Logger();
            configModel = new ConfigModel();
            _intermediateModel.configModel = configModel;
            ngResharperManager = new NGResharperManager(_intermediateModel, _logger);
        }

        public NextGenResharperViewmodel CurrentContext {
            get
            {
                return (new NextGenResharperViewmodel(this));
            }
        }

        public async void ShowMessage(string Title, string message)
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;

            await metroWindow.ShowMessageAsync(Title, message, MessageDialogStyle.Affirmative, metroWindow.MetroDialogOptions);
        }
    }
}
