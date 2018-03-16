using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dash : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler {

    private Image img;
    public Sprite sup;
    public Sprite sdown;

    private bool spellIsLocked = false;
    public float lockTime;
    private float timeLocked;

    void Start()
    {
        img = gameObject.GetComponent<Image>();
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
            img.sprite = sdown;
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            img.sprite = sup;
            transform.parent.transform.parent.transform.parent.GetComponent<PlayerController>().isDashing = true;
            spellIsLocked = true;
        }
    }

    private void LockSpell()
    {
        img.color = new Color32(225, 225, 225, 100);
        // Basic timer.
        if (timeLocked > 0)
        {
            timeLocked -= Time.deltaTime;
        }
        else
        {
            timeLocked = lockTime;
            spellIsLocked = false;
            img.color = Color.white;
        }
    }
}
