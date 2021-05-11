using UnityEngine;

namespace Story
{
    public class StoryTest
    {
        public void Test(int test, string yo)
        {
            Debug.Log($"This is amazing. {test}");

            if (yo != null)
            {
                Debug.Log($"Yo {yo}");
            }
        }

        public void NoParams()
        {
            Debug.Log($"No Params");
        }

        private void Parameters(string iamCool, bool gang, float number)
        {
            Debug.Log($"{iamCool} {gang} {number}");
        }
    }
}