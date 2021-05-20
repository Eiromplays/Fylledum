using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraMethod
    {
        [InspectorName("MethodName")]
        public string methodName;

        [InspectorName("ClassName")]
        public string className;

        [InspectorName("Parameters")]
        public List<string> parameters = new List<string>();
    }
}
