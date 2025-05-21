using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NSG
{
    public class WorldInputDetectionManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static WorldInputDetectionManager Singleton;
        public static WorldInputDetectionManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("Menu Control Hints")]
        public GameObject systemControllerHints;
        public GameObject systemKeyboardHints;
        public GameObject loadGameControllerHints;
        public GameObject loadGameKeyboardHints;

        [Header("Debug Info")]
        public bool controllerActive = false;
        GraphicRaycaster raycaster;

        [Header("Title Screen Canvas")]
        public Canvas titleScreenCanvas;

        [Header("Previously Selected Button")]
        public Selectable lastSelected;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            raycaster = titleScreenCanvas.GetComponent<GraphicRaycaster>();

            SceneManager.activeSceneChanged += OnSceneChange;

            Singleton.enabled = true;
        }

        private void Update()
        {
            StoreLastSelected();
        }

        private void LateUpdate()
        {
            DetectControllerInput();
        }

        private void StoreLastSelected()
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected != null)
            {
                Selectable selectable = selected.GetComponent<Selectable>();

                if  (selectable != null && selectable != lastSelected)
                {
                    lastSelected = selectable;
                }
            }
        }

        private void DetectControllerInput()
        {
            if (Gamepad.current != null && Gamepad.current.allControls.Any(control => control.IsPressed()))
            {
                if (!controllerActive)
                {
                    controllerActive = true;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    raycaster.enabled = false;
                    lastSelected.Select();

                    systemKeyboardHints.SetActive(false);
                    loadGameKeyboardHints.SetActive(false);
                    systemControllerHints.SetActive(true);
                    loadGameControllerHints.SetActive(true);
                }
            }
            else if (Mouse.current.delta.ReadValue() != Vector2.zero)
            {
                if (controllerActive)
                {
                    controllerActive = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    raycaster.enabled = true;

                    systemControllerHints.SetActive(false);
                    loadGameControllerHints.SetActive(false);
                    systemKeyboardHints.SetActive(true);
                    loadGameKeyboardHints.SetActive(true);
                }
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex != NSGUtils.GetWorldSceneIndex(true)) { Singleton.enabled = false; return; }

            Destroy(this);
        }
    }
}
