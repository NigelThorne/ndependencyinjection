using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AmmySidekick;
using NDependencyInjection;
using NUnit.Framework;

namespace NDependencyInjectionExampleApplication
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            EventLog log = new EventLog();


            App app = new App();
            app.InitializeComponent();

            RuntimeUpdateHandler.Register(app, "/" + AmmySidekick.Ammy.GetAssemblyName(app) + ";component/App.g.xaml");

            app.Run();
        }

    }

    /*
    TODO: Show user time since last allocation
    TODO: Default behaviour should be to add time to the existing allocation
    TODO: Submitting the form with the note changed will add to the existing allocation
    TODO: Submitting the form with the goal changed will create a new allocation 
    TODO: If you type minutes that sets it. 
    TODO: If the field is clear, populate it with number of unallocated minutes. 

    TODO: Setting 'to' to it's max value ( or within a minute ) means I will move with 
    TODO: 

    */


    [TestFixture]
    public class Test
    {
        [Test]
        public void Controller()
        {
            var uiController = new UIController();

            var uiModel = new UIModel();
            
        }

        [Test]
        public void TestLogCanStoreEvents()
        {
            EventLog log = new EventLog();
            DateTime start = new DateTime();
            DateTime end = start.AddMinutes(3);
            log.RecordEvent(start, end, "Activity text");
        }

    }

    public class UIModel
    {
    }

    public class UIController
    {
    }

    internal class EventLog  
    {
        public void RecordEvent(DateTime start, DateTime end, string activityText)
        {


            throw new NotImplementedException();
        }
    }
}
