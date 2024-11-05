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
        <li>
      </ul>
    </div>
</details>

---

### [요구사항 3] 개선 문제
