
using System;

namespace XFrame.Modules.Plots
{
    internal partial class Story
    {
        private class SectionInfo
        {
            public int Index;
            public ISection Section;
            public SectionState State;
            public IPlotDataProvider Data;

            public SectionInfo(int index, ISection section, SectionState state, IPlotDataProvider data)
            {
                Index = index;
                Section = section;
                State = state;
                Data = data;
            }

            public void SetFinishData()
            {
                Data.SetSectionFinish(Index, true);
            }

            public bool CheckFinishData()
            {
                return Data.CheckSectionFinish(Index);
            }
        }
    }
}
