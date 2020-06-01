﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescriptionUI : MonoBehaviour
{
    RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate() {
        rectTransform.localPosition = new Vector2(Input.mousePosition.x - Screen.width / 2 - rectTransform.sizeDelta.x / 2 - 1, 
                                                  Input.mousePosition.y - Screen.height / 2 - rectTransform.sizeDelta.y / 2 - 1);
    }
}
