using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Story;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class StoryManager : MonoBehaviour
    {
        public static StoryManager instance;

        public List<Story.Story> stories = new List<Story.Story>();

        public List<Story.Story> availableStories = new List<Story.Story>();

        public List<Story.Story> loadedStories = new List<Story.Story>();

        public List<Story.Story> chosenStories = new List<Story.Story>();

        public GameObject storyUiPrefab;

        public GameObject storyUiSpawn;

        public TextMeshProUGUI storyUiInformation;

        public GameObject storyUi;

        public List<StoryMethod> cachedMethods = new List<StoryMethod>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CacheMethods();

            availableStories = stories.ToList();
            storyUi.SetActive(false);
        }

        private void CacheMethods()
        {
            foreach (var story in stories)
            {
                var type = Type.GetType(story.className);
                if (type == null) continue;

                var method = type.GetMethod(story.methodName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                    BindingFlags.NonPublic);
 
                if (method == null) continue;

                List<object> parameters = new List<object>();

                ConvertValuesToType(method, story, parameters);

                var initiatedObject = Activator.CreateInstance(type);

                AddCacheMethod(initiatedObject, method, type, story, parameters);
            }
        }

        private static void ConvertValuesToType(MethodInfo method, Story.Story story, List<object> parameters)
        {
            for (var i = 0; i < method.GetParameters().Length; i++)
            {
                var parameter = method.GetParameters()[i];
                var storyParam = story.parameters.ElementAtOrDefault(i);
                if (storyParam == null) continue;

                parameters.Add(Convert.ChangeType(storyParam, parameter.ParameterType));
            }
        }

        private void AddCacheMethod(object initiatedObject, MethodInfo method, Type type, Story.Story story, List<object> parameters)
        {
            cachedMethods.Add(new StoryMethod
            {
                initiatedObject = initiatedObject,
                methodInfo = method,
                type = type,
                storyQuestion = story.question,
                parameters = parameters.ToArray()
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Cursor.lockState = Cursor.lockState != CursorLockMode.None
                    ? CursorLockMode.None
                    : CursorLockMode.Locked;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                LoadStories(true);
            }
        }

        public void LoadStories(bool loadAll = false, int amount = 2)
        {
            WipeStories();
            if (UpdateStoriesUi()) return;

            if (loadAll)
            {
                amount = availableStories.Count;
            }

            loadedStories = availableStories.Take(amount).ToList();

            GenerateStoriesUi(amount);
        }

        private void GenerateStoriesUi(int amount)
        {
            foreach (var story in availableStories.Take(amount).ToList())
            {
                var newStoryUi = Instantiate(storyUiPrefab, storyUiSpawn.transform, false);

                SetStoryUiText(newStoryUi, story.question);

                newStoryUi.name = story.question;

                availableStories.Remove(story);
            }
        }

        private bool UpdateStoriesUi()
        {
            storyUi.SetActive(true);
            if (!availableStories.Any())
            {
                storyUiInformation.text = $"No More Stories to load";
                storyUiInformation.color = Color.red;
                return true;
            }

            storyUiInformation.text = "Choose one";
            storyUiInformation.color = Color.white;
            return false;
        }

        public void WipeStories()
        {
            foreach (Transform ui in storyUiSpawn.transform)
            {
                Destroy(ui.gameObject);
            }
        }

        private void SetStoryUiText(GameObject ui, string text)
        {
            ui.transform.Find("Story Question").GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}