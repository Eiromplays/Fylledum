using System;
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
    }
}
