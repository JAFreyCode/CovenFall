using UnityEngine;
using UnityEngine.UI;

namespace NSG
{
    public class UI_StatBar : MonoBehaviour
    {
        [Header("StatBar Data")]
        public Slider slider;
        public RectTransform rectTransform;

        [Header("Bar Options")]
        public bool scaleBarLengthsWithStats = true;
        public float widthScaleMultiplier = 1;
        
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (!scaleBarLengthsWithStats) return;

            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            PlayerUIManager._Singleton.playerUIHudManager.RefreshHUD();
        }
    }
}
