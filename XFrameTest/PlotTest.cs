using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Plots;

namespace XFrameTest
{
    public class Section1 : ISection
    {
        public bool IsDone { get; private set; }

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

        public void OnInit(IDataProvider data)
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
    }

    public class Section2 : ISection
    {
        public bool IsDone { get; private set; }

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

        public void OnInit(IDataProvider data)
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
    }

    [TestClass]
    public class PlotTest
    {
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                IStory[] stories = new IStory[]
                {
                    XModule.Plot.NewStory("story1").AddSection(typeof(Section1)),
                    XModule.Plot.NewStory("story2").AddSection(typeof(Section1))
                };
                XModule.Plot.Helper.Event.Trigger(NewStoryEvent.Create(stories));

                stories = new IStory[]
                {
                    XModule.Plot.NewStory("story3").AddSection(typeof(Section2)),
                    XModule.Plot.NewStory("story4").AddSection(typeof(Section2))
                };
                XModule.Plot.Helper.Event.Trigger(NewStoryEvent.Create(stories, typeof(NonBlockDirector)));
            });
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
