using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Plots;

namespace XFrameTest
{
    public class Section1 : ISection
    {
        public bool IsDone { get; private set; }

        public IDataProvider Data => throw new NotImplementedException();

        public IStory Story => throw new NotImplementedException();

        public bool CanStart()
        {
            Log.Debug("CanStart" + GetHashCode());
            return true;
        }

        public bool OnFinish()
        {
            Log.Debug("OnFinish" + GetHashCode());
            return true;
        }

        public void OnCreate(IStory story, IDataProvider data)
        {
            Log.Debug("OnInit" + GetHashCode());
        }

        public void OnStart()
        {
            Log.Debug("OnStart" + GetHashCode());
        }

        public void OnUpdate()
        {
            Log.Debug("OnUpdate" + GetHashCode());
            IsDone = true;
        }

        public void OnInit()
        {
            throw new NotImplementedException();
        }
    }

    public class Section2 : ISection
    {
        public bool IsDone { get; private set; }

        public IDataProvider Data => throw new NotImplementedException();

        public IStory Story => throw new NotImplementedException();

        public bool CanStart()
        {
            Log.Debug("None Block CanStart" + GetHashCode());
            return true;
        }

        public bool OnFinish()
        {
            Log.Debug("None Block OnFinish" + GetHashCode());
            return true;
        }

        public void OnCreate(IStory story, IDataProvider data)
        {
            Log.Debug("None Block OnInit" + GetHashCode());
        }

        public void OnStart()
        {
            Log.Debug("None Block OnStart" + GetHashCode());
        }

        public void OnUpdate()
        {
            Log.Debug("None Block OnUpdate" + GetHashCode());
            IsDone = true;
        }

        public void OnInit()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class PlotTest
    {
        [TestMethod]
        public void Test1()
        {

        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {

            });
        }
    }
}
