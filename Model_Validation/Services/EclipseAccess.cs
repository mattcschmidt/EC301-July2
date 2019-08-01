using VMS.TPS.Common.Model.API;

namespace Model_Validation.Services
{
    public class EclipseAccess
    {
        /// <summary>
        ///  
        /// </summary>
        /// <returns>instance of an Eclipse application.</returns>
        public Application AccessEclipse()
        {
            return Application.CreateApplication();
        }
    }
}
