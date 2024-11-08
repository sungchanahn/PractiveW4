using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition Health;
    public Condition Hunger;
    public Condition Stamina;
    public Condition Mana;

    private void Start()
    {
        CharacterManager.Instance.Player.Condition.UiCondition = this;
    }
}
