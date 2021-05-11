using System;
using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    [Serializable]
    public class Story
    {
        [InspectorName("Question")]
        public string question;

        [InspectorName("Chosen")]
        public bool chosen;

        [InspectorName("MethodName")]
        public string methodName;

        [InspectorName("ClassName")]
        public string className;

        [InspectorName("Parameters")]
        public List<string> parameters = new List<string>();
    }
}