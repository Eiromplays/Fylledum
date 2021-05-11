using System;
using System.Reflection;

namespace Story
{
    public class StoryMethod
    {
        public string storyQuestion;

        public MethodInfo methodInfo;

        public Type type;

        public object initiatedObject;

        public object[] parameters;
    }
}