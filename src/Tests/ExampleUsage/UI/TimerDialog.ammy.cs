using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDependencyInjection.Tests.ExampleUsage.UI
{
    public partial class TimerDialog : ITimerDialog, IStartListener
    {
        public TimerDialog(TimerViewModel model)
        {
            InitializeComponent();
        }

        public void ShowDialog()
        {
        }

        public void OnStart()
        {
            
        }
    }
}
