#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    public class CustomNetworkManager : MonoBehaviour
    {

        public NetworkManager manager;
        private bool flag;
        private bool flag_disconnect;
        private int i;
        private int flag2;
        public bool startGame = false;

        void Start()
        {
            flag = true;
            flag2 = 0;
            i = 0;
            
            manager = this.GetComponent<NetworkManager>();
            manager.StartMatchMaker();
            manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
        }
        private void OnPlayerDisconnected(NetworkPlayer player)
        {
            if (player == GetServerPlayer())
            {
                manager.matchMaker.DestroyMatch(manager.matches[0].networkId, 0, OnMatchDestroy);
            }
        }
        public void OnMatchDestroy(bool success, string extendedInfo)
        {
            // ...
        }
        public static NetworkPlayer GetServerPlayer()
        {
            if (Network.isClient)
                return Network.connections[0];
            if (Network.isServer)
                return Network.player;
            // not connected or not running as server
            return Network.connections[0];
        }
        void Update()
        {
            //Si la lista de matches no esta vacia (null) && Si el count es superiror a 0
            if (manager.matches != null && manager.matches.Count > 0 && flag == true && flag2 < 100)
            {
                flag = false;
                manager.matchName = manager.matches[0].name;
                manager.matchSize = (uint)manager.matches[0].currentSize;
                manager.matchMaker.JoinMatch(manager.matches[0].networkId, "", "", "", 0, 0, manager.OnMatchJoined);
            }
            if (flag2 > 100 && flag == true)
            {
                manager.matchName = "MagicBowl Game";
                manager.matchSize = 4;
                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                flag = false;
            }
            // to change for 4
            if(manager.numPlayers >= 1)
            {
                startGame = true;
            }
            
            flag2++;
            i++;
        
        }
    }
};

#endif //ENABLE_UNET