using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NSG
{
    public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previouslySelected;
        [SerializeField] RectTransform currentSelectedTransform;

        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] Scrollbar scrollbar;
        [SerializeField] float smoothTime = 0.1f;
        [SerializeField] float centeringOffset = 0.5f; // Adjust this value to control the centering offset

        private float targetScrollPosition;
        private float velocity = 0f;

        private void Update()
        {
            if (WorldInputDetectionManager._Singleton.controllerActive)
            {
                ControllerScroll();   
            }
            else
            {
                MouseScroll();
            }
        }

        private void LateUpdate()
        {
            if (!WorldInputDetectionManager._Singleton.controllerActive) { return; }

            // Smoothly move to the target scroll position
            scrollRect.horizontalNormalizedPosition = Mathf.SmoothDamp(
                scrollRect.horizontalNormalizedPosition, targetScrollPosition, ref velocity, smoothTime);
        }

        private void ControllerScroll()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == null || currentSelected == previouslySelected) return;

            previouslySelected = currentSelected;
            currentSelectedTransform = currentSelected.GetComponent<RectTransform>();

            targetScrollPosition = GetNormalizedScrollPosition(currentSelectedTransform);
        }

        private void MouseScroll()
        {
            if (!TitleScreenManager._Singleton.loadGameMenu.activeSelf) { return; }

            scrollbar.value += TitleScreenLoadMenuInputManager._Singleton.scrollValue.y * Time.deltaTime;

            scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
        }

        private float GetNormalizedScrollPosition(RectTransform target)
        {
            float contentWidth = contentPanel.rect.width;
            float viewportWidth = scrollRect.viewport.rect.width;

            // Get local position of target within the content panel
            float targetX = target.anchoredPosition.x;

            // Adjust for half of the viewport to center the button properly, and add centering offset
            float centeredPosition = targetX - (viewportWidth / 2) + (target.rect.width / 2) + (viewportWidth * centeringOffset);

            // Normalize the position within the ScrollRect
            float normalizedPosition = Mathf.Clamp01(centeredPosition / (contentWidth - viewportWidth));

            return normalizedPosition;
        }
    }
}
