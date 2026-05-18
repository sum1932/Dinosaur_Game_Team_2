# 2D Sprite Character + 3D Map Implementation Plan

이 문서는 프로젝트 방향이 **3D 맵 위에 2D Sprite 플레이어/NPC를 배치하는 2.5D 방식**으로 변경됨에 따라 필요한 구현 계획을 정리한 문서입니다.

실제 구현 전에 팀원이 같은 구조를 기준으로 작업할 수 있도록 플레이어, NPC, 카메라, 애니메이션, 상호작용 구조를 먼저 정의합니다.

---

## 1. 목표

- 맵과 충돌 환경은 3D로 유지한다.
- 플레이어와 NPC 공룡은 2D Sprite로 표현한다.
- 플레이어와 NPC Sprite는 항상 카메라를 바라보는 Billboard 방식으로 표시한다.
- 기존 3인칭 카메라 조작감과 WASD 이동 구조는 최대한 유지한다.
- 이동/충돌/상호작용 로직과 시각 표현(Sprite)을 분리한다.

---

## 2. 전체 구조

```text
3D World
├─ Terrain / Mesh / Collider
├─ Props / Environment Objects
├─ Player Root
│  └─ 2D Sprite Visual
└─ NPC Root
   └─ 2D Sprite Visual
```

핵심 원칙은 **Root는 게임 로직**, **Visual은 화면 표현**을 담당하는 것입니다.

---

## 3. 플레이어 구조

플레이어는 루트 오브젝트와 스프라이트 비주얼 오브젝트를 분리합니다.

```text
Player
├─ CharacterController
├─ PlayerMovement
├─ InteractionDetector
└─ Visual
   ├─ SpriteRenderer
   ├─ Animator
   └─ BillboardToCamera
```

### Player Root 역할

- 위치 이동
- 점프와 중력
- 충돌 처리
- 상호작용 범위 판단
- 게임 로직 기준 방향 관리

### Visual 역할

- 2D Sprite 표시
- 카메라를 향해 회전
- 걷기/대기/대화 애니메이션 재생
- 좌우 반전 또는 방향별 애니메이션 처리

플레이어 루트가 이동 방향으로 회전하더라도, 실제 보이는 Sprite는 카메라를 바라보도록 별도로 제어합니다.

---

## 4. NPC 구조

NPC 공룡도 플레이어와 같은 기준으로 루트와 비주얼을 분리합니다.

```text
NPC_Dinosaur
├─ Collider
├─ NPCInteraction
├─ NPCDialogue
└─ Visual
   ├─ SpriteRenderer
   ├─ Animator
   └─ BillboardToCamera
```

### NPC Root 역할

- 상호작용 가능 범위 제공
- 대화 데이터 연결
- NPC 상태 관리
- 위치 배치 기준

### NPC Visual 역할

- 공룡 Sprite 표시
- 카메라를 바라보도록 회전
- 감정/대화/대기 애니메이션 재생

---

## 5. Billboard 방식

빌보드는 Sprite가 항상 카메라를 바라보도록 만드는 방식입니다.

이 프로젝트에는 기본적으로 **Y-Axis Billboard**를 사용합니다.

### Y-Axis Billboard

- 좌우 방향만 카메라를 바라본다.
- X/Z 평면에서만 회전한다.
- Sprite가 지면에 수직으로 서 있는 느낌을 유지할 수 있다.
- 3D 맵 위 2D 캐릭터에 적합하다.

### Full Billboard

- 카메라의 상하좌우 회전을 모두 따라간다.
- 항상 카메라 정면을 향하지만, 캐릭터가 바닥에 서 있다는 느낌이 약해질 수 있다.
- 특수 이펙트, UI 마커, 떠 있는 오브젝트에만 선택적으로 사용한다.

---

## 6. 추가 예정 스크립트

### BillboardToCamera.cs

역할:

- 현재 카메라를 찾는다.
- `LateUpdate`에서 Visual 오브젝트를 카메라 방향으로 회전시킨다.
- Billboard 모드를 선택할 수 있게 한다.

예상 옵션:

```text
Billboard Mode
- Y Axis
- Full

Target Camera
- 비어 있으면 Camera.main 사용
```

적용 대상:

- Player/Visual
- NPC/Visual
- 2D Sprite로 표현되는 상호작용 오브젝트

---

## 7. 카메라와 이동 시스템

기존 카메라/이동 구조는 큰 틀에서 유지합니다.

현재 유지할 것:

- WASD 이동
- Space 점프
- E 상호작용 입력
- 우클릭 드래그 카메라 회전
- 마우스 휠 줌인/줌아웃
- 카메라 기준 이동 방향 계산

변경할 것:

- 플레이어 Mesh 중심 표현을 Sprite Visual 중심 표현으로 변경
- 플레이어 루트 회전과 Sprite Visual 회전을 분리
- Sprite Visual은 카메라를 바라보도록 Billboard 적용

