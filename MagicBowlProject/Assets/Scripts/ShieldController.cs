﻿using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ShieldController : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler
{
	private Image shieldimg;
	public Sprite sup;
	public Sprite sdown;

	private bool spellIsLocked = false;
	public float lockTime;
	private float timeLocked;

	// Use this for initialization
	void Start()
	{
		shieldimg = gameObject.GetComponent<Image>();
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
			shieldimg.sprite = sdown;
		}
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		if (!spellIsLocked)
		{
			shieldimg.sprite = sup;
			transform.parent.transform.parent.transform.parent.GetComponent<ShieldPlayer>().PlayShield();
			spellIsLocked = true;
			GetComponent<AudioPlayer>().playice();

		}
	}

	private void LockSpell()
	{
		shieldimg.color = new Color32(225, 225, 225, 100);
		// Basic timer.
		if (timeLocked > 0)
		{
			timeLocked -= Time.deltaTime;
		}
		else
		{
			// Stun time is over.
			timeLocked = lockTime;
			spellIsLocked = false;
			shieldimg.color = Color.white;
		}
	}
}
