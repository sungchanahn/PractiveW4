using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerCondition Condition;
    public Equipment equip;

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}
