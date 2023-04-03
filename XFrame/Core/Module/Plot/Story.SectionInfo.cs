﻿
namespace XFrame.Modules.Plots
{
    public partial class Story
    {
        private class SectionInfo
        {
            public ISection Section;
            public SectionState State;

            public SectionInfo(ISection section, SectionState state)
            {
                Section = section;
                State = state;
            }
        }
    }
}
