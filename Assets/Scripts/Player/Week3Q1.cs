using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week3Q1 : MonoBehaviour
{
    /*
     * SendMassage VS Invoke Event
     * ������
     * �Է��� ������ Ư�� �Լ��� ȣ���Ѵ�.
     * 
     * ������
     * SendMassage�� ��� "On + Action�̸�"�� �Լ��� ã�� ȣ���Ѵ�. ex) OnMove, OnLook
     * Invoke Event�� ��� Inspector���� �Է¿� ���� �Լ��� �����ϰ�, �Է��� ������ ������ �Լ��� ȣ���Ѵ�.
    */

    /*
     * CharacterManager�� Player�� ����
     * Player : �÷��̾� ��ü�� ���� ��ɵ��� ������ ���� �ϳ��� ��� Player�� ������ �� �ְ� �Ѵ�.
     * CharacterManager : �̱����̱� ������ ���ӿ��� �÷��̾ �ϳ��� ������ �� �ְ� �Ѵ�. ��𼭵� Player�� ������ �� �ְ� �Ѵ�.
     */

    /*
     * Move �м�
     * 3D �󿡼� �¿�(x��)�� �Է°� curMovementInput�� x��, �յ�(z)��� y���̴�.
     * dir���� �����¿�� ���� ���������� ����(3D�� y��)�� 0�̱� ������ ���� rigidbody�� velocity.y�� �������ش�.
     * �Է��� ���� ���� ���� curMovementInput�� zero
     */

    /*
     * CameraLook �м�
     * ī�޶� ���Ϸ� �����̷��� �Է°��� y�� x���� �������� ȸ���� �����ؾ��Ѵ�.
     * ī�޶� �ٶ󺸴� ������ z���̱� ������ z���� �������� ȸ���ϸ� �ٶ󺸴� ������ �������� �������� ����.
     * 3D���� EulerAngles�� x���� ����� �� ī�޶� �Ʒ��� ȸ���Ѵ�.
     * �Է°��� ���(���콺�� ���� ���� ��)�� ���� ī�޶� ���� ȸ���ؾ� �ϱ� ������ ��ȣ�� �ݴ�� �����ؾ� �Ѵ�.
     * ī�޶��� �¿� ȸ���� y���� �������� �����Ѵ�.
     */

    /*
     * IsGrounded �м�
     * ������Ʈ�� Pivot�� �������� �����¿�� ���ݾ� ������ ��ġ���� �Ʒ��� ���� Ray�� ���.
     * ������Ʈ�� �ٴڿ����� ������Ʈ�� �پ����� ���� �������� pivot�� �ٴ� ������ �Ÿ� + ���� ��(���⼱ 0.1))��ŭ Ray�� ���.
     * Raycast���� �ٴ� Layer�� ����Ǹ� �ٴڿ� �پ��ִ� ��
     */

    /*
     * `Move`�� `CameraLook` �Լ��� ���� `FixedUpdate`, `LateUpdate`���� ȣ���ϴ� ����
     * LateUpdate�� FixedUpdate�� ��� ���� �� ȣ��ȴ�.
     * FixedUpdate������ ������ �������� ȣ��Ǳ� ������ ������Ʈ�� �������� �κ��� ó���Ѵ�.
     * (Update�� �������� ������� ȣ��Ǳ� ������ �ұ�Ģ���� ȣ���̴�. ���� �����ϰ� ����Ǿ�� �ϴ� ���� �ۿ��� ó���ϱ⿡ �������� �ʴ�.)
     * ī�޶�� ĳ������ ��� �����ۿ�(FixedUpdate)�� ���� �� �̵��ؾ� �Ѵ�.
     */

    // Ȯ�� ���� : ���� ������ �������� ���ο� ����� �߰� �����غ��ô�.
    // - ���ο� �Է� ���� �޾� ȯ�漳�� â�� ��������.
    // - ����â�� ���� �� ���콺 Ŀ���� ���̰� ī�޶� ȸ���� ���� ����� �ڵ�� �����غ�����.


    /*
     * ������ UI ��ũ��Ʈ�� ����� ������ ���� ��ü������ �������� �����غ�����.
     * UI ��ũ��Ʈ�� ������ ����� �ٸ� ��ũ��Ʈ���� ���յ��� ���� UI ���� �� Ȯ�忡 ������ �� �ִ�.
     */


    /*
     * �������̽��� Ư¡�� ���� �����غ��� ������ ������ �м��غ�����.
     * �������̽��� �޼���, �̺�Ʈ, ������Ƽ�� ���� �� �ִ�.
     * ����������(private, protected) ��� X, public�� ���
     * Ŭ������ �ϳ��� Ŭ������ ��� ���� �� ������, �������̽��� ���� �� ��� ���� �� �ִ�.
     * �޼����� ���� �κ��� ���� ������ ���� ��� ���� Ŭ�������� ������ ����� �Ѵ�.
     * ��� �޴� Ŭ�������� �ݵ�� �ؾ� �� ����,��� �̶�� �� �� �ִ�.
     * ��� �޴� Ŭ�������� ���� �ٸ� ������ �� �� �ִ�.
     * 
     * interface IDamagable���� TakePhysicalDamage �޼��带 ������ �ִ�.
     * IDamagable�� ��� �޴� Ŭ���������� TakePhysicalDamage �޼��带 �ݵ�� �����ؾ� �Ѵ�.
     * IDamagable�� ��� �޴� Ŭ������ PlayerCondition�� NPC�� �ִµ�,
     * PlayerCondition - health ����, OnTakeDamage�� ��ϵ� �޼���(DamageIndicator UI Image�� �״� ��) ȣ��
     * NPC - health ����, die ȣ��, �ǰ�ȿ��(DamageFlash) ȣ��
     */


    /*
     * �ٽ� ������ �м��غ�����. (UI ��ũ��Ʈ ����, `CampFire`, `DamageIndicator`)
     * UI ��ũ��Ʈ ����
     * DamageIndicator : Player �ǰ� �� TakePhysicalDamage�� ���� UIImage(�÷��̾� �ǰ� ȿ�� �̹���)�� �״� ���� �޼��� ȣ��
     * Condition - UICondition
     * Condition ��ũ��Ʈ�� Health, Stamina, Hungry �� ������ UI������Ʈ�� ��� (����� �� �ݿ�)
     * UICondition ��ũ��Ʈ�� �� Condition�� PlayerCondition(���� �����ϴ� �κ�)�� ������ ����
     * ItemSlot - UIInventory
     * ItemSlot : ItemSlot �� ĭ�� ���� ��ũ��Ʈ
     * UIInventory : ItemSlot�� �߿��� ��ȣ�ۿ� ����� �ݿ��� ItemSlot ����
     * 
     * CampFire : OnTriggerEnter���� ���� collider�� IDamagable�� ���� ���
     * �� ������Ʈ�� IDamagable�� List�� �ִ´�.
     * DealDamage���� List ���� ��� IDamagable�� TakePhysicalDamage�� ȣ��
     * InvokeRepeating���� �ݺ�
     * Exit���� ������ ���� collider�� ���� ��� �ش� IDamagable�� List���� ����
     * 
     * 
     */
}
