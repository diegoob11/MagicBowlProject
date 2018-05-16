#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

    public class CustomNetworkManager : MonoBehaviour
    {

        public NetworkManager manager;
        private bool matchFound;
        private bool disconnect;
        private int iterations;
        private int maxIterations;
        public int minPlayersPerRoom;
        public bool startGame = false;
        public int selectionIndex;

        private bool listTheMatch = true;

        void Start()
        {
            matchFound = true;
            iterations = 0;

            manager = this.GetComponent<NetworkManager>();
            manager.StartMatchMaker();
            manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);

            //Accede al indice de seleccion para saber que personaje spawnear
            GameObject md = GameObject.Find("mc");
            selectionIndex = md.GetComponentInChildren<CharacterCreation>().selectionIndex;
            //Selecciona el player prefab
            switch (selectionIndex)
            {
                case 1:
                    manager.playerPrefab = manager.spawnPrefabs[1];
                    break;
                default:
                    manager.playerPrefab = manager.spawnPrefabs[0];
                    break;
            }
            maxIterations = 100;

        }

        void Update()
        {

            
            // Si la lista de matches no es nula && Si el count de la lista de matches es mas grande que 0 &&
            // iterations es mas pequeño que maxIterations, cosa
            // que implica que lo acota a las primeras maxIterations interaciones, coge el primer match de la
            // lista y la asigna al manager, se une.
            if (manager.matches != null && manager.matches.Count > 0 && matchFound &&
                iterations < maxIterations)
            {
                matchFound = false;
                manager.matchName = manager.matches[0].name;
                manager.matchSize = (uint)manager.matches[0].currentSize;
                manager.matchMaker.JoinMatch(manager.matches[0].networkId, "", "", "", 0, 0,
                    manager.OnMatchJoined);

                // manager.matchName = "MagicBowl Game";
                // manager.matchSize = 4;
                // manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0,
                //      0, manager.OnMatchCreate);
                // matchFound = false;

                manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
            }
            //Si ha hecho mas de maxIterations iteraciones, y no ha encontrado partida (matchFound == true), 
            // crea la partida.
            if (iterations >= maxIterations && matchFound)
            {
                manager.matchName = "MagicBowl Game";
                manager.matchSize = 4;
                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0,
                     0, manager.OnMatchCreate);
                matchFound = false;

                manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
            }
            // Ha de ser igual a 4
            if (manager.numPlayers >= minPlayersPerRoom)
            {
                startGame = true;
                //Needed to destry the match
                if(listTheMatch){
                     manager.matchMaker.ListMatches(0, 1, "", true, 0, 0, manager.OnMatchList);
                }
                listTheMatch = false;
            }

            iterations++;
        }

        private void OnPlayerDisconnected(NetworkPlayer player)
        {
            if (player == GetServerPlayer())
            {
                Debug.Log("Server player disconnected");
                manager.matchMaker.DestroyMatch(manager.matches[0].networkId, 0, OnDestroyMatch);
            }
        }
    
        public void OnDestroyMatch(bool success, string extendedInfo)
        {
            Debug.Log("MatchDestroyed");
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

      
    }
};

#endif //ENABLE_UNET