주의할 점:

- `PlayerMovement`는 루트 오브젝트에 유지한다.
- `SpriteRenderer`는 루트가 아니라 `Visual` 자식 오브젝트에 둔다.
- 카메라가 바라보는 대상은 Player Root 또는 Camera Target으로 유지한다.

---

## 8. 애니메이션 방향 처리

2D Sprite 캐릭터는 이동 방향에 따라 애니메이션을 선택해야 합니다.

### 1차 방식

가장 단순한 방식입니다.

```text
Idle
Walk
Jump
Talk
```

방향별 Sprite를 나누지 않고 하나의 애니메이션만 사용합니다.

### 2차 방식

카메라 기준 이동 방향에 따라 애니메이션을 나눕니다.

```text
Idle_Front
Idle_Back
Idle_Side
Walk_Front
Walk_Back
Walk_Side
```

예시:

| 입력 | 애니메이션 |
| --- | --- |
| W | `Walk_Back` |
| S | `Walk_Front` |
| A / D | `Walk_Side` |
| Space | `Jump` |
| 대화 중 | `Talk` |

Side 애니메이션은 `SpriteRenderer.flipX`로 좌우를 처리할 수 있습니다.

---

## 9. 상호작용 시스템과의 관계

상호작용 판정은 Sprite가 아니라 Root 기준으로 처리합니다.

```text
Player Root
└─ InteractionDetector

NPC Root
└─ NPCInteraction
```

이렇게 해야 Sprite가 카메라를 향해 회전해도 상호작용 방향이나 충돌 범위가 흔들리지 않습니다.

---

## 10. 구현 순서

### 1단계: 구조 정리

1. Player 프리팹을 Root/Visual 구조로 분리한다.
2. Player Root에 `CharacterController`, `PlayerMovement`를 유지한다.
3. Visual 자식 오브젝트에 `SpriteRenderer`를 추가한다.
4. 기존 테스트용 Mesh는 제거하거나 비활성화한다.

### 2단계: Billboard 구현

1. `BillboardToCamera.cs`를 만든다.
2. Y-Axis Billboard 모드를 기본값으로 둔다.
3. Player Visual에 적용한다.
4. NPC 테스트 오브젝트에도 적용한다.
5. 카메라 회전/줌 중 Sprite가 자연스럽게 카메라를 바라보는지 확인한다.

### 3단계: 애니메이션 연결

1. 기본 Idle/Walk 애니메이션을 만든다.
2. `PlayerMovement`에서 이동 입력 또는 속도를 Animator에 전달한다.
3. Sprite 방향 처리가 필요하면 `SpriteRenderer.flipX` 또는 방향별 애니메이션을 연결한다.

### 4단계: NPC 적용

1. NPC Root/Visual 구조를 만든다.
2. NPC Visual에 `BillboardToCamera`를 적용한다.
3. NPC 대화 트리거와 Sprite 표시가 서로 간섭하지 않는지 확인한다.

---

## 11. 테스트 체크리스트

- 플레이어가 3D 바닥 위에서 정상 이동하는가?
- 카메라 회전 중 플레이어 Sprite가 항상 카메라를 바라보는가?
- 카메라 줌인/줌아웃 중 Sprite가 뒤집히거나 눕지 않는가?
- NPC Sprite도 동일하게 카메라를 바라보는가?
- Sprite Visual 회전이 CharacterController 충돌에 영향을 주지 않는가?
- E 상호작용 범위가 Sprite 방향과 무관하게 안정적으로 동작하는가?
- 애니메이션 전환이 이동 입력과 맞는가?

---

## 12. 주의사항

- SpriteRenderer를 Player Root에 직접 두지 않는 것을 권장합니다.
- Billboard 회전은 Visual 자식 오브젝트에만 적용합니다.
- Root 회전은 이동/상호작용 로직에서 사용할 수 있으므로 시각 회전과 분리합니다.
- Full Billboard는 캐릭터가 지면에서 떠 보일 수 있으므로 기본값으로 사용하지 않습니다.
- 카메라가 Cinemachine을 거치더라도 Billboard는 최종 출력 카메라 기준으로 계산해야 합니다.

---

## 13. 최종 권장 구조

```text
Player
├─ CharacterController
├─ PlayerMovement
├─ InteractionDetector
└─ Visual
   ├─ SpriteRenderer
   ├─ Animator
   └─ BillboardToCamera

NPC_Dinosaur
├─ Collider
├─ NPCInteraction
├─ NPCDialogue
└─ Visual
   ├─ SpriteRenderer
   ├─ Animator
   └─ BillboardToCamera
```

이 구조를 사용하면 맵은 3D로 유지하면서도 플레이어와 NPC는 2D Sprite 감성으로 표현할 수 있고, 이후 대화 시스템과 NPC 배치 작업도 안정적으로 확장할 수 있습니다.
