using Model_Validation.Events;
using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;

namespace Model_Validation.ViewModels
{
    public class ProfileViewModel:BindableBase
    {
        private PlotModel myPlotModel;
        private IEventAggregator _eventAggregator;

        public PlotModel MyPlotModel
        {
            get { return myPlotModel; }
            set { SetProperty(ref myPlotModel, value); }
        }
        public ProfileViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            MyPlotModel = new PlotModel
            {
                Title = "Dose Profiles"
            };
            _eventAggregator.GetEvent<UpdatePlotEvent>().Subscribe(OnUpdatePlot);
        }

        private void OnUpdatePlot(List<DoseProfile> obj)
        {
            if (obj.Count() != 0)
            {
                MyPlotModel.Series.Clear();
                foreach (DoseProfile dp in obj)
                {
                    LineSeries series = new LineSeries();
                    foreach(ProfilePoint profPoint in dp)
                    {
                        series.Points.Add(new DataPoint(profPoint.Position.x, profPoint.Value));
                    }
                    MyPlotModel.Series.Add(series);
                }
                MyPlotModel.InvalidatePlot(true);
            }
        }
    }
}
