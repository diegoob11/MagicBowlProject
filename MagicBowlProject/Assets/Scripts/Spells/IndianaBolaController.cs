using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IndianaBolaController : NetworkBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bolaImg;
    private Image background;
    public Sprite bup;
    public Sprite bdown;
    public Sprite sup;
    public Sprite sdown;
    private Vector3 inputVector;

    public GameObject helper;

    public float angle;
    public GameObject background2;
    public Vector3 shootDirection;

    private bool spellIsLocked = false;
    public float lockTime;
    private float timeLocked;

    // Use this for initialization
    void Start()
    {
        bolaImg = gameObject.GetComponent<Image>();
        background = background2.GetComponent<Image>();
        angle = 0.0f;
        shootDirection = Vector3.zero;

        helper = Instantiate(helper) as GameObject;
        helper.SetActive(false);

        timeLocked = lockTime;
    }

    // Update is called once per frame
    void Update()
    {
        helper.transform.eulerAngles = new Vector3(90, angle + 90, 0);

        shootDirection.x = inputVector.x;
        shootDirection.z = inputVector.z;

        shootDirection = shootDirection.normalized * 3;

        helper.transform.position = transform.parent.transform.parent.transform.parent.transform.position + new Vector3(0, 0.3f, 0);

        if (spellIsLocked)
        {
            LockSpell();
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            bolaImg.sprite = sdown;
            background.sprite = bdown;
            helper.SetActive(true);
            OnDrag(ped);
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            bolaImg.sprite = sup;
            bolaImg.rectTransform.anchoredPosition = Vector3.zero;
            background.sprite = bup;

            helper.SetActive(false);

            inputVector = Vector3.zero;
            transform.parent.transform.parent.transform.parent.GetComponent<IndianaBolaPlayer>().PlayIndianaBola();

            spellIsLocked = true;
            GetComponent<AudioPlayer>().playindiana();

        }
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        if (!spellIsLocked)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background.rectTransform, ped.position, ped.pressEventCamera, out pos))
            {
                pos.x = (pos.x / background.rectTransform.sizeDelta.x);
                pos.y = (pos.y / background.rectTransform.sizeDelta.y);

                inputVector = new Vector3(pos.x * 4, 0, -pos.y * (1 / 0.7f));

                if (inputVector.magnitude > 1)
                {
                    inputVector = inputVector.normalized * 1.2f;
                }

                bolaImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (background.rectTransform.sizeDelta.x / 6),
                    -inputVector.z * (background.rectTransform.sizeDelta.y / 3f));

                angle = Mathf.Atan2(inputVector.z, inputVector.x) * 180 / Mathf.PI;
            }
        }
    }

    private void LockSpell()
    {
        bolaImg.color = new Color32(225, 225, 225, 100);
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
            bolaImg.color = Color.white;
        }
    }
}