using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteMechanics : MonoBehaviour
{
    private Board _board;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        _board = FindAnyObjectByType<Board>();
    }

    public void OnClick()
    {
        _board.card = "delete";
    }
}
