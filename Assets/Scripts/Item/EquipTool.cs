using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
    public bool isMagic;
    public float useMana;

    private Animator animator;
    private Camera camera;
    PlayerCondition playerCondition;

    private void Start()
    {
        playerCondition = CharacterManager.Instance.Player.Condition;
        animator = GetComponent<Animator>();
        camera = Camera.main;
        damage = 1;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (isMagic)
            {
                if (playerCondition.CanUseCondition(playerCondition.stamina, useStamina)
                    && playerCondition.CanUseCondition(playerCondition.mana, useMana))
                {
                    playerCondition.UseCondition(playerCondition.stamina, useStamina);
                    playerCondition.UseCondition(playerCondition.mana, useMana);
                    attacking = true;
                    animator.SetTrigger("Attack");
                    Invoke("OnCanAttack", attackRate);
                }
            }
            else
            {
                if (playerCondition.CanUseCondition(playerCondition.stamina, useStamina))
                {
                    playerCondition.UseCondition(playerCondition.stamina, useStamina);
                    attacking = true;
                    animator.SetTrigger("Attack");
                    Invoke("OnCanAttack", attackRate);
                }
            }
        }
    }

    private void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
            else if (doesDealDamage && hit.collider.TryGetComponent(out NPC npc))
            {
                npc.TakePhysicalDamage(damage);
            }
        }
    }
}
