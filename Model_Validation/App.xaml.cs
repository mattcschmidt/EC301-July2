using Autofac;
using Model_Validation.Services;
using Model_Validation.Startup;
using Model_Validation.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VMS.TPS.Common.Model.API;

[assembly: ESAPIScript(IsWriteable = true)]
namespace Model_Validation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //access Eclipse.
            VMS.TPS.Common.Model.API.Application app;
            try
            {
                app = new EclipseAccess().AccessEclipse();
            }
            catch
            {
                throw new ApplicationException("Application could not connect");
            }
            //MessageBox.Show(app.PatientSummaries.Count().ToString());
            //Launch the application.
            var bs = new Bootstrapper();
            var container = bs.Bootstrap(app);
            var mainView = container.Resolve<MainView>();
            mainView.ShowDialog();
        }
    }
}
