using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireballLifetime : MonoBehaviour
{

    private float age;
    public float m_MaxLifeTime;

    void Start()
    {
        age = 0f;
        m_MaxLifeTime = 2f;
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age > m_MaxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
