using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Stamina : NetworkBehaviour
{
    public float maxStamina;
    [SyncVar(hook = "OnChangeStamina")] public float currentStamina;
    public float recoverySpeed;
    public Image staminaBar;
    private PlayerController playerController;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (!GetComponent<PlayerController>().isStunned && (currentStamina < maxStamina))
        {
            currentStamina += recoverySpeed * Time.deltaTime;
        }
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void TakeDamage(int amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
            currentStamina = 0;
    }

    public void StopStun()
    {
        currentStamina = 1;
    }

    void OnChangeStamina(float stamina)
    {
        staminaBar.fillAmount = stamina / maxStamina;
    }
}
