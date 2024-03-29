﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace Model_Validation.ViewModels
{
    public class MainViewModel
    {
        private Application _app;
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public PatientViewModel PatientViewModel { get; }
        public ProfileViewModel ProfileViewModel { get; }

        public MainViewModel(Application app,
            PatientViewModel patientViewModel,
            ProfileViewModel profileViewModel)
        {
            _app = app;
            Title = $"Model Validation; {_app.CurrentUser}";
            PatientViewModel = patientViewModel;
            ProfileViewModel = profileViewModel;
        }

        internal void OnCloseApplication()
        {
            _app.ClosePatient();
            _app.Dispose();
        }
    }
}
