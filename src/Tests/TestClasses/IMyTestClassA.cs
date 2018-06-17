//Copyright (c) 2008 Nigel Thorne
namespace NDependencyInjection.Tests.TestClasses
{
    public interface IMyTestClassA
    {
        void DoSomething();
    }

    public interface ITestListener
    {
        void OnEvent();
    }

    public class TestListener: ITestListener
    {
        public string log = "";
        public void OnEvent()
        {
            log += "Called";
        }
    }
}