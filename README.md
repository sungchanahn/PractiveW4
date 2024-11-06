# PractiveW4
## Q1
### [요구사항 1] 분석 문제
#### 1. Equipment와 EquipTool 기능의 구조와 핵심 로직을 분석   
- Equipment : 장비 장착 / 해제 및 장비 사용 (Attack) 입력 시 행동 호출   
EquipNew에서 Player 오브젝트의 EquipCamera(장착한 장비를 보여주는 카메라)의 자식으로 장착한 장비를 생성한다.   
이때 장착되어 있던 장비는 UnEquip 메서드를 통해 Destroy 한다.   
OnAttackInput을 InputSystem에서 설정한 Attack(mouse 좌클릭)에 연결하여,   
입력이 들어왔을 때 장비를 장착한 상태, canLook(인벤토리UI 비활성화된 상태)라면 EquipTool의 OnAttackInput을 호출한다.

- EquipTool : 장비 사용 (Attack) 시 실제 행동 기능 부분   
OnAttackInput은 InputSystem의 Attack이 발생할 때 호출된다.   
OnAttackInput에서 장비 사용(공격 모션) animation 실행, 장비 사용 쿨타임 적용(OnCanAttack 메서드, bool attacking)   
OnHit에서 Raycast를 이용해 현재 장비와 닿은 물체가 자원 체취 or 공격인지 판별하여 적용   


#### 2. Resource 기능의 구조와 핵심 로직을 분석
EquipTool의 OnHit에서 Resource의 Gather메서드 호출.
- itemToGive : 생성될 아이템
- quantityPerHit : Gather 호출 시 생성될 아이템 개수
- capacity : 자원 오브젝트에서 체취할 수 있는 아이템 총량
- Gather
EquipTool에서 doesGatherResources 장비(도끼)인 경우, 충돌된 오브젝트가 Resource를 갖고 있을 때 Gather 호출   
마우스 좌클릭 - EquipTool의 OnAttackInput 호출 - OnHit에서 반별 - Gather 호출   
한 번 호출될 때 quantityPerHit만큼 아이템 생성. capacity가 0이 되면 생성 멈춤   


---

### [요구사항 2] 확장 문제
#### 1. 새로운 자원을 만들고 새로운 자원채취 보상 아이템을 설정
![Q1_2_1_1](https://github.com/user-attachments/assets/1bb3872d-62af-4f89-b10f-399b7dd0c61d)
![Q1_2_1_2](https://github.com/user-attachments/assets/38ab4c8a-7491-44d2-854b-893a96df8706)   
Image와 같이 바위 오브젝트를 만들고 제취 아이템으로 돌(Item_Rock Prefab)이 생성되게 설정


#### 2. 두 개의 능력치를 사용하는 새로운 무기를 만들고 구현
![Q1-2-2](https://github.com/user-attachments/assets/b435b0b9-214b-47e4-8032-cf765cac7484)


<details>
  <summary>코드</summary>
    <div markdown="1">
      <ul>
        <li>EquipTool.cs</li>
        <img src = "https://github.com/user-attachments/assets/636b4faf-8013-48a0-a088-b20eadc11250">
        <li>PlayerCondition.cs</li>
        <img src = "https://github.com/user-attachments/assets/78375bb5-c10b-48a8-8eea-1b86a084fc6a">
      </ul>
    </div>
</details>

---

### [요구사항 3] 개선 문제

---

## Q2
### [요구사항 1] 분석 문제
#### 1. AI Navigation에서 가장 핵심이 되는 개념 복습
유니티에서는 내비게이션 메시를 자동으로 생성하여 게임 캐릭터가 갈 수 있는 곳, 없는 곳을 구분하고 최적의 경로를 탐색하여 움직일 수 있는 AI Navigation 시스템이 있다.

- NavMesh (Navigation Mesh)   
게임 월드에서 걸을 수 있는 표면을 뜻한다. 이동 가능/불가능한 곳을 설정하고, 그 위에서 어느 한 위치로 이동할 수 있는 최적의 경로를 찾아 자동으로 이동시킬 수 있다.

- NavMesh Agent   
NavMesh Agent 컴포넌트를 사용하여 목표 위치로 이동하는 동안 장애물, 다른 NavMeshAgent를 피할 수 있는 캐릭터를 만들 수 있다.
Obstacle Avoidance를 통해 자신의 회피 영역을 설정할 수 있다.

- NavMesh Obstacle   
이 컴포넌트를 사용하여 장애물을 설정할 수 있다. 움직이는 장애물이면 피하도록 설정, 정지한 경우 NavMesh 상에서 갈 수 없는 곳으로 설정한다. 경로를 완전히 차단하면 다른 경로를 찾게 할 수 있다.

**AI Navigation의 주요 기능**
1. NavMesh - 이동 가능 영역 구분
2. Pathfinding - 경로 탐색
3. Steering Behavior - 오브젝트가 경로를 따라 이동할 때 자연스러운 동작 구현
4. Obstacle Avoidance - 장애물 회피
5. Local Avoidance - 오브젝트 간 회피

#### 2. NPC 기능의 구조와 핵심 로직 분석
3가지 상태 Idle, Wandering, Attacking을 가진다.   
Update에서는 현재 상태만 확인하여 그 상태의 Update메서드를 호출한다.   
Idle, Wandering 상태에서는 PassiveUpdate를 호출하는데 Idle 상태와 Wandering 상태를 상황에 맞게 서로 바꿔준다.   
이 때 플레이어가 detectDistance 안에 있으면 Attacking 상태로 전환한다.   
Attacking 상태에서 호출되는 AttackingUpdate에서   
- 공격 거리, 시야 안에 있으면 공격   
- 공격거리 < 플레이어 거리 < 탐지거리: 추적 가능한 경로인지 판단해서 추적하거나 Wandering 상태로 전환   
- 그 외: Wandering 상태로 전환   
IDamagable을 상속 받아 플레이어에게 공격받으면 TakePhysicalDamage 호출

---

### [요구사항 2] 확장 문제
#### 1. AI 네비게이션 기능을 바탕으로 펫 기능 구현

#### 2. AI 네비게이션 기능을 바탕으로 원거리 공격 몬스터 구현 (ex. 기존 몬스터보다 추적 범위를 넓히고 원거리에서 무기를 던짐)
