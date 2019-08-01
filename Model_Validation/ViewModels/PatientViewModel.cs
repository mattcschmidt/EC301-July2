using Model_Validation.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

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
            set {
                SetProperty(ref selectedPlan, value);
                CalculateDoseProfiles();
            }
        }

        private IEventAggregator _eventAggregator;
        private Application _app;
        private Patient patient;
        private Course course;
        private PlanSetup planSetup;
        private string fieldSizes;

        public string FieldSizes
        {
            get { return fieldSizes; }
            set { SetProperty(ref fieldSizes, value); }
        }
        public DelegateCommand CalculatePlanCommand { get; private set; }
        private string doseProfileData;

        public string DoseProfileData
        {
            get { return doseProfileData; }
            set { SetProperty(ref doseProfileData, value); }
        }

        public PatientViewModel(Application app,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _app = app;
            OpenPatientCommand = new DelegateCommand(OnOpenPatient);
            Courses = new ObservableCollection<string>();
            PlanSetups = new ObservableCollection<string>();

            CalculatePlanCommand = new DelegateCommand(OnCalculatePlan, CanCalculatePlan);
            FieldSizes = "2;4;6;8;10;20";
        }

        private void OnCalculatePlan()
        {
            patient.BeginModifications();
            Course course_temp = patient.AddCourse();
            ExternalPlanSetup plan_temp = course_temp.
                AddExternalPlanSetup(patient.StructureSets.FirstOrDefault());
            ExternalBeamMachineParameters exBeamParams = new ExternalBeamMachineParameters(
                "HESN10",
                "6X",
                600,
                "STATIC",
                null);
            foreach(string fs in FieldSizes.Split(';'))
            {
                double fsd = Convert.ToDouble(fs);
                plan_temp.AddStaticBeam(exBeamParams,
                    new VRect<double>(-fsd / 2 * 10, -fsd / 2 * 10, fsd / 2 * 10, fsd / 2 * 10),
                    0,
                    0,
                    0,
                    new VVector(0, -200, 0));
            }
            plan_temp.SetPrescription(1, new DoseValue(100, DoseValue.DoseUnit.cGy), 1);
            plan_temp.CalculateDose();
            
            _app.SaveModifications();
            AddCourses(patient);
            SelectedCourse = course_temp.Id;
            SelectedPlan = plan_temp.Id;
        }

        private bool CanCalculatePlan()
        {
            //this is only going to calculate on the first structure set found.
            return patient != null && patient.StructureSets.FirstOrDefault() != null;
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
                    CalculatePlanCommand.RaiseCanExecuteChanged();
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
        private void CalculateDoseProfiles()
        {
            if (SelectedPlan != null)
            {
                planSetup = course.PlanSetups.FirstOrDefault(x => x.Id == SelectedPlan);
                double[] depths = new double[] { 15, 50, 100, 200, 300 };//mm
                List<DoseProfile> profiles = new List<DoseProfile>();
                foreach (Beam b in planSetup.Beams)
                {
                    foreach (double d in depths)
                    {
                        double x1 = b.ControlPoints.First().JawPositions.X1;
                        double x2 = b.ControlPoints.First().JawPositions.X2;
                        VVector start = new VVector(x1 - 50, d - 200, 0);
                        VVector end = new VVector(x2 + 50, d - 200, 0);
                        DoseProfile prof = b.Dose.GetDoseProfile(
                            start, end, new double[Convert.ToInt16(end.x - start.x + 1)]);
                        DoseProfileData += $"Profile - d {d}mm - FS {(x2 - x1) / 10}cm\n";
                        profiles.Add(prof);
                    }
                }
                _eventAggregator.GetEvent<UpdatePlotEvent>().Publish(profiles);
            }
        }
    }
}
