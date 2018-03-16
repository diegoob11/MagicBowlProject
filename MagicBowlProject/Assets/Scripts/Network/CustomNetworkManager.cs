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
        private int flag2;
        public bool startGame = false;

        void Start()
        {
            flag = true;
            flag2 = 0;
            
            manager = this.GetComponent<NetworkManager>();
            manager.StartMatchMaker();
            manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
        }
        private void OnPlayerDisconnected(NetworkPlayer player)
        {
            if (player == GetServerPlayer())
            {
                Debug.Log("ASDASDASDASDASDSDDASDASD");
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
            //Debug.Log(manager.matches.Count);
            //Si la lista de matches no es nula && Si el count de la lista de matches es mas grande que 0 && el flag2 es mas pequeño que 100, cosa
            //que implica que lo acota a las primeras 100 interaciones, coge el primer match de la lista y la asigna al manager, se une.
            
            // Debug.Log(manager.matches);
            // Debug.Log(manager.matches.Count);
            
            if (manager.matches != null && manager.matches.Count > 0 && flag == true && flag2 < 100 && manager.numPlayers >= 1)
            {
                flag = false;
                manager.matchName = manager.matches[0].name;
                manager.matchSize = (uint)manager.matches[0].currentSize;
                manager.matchMaker.JoinMatch(manager.matches[0].networkId, "", "", "", 0, 0, manager.OnMatchJoined);
            }
            //Si ha hecho mas de 100 iteraciones, y no ha encontrado partida (flag == true), crea la partida.
            if (flag2 >= 100 && flag == true)
            {
               
                manager.matchName = "MagicBowl Game";
                manager.matchSize = 4;
                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                flag = false;

                manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);

                //Debug.Log("After" + manager.matches[0]);
            }
            //Ha de ser igual a 4
            if(manager.numPlayers >= 1)
            {
                startGame = true;
            }          
            flag2++;

        }
    }
};

#endif //ENABLE_UNET