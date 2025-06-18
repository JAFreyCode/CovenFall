using System.Collections.Generic;
using UnityEngine;

namespace NSG
{
    public class GameSessionManager : MonoBehaviour
    {
        public static GameSessionManager _Singleton;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private void Awake()
        {
            if (_Singleton != null)
                _Singleton = this;
            else
                Destroy(gameObject);
        }

        public void AddPlayerToActivePlayersList(PlayerManager newPlayer)
        {
            // CHECK THE LIST, IF IT DOES NOT ALREADY CONTAIN THE PLAYER, ADD THEM
            if (!players.Contains(newPlayer))
            {
                players.Add(newPlayer);
            }

            // CHECK THE LIST FOR NULL SLOTS, AND REMOVE THE NULL SLOTS
            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            // CHECK THE LIST, IF IT DOES CONTAIN THE PLAYER, REMOVE THEM
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            // CHECK THE LIST FOR NULL SLOTS, AND REMOVE THE NULL SLOTS
            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }
}
