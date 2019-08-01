using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.Types;

namespace Model_Validation.Events
{
    public class UpdatePlotEvent:PubSubEvent<List<DoseProfile>>
    {
    }
}
