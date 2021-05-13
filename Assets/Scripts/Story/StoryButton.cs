using System;
using Managers;
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

            var storyMethod = StoryManager.instance.CachedMethods.FirstOrDefault(m =>
                m.storyQuestion.Equals(story.question, StringComparison.OrdinalIgnoreCase));

            StoryManager.instance.storyUi.SetActive(false);

            storyMethod?.methodInfo.Invoke(storyMethod.initiatedObject, storyMethod.parameters);
        }
    }
}