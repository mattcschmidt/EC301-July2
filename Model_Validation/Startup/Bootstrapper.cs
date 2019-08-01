using Autofac;
using Model_Validation.ViewModels;
using Model_Validation.Views;
using VMS.TPS.Common.Model.API;

namespace Model_Validation.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap(Application app)
        {
            var builder = new ContainerBuilder();
            //view
            builder.RegisterType<MainView>().AsSelf();
            //viewmodels.
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<PatientViewModel>().AsSelf();
            //esapi stuff
            builder.RegisterInstance<Application>(app);
            return builder.Build();
        }
    }
}
