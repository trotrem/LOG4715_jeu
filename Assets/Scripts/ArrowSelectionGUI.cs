using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionGUI : MonoBehaviour {

    [SerializeField]
    GameObject NormalArrowIcon;

    [SerializeField]
    GameObject FireArrowIcon;

    [SerializeField]
    GameObject WindArrowIcon;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void Select(ArrowType selectedArrow)
    {
        switch (selectedArrow)
        {
            case ArrowType.NORMAL:
                NormalArrowIcon.SetActive(true);
                FireArrowIcon.SetActive(false);
                WindArrowIcon.SetActive(false);
                break;
            case ArrowType.FIRE:
                NormalArrowIcon.SetActive(false);
                FireArrowIcon.SetActive(true);
                WindArrowIcon.SetActive(false);
                break;
            case ArrowType.WIND:
                NormalArrowIcon.SetActive(false);
                FireArrowIcon.SetActive(false);
                WindArrowIcon.SetActive(true);
                break;
        }
    }
}
