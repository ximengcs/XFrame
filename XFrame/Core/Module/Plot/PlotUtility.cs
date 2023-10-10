﻿using System;
using System.Collections.Generic;
using XFrame.Core;
using XFrame.Modules.Archives;
using XFrame.Modules.Reflection;
using XFrame.SimpleJSON;

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
            node.Add("sections", sections);
            Type[] types = story.GetSections();
            foreach (Type type in types)
                sections.Add(type.FullName);
        }

        public static IStory[] InnerRestoreStories(JsonArchive archive)
        {
            List<IStory> stories = new List<IStory>();
            JSONObject map = archive.GetOrNewObject("stories");
            var it = map.GetEnumerator();
            while (it.MoveNext())
            {
                var item = it.Current;
                IStory story = XModule.Plot.NewStory(item.Key);
                JSONNode sections = item.Value["sections"];
                foreach (JSONNode section in sections)
                {
                    Type type = XModule.Type.GetType(section);
                    story.AddSection(type);
                }
                stories.Add(story);
            }
            return stories.ToArray();
        }
    }
}
