using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NSG
{
    public class ButtonSelectionManager : MonoBehaviour
    {
        private Button thisButton;

        private void Awake()
        {
            thisButton = GetComponent<Button>();
        }

        void Update()
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (!thisButton.transform.Find("ButtonSelection"))
                return;

            if (gameObject != selectedObject)
            {
                RectTransform disableSelection = thisButton.transform.Find("ButtonSelection") as RectTransform;

                disableSelection.gameObject.SetActive(false);
            }
        }
    }
}
