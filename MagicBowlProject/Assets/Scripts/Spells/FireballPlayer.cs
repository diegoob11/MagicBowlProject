using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireballPlayer : NetworkBehaviour
{
    public GameObject particleSys; // The actual particle system for the spell
    public int damage; // Amount of damage the fireball does

    private void Update()
    {
        if (isLocalPlayer)
        {
            foreach (Transform t in transform)
            {
                if (t.name == "SpellCanvas(Clone)")
                {
                    float angle = t.GetChild(0).transform.GetChild(0).GetComponent<FireballController>().angle;
                    Aim(angle);
                }
            }
        }
    }

    public void Aim(float angle)
    {
        particleSys.transform.eulerAngles = new Vector3(18, angle + 90, 0);
        particleSys.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    public void PlayFireball()
    {
        if (isLocalPlayer)
        {
            GetComponent<PlayerController>().PlaySpellAnimation();
            CmdPlayFireball(particleSys.transform.position, particleSys.transform.eulerAngles);
        }
    }

    [Command]
    public void CmdPlayFireball(Vector3 position, Vector3 rotation)
    {
        RpcPlayFireball(position, rotation);
    }

    [ClientRpc]
    public void RpcPlayFireball(Vector3 position, Vector3 rotation)
    {
        // Play only if the player is not stunned
        if (!(GetComponent<PlayerController>().isStunned))
        {
            GameObject particleSysNetwork = Instantiate(particleSys) as GameObject;
            particleSysNetwork.transform.eulerAngles = rotation;
            particleSysNetwork.transform.position = position;
            //Play the particle system
            particleSysNetwork.GetComponent<ParticleSystem>().Play();

            //Cal probar si funciona, si s'executa el so a l'altra banda
        }
    }
}