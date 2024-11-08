using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week3Q1 : MonoBehaviour
{
    /*
     * SendMassage VS Invoke Event
     * 공통점
     * 입력을 받으면 특정 함수를 호출한다.
     * 
     * 차이점
     * SendMassage의 경우 "On + Action이름"인 함수를 찾아 호출한다. ex) OnMove, OnLook
     * Invoke Event의 경우 Inspector에서 입력에 대한 함수를 설정하고, 입력을 받으면 설정된 함수를 호출한다.
    */

    /*
     * CharacterManager와 Player의 역할
     * Player : 플레이어 객체가 갖는 기능들을 참조를 통해 하나로 모아 Player로 접근할 수 있게 한다.
     * CharacterManager : 싱글톤이기 때문에 게임에서 플레이어가 하나만 존재할 수 있게 한다. 어디서든 Player에 접근할 수 있게 한다.
     */

    /*
     * Move 분석
     * 3D 상에서 좌우(x축)는 입력값 curMovementInput의 x값, 앞뒤(z)축는 y값이다.
     * dir에서 전후좌우는 값을 적용했지만 상하(3D의 y축)는 0이기 때문에 현재 rigidbody의 velocity.y를 적용해준다.
     * 입력을 받지 않을 때는 curMovementInput은 zero
     */

    /*
     * CameraLook 분석
     * 카메라가 상하로 움직이려면 입력값의 y를 x축을 기준으로 회전을 적용해야한다.
     * 카메라가 바라보는 방향이 z축이기 때문에 z축을 기준으로 회전하면 바라보는 방향을 기준으로 원형으로 돈다.
     * 3D에서 EulerAngles의 x값이 양수일 때 카메라가 아래로 회전한다.
     * 입력값이 양수(마우스를 위로 향할 때)일 때는 카메라가 위로 회전해야 하기 때문에 부호를 반대로 적용해야 한다.
     * 카메라의 좌우 회전은 y축을 기준으로 적용한다.
     */

    /*
     * IsGrounded 분석
     * 오브젝트의 Pivot을 기준으로 전후좌우로 조금씩 떨어진 위치에서 아래를 향해 Ray를 쏜다.
     * 오브젝트가 바닥역할의 오브젝트에 붙어있을 때를 기준으로 pivot과 바닥 사이의 거리 + 작은 값(여기선 0.1))만큼 Ray를 쏜다.
     * Raycast에서 바닥 Layer가 검출되면 바닥에 붙어있는 것
     */

    /*
     * `Move`와 `CameraLook` 함수를 각각 `FixedUpdate`, `LateUpdate`에서 호출하는 이유
     * LateUpdate는 FixedUpdate가 모두 끝난 후 호출된다.
     * FixedUpdate에서는 일정한 간격으로 호출되기 때문에 오브젝트의 물리적인 부분을 처리한다.
     * (Update는 프레임을 기반으로 호출되기 때문에 불규칙적인 호출이다. 따라서 일정하게 적용되어야 하는 물리 작용을 처리하기에 적합하지 않다.)
     * 카메라는 캐릭터의 모든 물리작용(FixedUpdate)가 끝난 후 이동해야 한다.
     */

    // 확장 문제 : 강의 내용을 바탕으로 새로운 기능을 추가 구현해봅시다.
    // - 새로운 입력 값을 받아 환경설정 창을 만들어보세요.
    // - 설정창이 떴을 때 마우스 커서가 보이고 카메라 회전을 막는 기능을 코드로 구현해보세요.


    /*
     * 별도의 UI 스크립트를 만드는 이유에 대해 객체지향적 관점에서 생각해보세요.
     * UI 스크립트를 별도로 만들면 다른 스크립트와의 결합도를 낮춰 UI 수정 및 확장에 대응할 수 있다.
     */


    /*
     * 인터페이스의 특징에 대해 정리해보고 구현된 로직을 분석해보세요.
     * 인터페이스는 메서드, 이벤트, 프로퍼티만 가질 수 있다.
     * 접근제한자(private, protected) 사용 X, public만 사용
     * 클래스는 하나의 클래스만 상속 받을 수 있지만, 인터페이스는 여러 개 상속 받을 수 있다.
     * 메서드의 구현 부분이 없기 때문에 실제 상속 받은 클래스에서 구현을 해줘야 한다.
     * 상속 받는 클래스에서 반드시 해야 할 업무,약속 이라고 할 수 있다.
     * 상속 받는 클래스마다 서로 다른 구현을 할 수 있다.
     * 
     * interface IDamagable에서 TakePhysicalDamage 메서드를 가지고 있다.
     * IDamagable을 상속 받는 클래스에서는 TakePhysicalDamage 메서드를 반드시 구현해야 한다.
     * IDamagable을 상속 받는 클래스는 PlayerCondition과 NPC가 있는데,
     * PlayerCondition - health 감소, OnTakeDamage에 등록된 메서드(DamageIndicator UI Image를 켰다 끔) 호출
     * NPC - health 감소, die 호출, 피격효과(DamageFlash) 호출
     */


    /*
     * 핵심 로직을 분석해보세요. (UI 스크립트 구조, `CampFire`, `DamageIndicator`)
     * UI 스크립트 구조
     * DamageIndicator : Player 피격 시 TakePhysicalDamage를 통해 UIImage(플레이어 피격 효과 이미지)를 켰다 끄는 메서드 호출
     * Condition - UICondition
     * Condition 스크립트는 Health, Stamina, Hungry 등 각각의 UI오브젝트를 담당 (컨디션 값 반영)
     * UICondition 스크립트는 각 Condition과 PlayerCondition(값을 변경하는 부분)의 연결점 역할
     * ItemSlot - UIInventory
     * ItemSlot : ItemSlot 한 칸에 대한 스크립트
     * UIInventory : ItemSlot들 중에서 상호작용 결과를 반영할 ItemSlot 선택
     * 
     * CampFire : OnTriggerEnter에서 들어온 collider가 IDamagable을 가진 경우
     * 그 오브젝트의 IDamagable을 List에 넣는다.
     * DealDamage에서 List 안의 모든 IDamagable의 TakePhysicalDamage를 호출
     * InvokeRepeating으로 반복
     * Exit으로 위에서 들어온 collider가 나갈 경우 해당 IDamagable을 List에서 제거
     * 
     * 
     */
}
