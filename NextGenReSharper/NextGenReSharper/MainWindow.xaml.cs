using System.Windows;
using NextGenReSharper.UserControl;
using NextGen.Models.NGReSharper;
using Autofac;

namespace NextGenReSharper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private ContainerBuilder builder;
        public MainWindow()
        {
            InitializeComponent();

            //initialize the container here since we need to dispose when done
            builder = new ContainerBuilder();


            //Register components here

            var container = builder.Build();

            var context = new NGReSharperContext(container);

            NextgenResharperViewControl.DataContext = context.CurrentContext;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow awindow = new AboutWindow();

            awindow.ShowDialog();
        }
    }
}
