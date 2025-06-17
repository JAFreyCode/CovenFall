using System.Collections;
using TMPro;
using UnityEngine;

namespace NSG
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Death PopUp")]
        [SerializeField] GameObject deathPopUp;
        [SerializeField] TextMeshProUGUI deathPopUpText;
        [SerializeField] CanvasGroup deathPopUpCanvasGroup;

        public void SendYouHaveBeenCursedPopUp()
        {
            deathPopUp.SetActive(true);

            StartCoroutine(DeathPopUpFadeInOverTime(deathPopUpCanvasGroup, 4));
            StartCoroutine(DeathPopUpFadeOutOverTime(deathPopUpCanvasGroup, 2, 8));
        }

        private IEnumerator DeathPopUpFadeInOverTime(CanvasGroup canvasGroup, float duration)
        {
            if (duration > 0)
            {
                canvasGroup.alpha = 0;
                float timer = 0;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Mathf.Clamp01(timer / duration));
                    yield return null;
                }
            }

            canvasGroup.alpha = 1;
        }

        private IEnumerator DeathPopUpFadeOutOverTime(CanvasGroup canvasGroup, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                float timer = 0;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Mathf.Clamp01(timer / duration));
                    yield return null;
                }
            }

            canvasGroup.alpha = 0;
            deathPopUp.SetActive(false);
        }
    }
}
