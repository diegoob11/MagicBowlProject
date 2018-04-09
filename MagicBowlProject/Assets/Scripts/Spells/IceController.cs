using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IceController : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Image buttonImg;
    private Image cooldownImg;

    public Sprite pressedImg;
    private Sprite releasedImg;

    private bool spellIsLocked = false;
    public float lockTime;
    private float timeLocked;

    // Use this for initialization
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
            transform.parent.transform.parent.transform.parent.GetComponent<IcePlayer>().PlayIce();
            spellIsLocked = true;
            cooldownImg.enabled = true;
            GetComponent<AudioPlayer>().playice();
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
            // Stun time is over.
            cooldownImg.enabled = false;
            timeLocked = lockTime;
            spellIsLocked = false;
        }
    }
}
