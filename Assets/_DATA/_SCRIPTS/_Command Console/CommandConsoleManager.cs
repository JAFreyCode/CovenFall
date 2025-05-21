using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSG
{
    public class CommandConsoleManager : MonoBehaviour
    {
        private static CommandConsoleManager Singleton;
        public static CommandConsoleManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        public PlayerManager player;

        [Header("Console Data")]
        public GameObject consoleContainer;
        public TMP_InputField commandInput;
        public RectTransform consoleOutput;
        public TMP_Text autocompleteText;
        public GameObject consoleOutputPrefab;

        private readonly Dictionary<string, (string Description, Action<string> Action)> commandActions = new Dictionary<string, (string, Action<string>)>();
        private readonly List<GameObject> consoleResponses = new List<GameObject>();

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            InitializeCommands();
            autocompleteText.text = string.Empty;
        }

        private void Update()
        {
            if (!NetworkManager.Singleton.IsHost) { return; }

            if (SceneManager.GetActiveScene().buildIndex == 0)
                return;

            HandleConsoleToggle();
            HandleAutocompletePreview();
            HandleAutocomplete();
        }

        private void HandleConsoleToggle()
        {
            if (Input.GetKeyDown(KeyCode.Period))
            {
                bool isActive = consoleContainer.activeSelf;
                consoleContainer.SetActive(!isActive);
                commandInput.text = string.Empty;
                Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !isActive;
                WorldInputManager._Singleton.enabled = isActive;

                if (!isActive)
                {
                    commandInput.Select();
                    commandInput.ActivateInputField();
                }
            }
        }

        public void ProcessCommand(string CommandString)
        {
            if (!Input.GetKeyDown(KeyCode.Return)) { return; }

            if (!CommandString.StartsWith("$"))
            {
                CreateConsoleResponse("Invalid Command: Commands Must Start With '$'");
                return;
            }

            string[] parts = CommandString.Substring(1).Trim().Split(' ', 2);
            string command = parts[0].ToLower();
            string arguments = parts.Length > 1 ? parts[1] : string.Empty;

            if (commandActions.TryGetValue(command, out var commandAction))
            {
                commandAction.Action.Invoke(arguments);
            }
            else
            {
                CreateConsoleResponse($"Invalid Command: {command}");
            }

            commandInput.text = string.Empty;
            commandInput.Select();
            commandInput.ActivateInputField();
        }

        private void InitializeCommands()
        {
            commandActions.Add("help", ("Displays detailed information about available commands.", _ => ShowHelp()));
            commandActions.Add("clear", ("Clears the console.", _ => ClearConsole()));
            commandActions.Add("walkingspeed", ("Usage: $walkingspeed [value] - Updates the walking speed of the player.", args => UpdatePlayerSpeed("Walking", args, 1.65f)));
            commandActions.Add("runningspeed", ("Usage: $runningspeed [value] - Updates the running speed of the player.", args => UpdatePlayerSpeed("Running", args, 5f)));
            commandActions.Add("sprintingspeed", ("Usage: $sprintingspeed [value] - Updates the sprinting speed of the player.", args => UpdatePlayerSpeed("Sprinting", args, 7f)));
            commandActions.Add("teleport", ("Usage: $teleport [X value] [Y value] [Z value] - Teleports the player to the specified location", args => TeleportPlayer(args, player.transform.position.ToString())));
            commandActions.Add("endurancelevel", ("Usage: $endurancelevel [value] - Changes the players endurance level.", args => ChangeEnduranceLevel(args, player.playerNetworkManager.endurance.Value)));
            commandActions.Add("whatislove?", ("Do you know what love is.", _ => Application.OpenURL("https://www.youtube.com/watch?v=G8RY_XOqJf4")));
            commandActions.Add("whoisrick?", ("Rickrolls you, of course.", _ => Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ")));
        }

        private void CreateConsoleResponse(string response)
        {
            DateTime systemTime = DateTime.Now;
            string currentSystemTime = "{ " + systemTime.ToString() + " }";

            // Parse the response for syntax highlighting.
            string highlightedResponse = HighlightSyntax(response);

            string finalResponse = currentSystemTime + ' ' + highlightedResponse;

            GameObject responsePrefab = Instantiate(consoleOutputPrefab);
            consoleResponses.Add(responsePrefab);

            RectTransform responseRect = responsePrefab.GetComponent<RectTransform>();
            responseRect.SetParent(consoleOutput);

            TMP_Text textComponent = responsePrefab.GetComponentInChildren<TMP_Text>();
            textComponent.text = finalResponse;
        }

        private void HandleAutocompletePreview()
        {
            if (!consoleContainer.activeSelf) return;

            string inputText = commandInput.text.Trim();
            if (string.IsNullOrEmpty(inputText))
            {
                autocompleteText.text = string.Empty;
                return;
            }

            string match = FindMatchingCommand(inputText);
            if (!string.IsNullOrEmpty(match))
            {
                // Display the suggestion as a faded preview.
                string remainingText = match.Substring(inputText.StartsWith("$") ? inputText.Length - 1 : inputText.Length);
                autocompleteText.text = inputText + remainingText;
            }
            else
            {
                autocompleteText.text = string.Empty;
            }
        }

        private void HandleAutocomplete()
        {
            if (!consoleContainer.activeSelf) return;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                string inputText = commandInput.text.Trim();
                if (string.IsNullOrEmpty(inputText)) return;

                string match = FindMatchingCommand(inputText);
                if (!string.IsNullOrEmpty(match))
                {
                    commandInput.text = $"${match}";
                    commandInput.caretPosition = commandInput.text.Length;
                }
            }
        }

        private string FindMatchingCommand(string input)
        {
            string inputWithoutDollar = input.StartsWith("$") ? input.Substring(1) : input;
            foreach (var command in commandActions.Keys)
            {
                if (command.StartsWith(inputWithoutDollar, StringComparison.OrdinalIgnoreCase))
                {
                    return command;
                }
            }
            return null;
        }

        private string HighlightSyntax(string response)
        {
            response = Regex.Replace(response, @"\[(?<content>[^\]]+)\]", match =>
            {
                return $"<color=#FFA500>[</color><color=#d4d11e>{match.Groups["content"].Value}</color><color=#FFA500>]</color>";
            });

            response = Regex.Replace(response, @"\$\w+", match =>
            {
                return $"<color=#A020F0><b>{match.Value}</b></color>";
            });
            response = Regex.Replace(response, @"\b\d+(\.\d+)?\b", match =>
            {
                return $"<color=#d4d11e>{match.Value}</color>";
            });

            return response;
        }

        #region Command Methods

        private void ShowHelp()
        {
            foreach (var command in commandActions)
            {
                CreateConsoleResponse($"${command.Key} - {command.Value.Description}");
            }
        }

        private void ClearConsole()
        {
            foreach (GameObject commandResponse in consoleResponses)
            {
                Destroy(commandResponse);
            }

            consoleResponses.Clear();
        }

        private void UpdatePlayerSpeed(string type, string args, float defaultSpeed)
        {
            if (float.TryParse(args, out float speed))
            {
                switch (type)
                {
                    case "Walking":
                        player.playerLocomotionManager.walkingSpeed = speed;
                        break;
                    case "Running":
                        player.playerLocomotionManager.runningSpeed = speed;
                        break;
                    case "Sprinting":
                        player.playerLocomotionManager.sprintingSpeed = speed;
                        break;
                }
                CreateConsoleResponse($"Default {type} Speed: {defaultSpeed}. Updated {type} Speed to: {speed}.");
            }
            else
            {
                CreateConsoleResponse("Invalid Speed Value.");
            }
        }
        
        private void TeleportPlayer(string args, string defaultPosition)
        {
            string[] coordinates = args.Trim().Split(' ');

            if (coordinates.Length != 3) { CreateConsoleResponse("Invalid Command: $teleport requires 3 arguments (X, Y, Z)."); return; }

            if (float.TryParse(coordinates[0], out float x) &&
                float.TryParse(coordinates[1], out float y) &&
                float.TryParse(coordinates[2], out float z))
            {
                player.transform.position = new Vector3(x, y, z);
                CreateConsoleResponse($"Players Default Position was: {defaultPosition}. Player teleported to: {x}, {y}, {z}.");
            }
            else
            {
                CreateConsoleResponse("Invalid Command: Coordinates must be numeric values.");
            }
        }

        private void ChangeEnduranceLevel(string args, int defaultLevel)
        {
            if (int.TryParse(args, out int level))
            {
                player.playerNetworkManager.endurance.Value = level;
                player.playerNetworkManager.maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
                PlayerUIManager._Singleton.playerUIHudManager.SetMaxStaminaValue(player.playerNetworkManager.maxStamina.Value);
                player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;

                CreateConsoleResponse($"Default Level: {defaultLevel}. Updated Level to {level}.");
            }
            else
            {
                CreateConsoleResponse("Invalid Level Value.");
            }
        }
        #endregion
    }
}
