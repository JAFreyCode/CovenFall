using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class NSGUtils
    {
        public static void SingletonCheck<T>(ref T instance, MonoBehaviour script) where T : MonoBehaviour
        {
            if (instance == null)
            {
                instance = script as T;
            }
            else if (instance != null)
            {
                UnityEngine.Object.Destroy(script.gameObject);
            }
        }

        public static int GetWorldSceneIndex(bool getCurrentScene = false)
        {
            if (getCurrentScene)
                return SceneManager.GetActiveScene().buildIndex;

            return SceneManager.GetActiveScene().buildIndex + 1;
        }

        public static LayerMask GetGroundLayers()
        {
            return LayerMask.GetMask("Default");
        }
    }
}
