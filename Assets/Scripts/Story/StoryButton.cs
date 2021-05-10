﻿using Managers;
using System.Linq;
using UnityEngine;

namespace Story
{
    public class StoryButton : MonoBehaviour
    {
        public void SelectStory()
        {
            var story = StoryManager.instance.loadedStories.FirstOrDefault(s => s.question.Equals(name));

            if (story == null) return;

            StoryManager.instance.chosenStories.Add(story);
            StoryManager.instance.WipeStories();
            StoryManager.instance.loadedStories.Clear();

            Debug.Log(JsonUtility.ToJson(story));
        }
    }
}