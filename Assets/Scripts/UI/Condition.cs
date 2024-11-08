using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float CurValue;
    public float StartValue;
    public float MaxValue;
    public float PassiveValue;
    public Image UiBar;

    private void Start()
    {
        CurValue = StartValue;
    }

    private void Update()
    {
        UiBar.fillAmount = GetPrecentage();
    }

    private float GetPrecentage()
    {
        return CurValue / MaxValue;
    }

    public void Add(float _value)
    {
        CurValue = Mathf.Min(CurValue + _value, MaxValue);
    }

    public void Substract(float _value)
    {
        CurValue = Mathf.Max(CurValue - _value, 0);
    }
}
