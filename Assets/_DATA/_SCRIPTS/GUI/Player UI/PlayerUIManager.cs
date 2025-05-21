using Unity.Netcode;
using UnityEngine;

namespace NSG
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Singleton")]
        private static PlayerUIManager Singleton;
        public static PlayerUIManager _Singleton { get { return Singleton; } private set { Singleton = value; } }

        [Header("References")]
        public PlayerUIHudManager playerUIHudManager {  get; private set; }

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        private void Awake()
        {
            NSGUtils.SingletonCheck(ref Singleton, this);

            GetReferences();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;

                NetworkManager.Singleton.Shutdown();

                if (NetworkManager.Singleton.IsHost)
                    return;

                Debug.Log("You are no longer a host!");

                bool hasConnectedAsClient = NetworkManager.Singleton.StartClient();

                if (hasConnectedAsClient)
                    return;

                Debug.LogError("You cannot connect as a client, please check your network connection!");
            }
        }

        private void GetReferences()
        {
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        }
    }
}
