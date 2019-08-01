using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace Model_Validation.ViewModels
{
    public class PatientViewModel : BindableBase
    {
        private string patientId;

        public string PatientId
        {
            get { return patientId; }
            set { SetProperty(ref patientId,value); }
        }
        public DelegateCommand OpenPatientCommand { get; private set; }
        public ObservableCollection<string> Courses { get; private set; }
        private string selectedCourse;

        public string SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                SetProperty(ref selectedCourse, value);
                AddPlanSetups();
            }
        }

        public ObservableCollection<string> PlanSetups { get; private set; }
        private string selectedPlan;

        public string SelectedPlan
        {
            get { return selectedPlan; }
            set { SetProperty(ref selectedPlan, value); }
        }
        private Application _app;
        private Patient patient;
        private Course course;
        private PlanSetup planSetup;

        public PatientViewModel(Application app)
        {
            _app = app;
            OpenPatientCommand = new DelegateCommand(OnOpenPatient);
            Courses = new ObservableCollection<string>();
            PlanSetups = new ObservableCollection<string>();
        }

        private void OnOpenPatient()
        {
            if (!String.IsNullOrEmpty(PatientId))
            {
                //open the patient.
                _app.ClosePatient();
                patient = _app.OpenPatientById(PatientId);
                Courses.Clear();
                //fill the courses.
                if (patient != null)
                {
                    AddCourses(patient);
                }
            }
        }

        private void AddCourses(Patient patient)
        {            
            foreach (Course c in patient.Courses)
            {
                Courses.Add(c.Id);
            }
        }

        private void AddPlanSetups()
        {
            PlanSetups.Clear();
            if(SelectedCourse != null)
            {
                course = patient.Courses.SingleOrDefault(x => x.Id == SelectedCourse);
                foreach(PlanSetup ps in course.PlanSetups)
                {
                    PlanSetups.Add(ps.Id);
                }
            }
        }
    }
}
