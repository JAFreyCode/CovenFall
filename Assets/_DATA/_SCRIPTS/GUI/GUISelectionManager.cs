using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NSG
{
    public class GUISelectionManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public List<Selectable> buttons = new List<Selectable>();

        private void Start()
        {
            Selectable[] foundButtons = FindObjectsByType<Selectable>(FindObjectsSortMode.None);
            buttons.AddRange(foundButtons);
        }

        private void Update()
        {
            CheckSelectedButton();
        }

        private void CheckSelectedButton()
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject == null)
                return;

            if (!selectedObject.GetComponentInParent<Selectable>() && !selectedObject.GetComponent<Selectable>() && !selectedObject.GetComponentInChildren<Selectable>())
                return;

            RectTransform selectionProtocol = selectedObject.transform.Find("ButtonSelection") as RectTransform;

            if (selectionProtocol == null)
                return;

            selectionProtocol.gameObject.SetActive(true);
        }

        private Selectable FindSelectable(GameObject obj)
        {
            Selectable selectable = obj.GetComponent<Selectable>();

            if (selectable == null)
                selectable = obj.GetComponentInParent<Selectable>();

            if (selectable == null)
                selectable = obj.GetComponentInChildren<Selectable>();

            return selectable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Selectable selectable = FindSelectable(eventData.pointerEnter?.gameObject);

            if (selectable != null)
            {
                selectable.Select();
                EnableHighlights();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current == null)
                return;

            EventSystem.current.SetSelectedGameObject(null);
        }

        private void EnableHighlights()
        {
            GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

            if (selectedObject == null)
                return;

            RectTransform selectionProtocol = selectedObject.transform.Find("ButtonSelection") as RectTransform;

            if (selectionProtocol == null)
                return;

            selectionProtocol.gameObject.SetActive(true);
        }
    }
}
