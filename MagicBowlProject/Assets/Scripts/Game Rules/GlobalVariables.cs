using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{

    public List<string> tags;
    public int currentSpawnedPlayer;
    public List<Transform> playerTransforms;
    // Use this for initialization
    void Start()
    {
        tags = new List<string> { "PlayerPurple", "PlayerBlue", "PlayerGreen", "PlayerRed" };
        currentSpawnedPlayer = 0;
    }
}
