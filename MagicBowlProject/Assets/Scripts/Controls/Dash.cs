using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dash : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler {
    private Image buttonImg;
    private Image cooldownImg;

    public Sprite pressedImg;
    private Sprite releasedImg;

    private bool spellIsLocked = false;
    public float lockTime;
    private float timeLocked;

    void Start()
    {
        buttonImg = gameObject.GetComponent<Image>();
        cooldownImg = transform.parent.Find("Cooldown").GetComponent<Image>();
        releasedImg = buttonImg.sprite;

        timeLocked = lockTime;
    }

    void Update()
    {
        if (spellIsLocked)
        {
            LockSpell();
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            buttonImg.sprite = pressedImg;
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            buttonImg.sprite = releasedImg;
            transform.parent.transform.parent.transform.parent.GetComponent<PlayerController>().isDashing = true;
            spellIsLocked = true;
            cooldownImg.enabled = true;
        }
    }

    private void LockSpell()
    {
        // Basic timer.
        if (timeLocked > 0)
        {
            timeLocked -= Time.deltaTime;
            cooldownImg.fillAmount = timeLocked / lockTime;
        }
        else
        {
            cooldownImg.enabled = false;
            timeLocked = lockTime;
            spellIsLocked = false;
        }
    }
}
