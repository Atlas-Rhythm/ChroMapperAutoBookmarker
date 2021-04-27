using System.Collections;
using UnityEngine;

namespace ChroMapperAutoBookmarker
{
    public class CoroutineStarter : MonoBehaviour
    {
        private static CoroutineStarter starterObject;

        public static Coroutine Start(IEnumerator coroutine)
        {
            if (starterObject == null)
            {
                starterObject = new GameObject(nameof(CoroutineStarter)).AddComponent<CoroutineStarter>();
                DontDestroyOnLoad(starterObject);
            }
            return starterObject.StartCoroutine(coroutine);
        }
    }
}
