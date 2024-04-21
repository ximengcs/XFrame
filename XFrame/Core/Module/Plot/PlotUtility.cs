using System;
using XFrame.Core;
using XFrame.SimpleJSON;
using XFrame.Modules.Archives;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    internal class PlotUtility
    {
        public static string InnerGetStorySaveName(string storyName)
        {
            return $"plot_{storyName}";
        }

        public static void InnerRemoveStoryState(JsonArchive archive, string storyName)
        {
            JSONObject map = archive.GetOrNewObject("stories");
            map.Remove(storyName);
        }

        public static void InnerRecordStoryState(JsonArchive archive, IStory story)
        {
            JSONObject map = archive.GetOrNewObject("stories");
            JSONObject node = new JSONObject();
            map.Add(story.Name, node);
            JSONArray sections = new JSONArray();
            node.Add("story_type", story.Director.GetType().FullName);

            IStoryHelper helper = story.Helper;
            if (helper != null)
                node.Add("story_helper_type", story.Helper.GetType().FullName);
            node.Add("sections", sections);
            foreach (ISection section in story.Sections)
                sections.Add(section.GetType().FullName);
        }

        public static IStory[] InnerRestoreStories(IPlotModule module, JsonArchive archive)
        {
            List<IStory> stories = new List<IStory>();
            JSONObject map = archive.GetOrNewObject("stories");
            var it = map.GetEnumerator();
            while (it.MoveNext())
            {
                var item = it.Current;
                JSONNode storyTypeNode = item.Value["story_type"];
                Type storyHelperType = null;
                if (item.Value.HasKey("story_helper_type"))
                {
                    JSONNode storyHelperTypeNode = item.Value["story_helper_type"];
                    storyHelperType = module.Domain.TypeModule.GetType(storyHelperTypeNode);
                }
                Type storyType = module.Domain.TypeModule.GetType(storyTypeNode);
                IStory story = module.NewStory(storyType, storyHelperType, item.Key);
                JSONNode sections = item.Value["sections"];
                foreach (JSONNode section in sections)
                {
                    Type type = module.Domain.TypeModule.GetType(section);
                    if (type != null)
                        story.AddSection(type);
                }
                stories.Add(story);
            }
            return stories.ToArray();
        }
    }
}
