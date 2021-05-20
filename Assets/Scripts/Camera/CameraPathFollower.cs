using Assets.Scripts.Camera;
using Assets.Scripts.Helpers;
using Assets.Scripts.Loading;
using Story;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Camera
{
    public class CameraPathFollower : MonoBehaviour
    {
		public float speed = 3f;
        public Transform pathParent;
        private Transform _targetPoint;
        private int _index;

        public bool restartAtEnd;
        public bool hideCameraAtEnd;

        private Vector3 _startPos;
        private Quaternion _startRotation;

        public GameObject fadeOnRestartUi;
        public bool showFadeOnRestart;
        public float delayRestart;

        public bool backToMainMenu;

        public int repeat;

        public List<GameObject> objectsToSetActive = new List<GameObject>();

        public List<GameObject> objectsToHide = new List<GameObject>();

        public List<CameraFollowOptions> cameraFollowOptions = new List<CameraFollowOptions>();

        public List<CameraMethod> cameraMethods = new List<CameraMethod>();

        public List<StoryMethod> MethodsToRunWhenComplete = new List<StoryMethod>();

        private void OnDrawGizmos()
        {
            for (int a = 0; a < pathParent.childCount; a++)
            {
                var from = pathParent.GetChild(a).position;
                var to = pathParent.GetChild((a + 1) % pathParent.childCount).position;
                Gizmos.color = new Color(1, 0, 0);
                Gizmos.DrawLine(from, to);
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            CacheMethods();

            _startRotation = transform.rotation;
            _startPos = transform.position;
            _index = 0;
            _targetPoint = pathParent.GetChild(_index);
        }

        private void CacheMethods()
        {
            foreach (var cameraMethod in cameraMethods)
            {
                var type = Type.GetType(cameraMethod.className);
                if (type == null) continue;

                var method = type.GetMethod(cameraMethod.methodName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance |
                    BindingFlags.Static | BindingFlags.NonPublic);

                if (method == null) continue;

                List<object> parameters = new List<object>();

                ConvertValuesToType(method, cameraMethod, parameters);

                var initiatedObject = Activator.CreateInstance(type);

                AddCacheMethod(initiatedObject, method, type, parameters);
            }
        }

        private void ConvertValuesToType(MethodInfo method, CameraMethod cameraMethod, List<object> parameters)
        {
            for (var i = 0; i < method.GetParameters().Length; i++)
            {
                var parameter = method.GetParameters()[i];
                var storyParam = cameraMethod.parameters.ElementAtOrDefault(i);
                if (storyParam == null) continue;

                parameters.Add(Convert.ChangeType(storyParam, parameter.ParameterType));
            }
        }

        private void AddCacheMethod(object initiatedObject, MethodInfo method, Type type, List<object> parameters)
        {
            MethodsToRunWhenComplete.Add(new StoryMethod
            {
                initiatedObject = initiatedObject,
                methodInfo = method,
                type = type,
                parameters = parameters.ToArray()
            });
        }

        private void RestartGame()
        {
            StartCoroutine(DelayBeforeRestart());
        }

        private IEnumerator DelayBeforeRestart()
        {
            if (showFadeOnRestart && fadeOnRestartUi != null)
            {
                fadeOnRestartUi.SetActive(true);
            }

            yield return new WaitForSeconds(delayRestart);
            _index = 0;
            _targetPoint = pathParent.GetChild(_index);
        }

        // Update is called once per frame

        public int repeated;
        private bool _loadingScene;
        private void Update()
        {
            if (_index == pathParent.childCount || repeated >= repeat && repeat > 0)
            {
                if (backToMainMenu && repeated >= repeat && repeat > 0 && !_loadingScene)
                {
                    _loadingScene = true;

                    LoadingHelper.LoadScene((int)SceneIndexes.MainMenu);
                    
                    return;
                }

                if (RestartAtEnd()) return;

                transform.SetPositionAndRotation(_startPos, _startRotation);
                RestartGame();
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, speed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _targetPoint.position) < 0.1f)) return;
            _index++;
            if (_index == pathParent.childCount)
            {
                repeated++;
                return;
            }
            _targetPoint = pathParent.GetChild(_index);

            var cameraFollowOption = cameraFollowOptions.FirstOrDefault(o => o.pathIndex == _index);

            if (cameraFollowOption != null)
            {
                transform.eulerAngles = cameraFollowOption.axis;
            }
        }

        private bool RestartAtEnd()
        {
            if (restartAtEnd) return false;

            foreach (var objectToSetActive in objectsToSetActive)
            {
                objectToSetActive.SetActive(true);
            }

            foreach (var objectToHide in objectsToHide)
            {
                objectToHide.SetActive(false);
            }

            if (hideCameraAtEnd)
            {
                gameObject.SetActive(false);
            }

            foreach (var method in MethodsToRunWhenComplete)
            {
                method?.methodInfo.Invoke(method.initiatedObject, method.parameters);
            }

            return true;
        }
    }
}