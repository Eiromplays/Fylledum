using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Loading;
using UnityEngine;

namespace Camera
{
    public class CameraPathFollower : MonoBehaviour
    {
		public float speed = 3f;
        public Transform pathParent;
        Transform targetPoint;
        int index;

        public bool restartAtEnd;
        public bool hideCameraAtEnd;

        private Vector3 startPos;
        private Quaternion startRotation;

        public GameObject fadeOnRestartUi;
        public bool showFadeOnRestart;
        public float delayRestart;

        public bool backToMainMenu;

        public int repeat;

        public List<GameObject> objectsToSetActive = new List<GameObject>();

        public List<GameObject> objectsToHide = new List<GameObject>();

        public List<CameraFollowOptions> cameraFollowOptions = new List<CameraFollowOptions>();

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
            startRotation = transform.rotation;
            startPos = transform.position;
            index = 0;
            targetPoint = pathParent.GetChild(index);
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
            index = 0;
            targetPoint = pathParent.GetChild(index);
        }

        // Update is called once per frame

        public int repeated;
        private void Update()
        {
            if (index == pathParent.childCount || repeated >= repeat && repeat > 0)
            {
                if (backToMainMenu)
                {
                    LoadingHelper.LoadScene((int)SceneIndexes.MainMenu);
                    return;
                }

                if (RestartAtEnd()) return;

                transform.SetPositionAndRotation(startPos, startRotation);
                RestartGame();
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, targetPoint.position) < 0.1f)) return;
            index++;
            if (index == pathParent.childCount)
            {
                repeated++;
                return;
            }
            targetPoint = pathParent.GetChild(index);

            var cameraFollowOption = cameraFollowOptions.FirstOrDefault(o => o.pathIndex == index);

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

            return true;
        }
    }
}