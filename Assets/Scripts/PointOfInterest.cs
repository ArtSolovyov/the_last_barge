﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PointOfInterest : MonoBehaviour
{
    public Vector3 RescaleMod = Vector3.one;
    public Transform PointDescription;
    public Vector3 ImageExtents = Vector3.zero;
    public Transform ConfirmWindow;
    public MapRandomizer ParentMapRandomizer;
    public string SceneToLoad = "Scenes/BuildingScene";

    protected Vector3 NewScale = Vector3.one;
    protected Transform PointDescrObject;
    protected Transform ConfirmWindowObject;

    void OnMouseEnterWorldCoordinates()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;
            NewScale.x = ls.x * RescaleMod.x;
            NewScale.y = ls.y * RescaleMod.y;
            NewScale.z = ls.z * RescaleMod.z;

            transform.localScale = NewScale;

            if (PointDescription)
            {
                Vector3 mousePos = new Vector3();
                mousePos.x = transform.position.x + ImageExtents.x;
                mousePos.y = transform.position.y + ImageExtents.y;
                mousePos.z = transform.position.z;

                PointDescrObject = Instantiate(PointDescription, mousePos, Quaternion.identity);
            }

            ParentMapRandomizer.BigState = true;
        }
    }

    protected void OnMouseEnter()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;
            NewScale.x = ls.x * RescaleMod.x;
            NewScale.y = ls.y * RescaleMod.y;
            NewScale.z = ls.z * RescaleMod.z;

            transform.localScale = NewScale;

            PointDescription.gameObject.SetActive(true);

            ParentMapRandomizer.BigState = true;
        }
    }

    public void OnMouseExit()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.BigState && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;
            NewScale.x = ls.x / RescaleMod.x;
            NewScale.y = ls.y / RescaleMod.y;
            NewScale.z = ls.z / RescaleMod.z;

            transform.localScale = NewScale;

            PointDescription.gameObject.SetActive(false);

            ParentMapRandomizer.BigState = false;
        }
    }

    public void OnMouseExitWorldCoordinates()
    {
        if (ParentMapRandomizer && ParentMapRandomizer.BigState && ParentMapRandomizer.ReleaseModal)
        {
            Vector3 ls = transform.localScale;
            NewScale.x = ls.x / RescaleMod.x;
            NewScale.y = ls.y / RescaleMod.y;
            NewScale.z = ls.z / RescaleMod.z;

            transform.localScale = NewScale;

            if (PointDescrObject)
            {
                Destroy(PointDescrObject.gameObject);
            }

            ParentMapRandomizer.BigState = false;
        }
    }

    void OnMouseDown()
    {
        if (ParentMapRandomizer && ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            ConfirmWindow.gameObject.SetActive(true);

            ParentMapRandomizer.ReleaseModal = false;
            ParentMapRandomizer.OverPoint = this;

//            Transform confirmWindowObject = PointDescription.transform.Find("YesNoPanel/Yes");

//            confirmWindowObject.gameObject.GetComponent<Button>().onClick.AddListener(OnClickConfirmButton);

            foreach (Button butYesNo in ConfirmWindow.gameObject.GetComponentsInChildren<Button>())
            {
                butYesNo.onClick.AddListener(OnClickConfirmButton);
            }

        }
    }

    public void OnClickConfirmButton()
    {
        if (ParentMapRandomizer)
        {
            ParentMapRandomizer.ReleaseModal = true;
            OnMouseExit();
            ParentMapRandomizer.MoveBargeToPosition(transform.position);
        }

        ConfirmWindow.gameObject.SetActive(false);
    }

    void OnMouseDownWorldCoordinates()
    {
        if (ParentMapRandomizer && ParentMapRandomizer && ParentMapRandomizer.ReleaseModal)
        {
            RectTransform PointDescrRect = PointDescrObject.GetComponentInChildren<RectTransform>();
            ConfirmWindowObject = Instantiate(ConfirmWindow, transform.position, Quaternion.identity);
            RectTransform ConfirmWindowRect = ConfirmWindow.GetComponentInChildren<RectTransform>();

            if (PointDescrRect && ConfirmWindowRect)
            {
                Vector3 posShift = PointDescrObject.position;
                posShift.y -= PointDescrRect.sizeDelta.y / 2 + ConfirmWindowRect.sizeDelta.y / 2;
                ConfirmWindowObject.position = posShift;
            }

//            if (ConfirmWindowObject.GetComponent<ConfirmWindow>())
//            {
//                ConfirmWindowObject.GetComponent<ConfirmWindow>().ParentPointOfInterest = this;
//                ParentMapRandomizer.ReleaseModal = false;
//            }

            OnMouseExit();
        }
    }


}
