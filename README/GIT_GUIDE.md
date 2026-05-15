# Dinosaur Game Git Guide

이 문서는 `Dinosaur_Game_Team_2` 프로젝트를 팀원들과 함께 작업하기 위한 Git/GitHub 사용 가이드입니다.

프로젝트는 Unity 기반 3D 게임입니다. 멸망하기 직전의 공룡 섬에서 플레이어가 섬 곳곳에 배치된 NPC 공룡을 찾고, 대화하며, 결국 섬의 멸망을 마주하는 게임을 목표로 합니다.

---

## 1. 프로젝트 저장소

- GitHub 저장소: https://github.com/sum1932/Dinosaur_Game_Team_2
- 기본 브랜치: `main`
- 엔진: Unity
- 언어: C#
- 프로젝트 폴더명: `Dinosaur_Game`

팀원은 GitHub 초대를 수락한 뒤 저장소를 내려받아 작업합니다.

```bash
git clone https://github.com/sum1932/Dinosaur_Game_Team_2.git
```

Unity Hub에서는 클론한 `Dinosaur_Game_Team_2` 폴더를 `Add project from disk`로 추가해 열면 됩니다.

---

## 2. Git과 GitHub 기본 개념

- **Git**: 파일 변경 기록을 저장하고 되돌릴 수 있게 해주는 버전 관리 도구
- **GitHub**: Git으로 관리하는 프로젝트를 인터넷에 올리고 팀원과 공유하는 서비스
- **Repository**: 프로젝트 저장소
- **Commit**: 현재 변경 내용을 하나의 기록으로 저장하는 것
- **Push**: 내 컴퓨터의 커밋을 GitHub에 올리는 것
- **Pull**: GitHub에 올라온 최신 내용을 내 컴퓨터로 가져오는 것
- **Conflict**: 여러 사람이 같은 파일의 같은 부분을 수정해서 Git이 자동으로 합치지 못하는 상태

---

## 3. GitHub Desktop으로 작업하기

초보자는 명령어보다 GitHub Desktop 사용을 권장합니다.

### 3.1 처음 설정

1. GitHub Desktop을 실행합니다.
2. GitHub 계정으로 로그인합니다.
3. `File > Clone repository`를 선택합니다.
4. `Dinosaur_Game_Team_2` 저장소를 선택합니다.
5. 원하는 위치에 클론합니다.
6. Unity Hub에서 클론된 폴더를 엽니다.

### 3.2 작업 시작 전

작업을 시작하기 전에 항상 최신 상태를 받아옵니다.

1. GitHub Desktop을 엽니다.
2. 상단의 `Fetch origin`을 누릅니다.
3. `Pull origin` 버튼이 보이면 눌러 최신 파일을 받습니다.
4. 현재 브랜치가 `main`인지 확인합니다.

### 3.3 작업 중

1. Unity에서 씬, 스크립트, 프리팹, 아트 리소스 등을 수정합니다.
2. GitHub Desktop의 `Changes` 탭에서 변경된 파일을 확인합니다.
3. 올릴 파일만 체크합니다.
4. `Summary`에 변경 내용을 짧게 적습니다.
5. 필요하면 `Description`에 자세한 설명을 적습니다.
6. `Commit to main`을 누릅니다.

커밋은 아직 내 컴퓨터에만 저장된 상태입니다.

### 3.4 작업 완료 후

1. GitHub Desktop에서 `Push origin`을 누릅니다.
2. GitHub 저장소 페이지에서 변경 내용이 올라갔는지 확인합니다.
3. 팀원에게 작업 완료 내용을 공유합니다.

---

## 4. 명령어로 작업하기

Git 명령어를 사용하는 팀원은 아래 흐름을 따르면 됩니다.

작업 시작 전:

```bash
git pull
```

변경 내용 확인:

```bash
git status
```

변경 파일 추가:

```bash
git add .
```

커밋:

```bash
git commit -m "Add dinosaur NPC dialogue trigger"
```

GitHub에 업로드:

```bash
git push
```

---

## 5. 커밋 메시지 작성법

커밋 메시지는 "무엇을 했는지"가 바로 보이게 작성합니다.

### 좋은 예시

| Summary | Description |
| --- | --- |
| `Add player movement controller` | 플레이어 이동과 회전 입력 처리 추가 |
| `Create dinosaur NPC dialogue trigger` | NPC 공룡 근처에서 대화 UI가 열리도록 구현 |
| `Update island terrain layout` | 공룡 섬 지형과 길 배치 수정 |
| `Fix camera follow jitter` | 플레이어 이동 중 카메라 떨림 수정 |
| `Add extinction event scene effect` | 멸망 연출용 하늘, 흔들림, 파티클 초안 추가 |

### 나쁜 예시

| Summary | 문제 |
| --- | --- |
| `fix` | 무엇을 고쳤는지 알 수 없음 |
| `update` | 어떤 파일이나 기능인지 알 수 없음 |
| `test` | 임시 작업인지 실제 변경인지 알 수 없음 |
| `asdf` | 의미 없음 |

추천 형식:

```text
동사 + 대상 + 내용
```

예시:

```text
Add NPC dialogue data
Fix player spawn position
Update main island lighting
Remove unused test prefab
```

---

## 6. 팀 작업 규칙

### 6.1 같은 씬을 동시에 수정하지 않기

Unity의 `.unity` 씬 파일은 충돌이 나면 해결하기 어렵습니다.

예를 들어 `Assets/Scenes/Main/Main.unity`를 여러 명이 동시에 수정하면 충돌 가능성이 큽니다. 씬을 수정하기 전에는 팀 채팅에 먼저 알립니다.

예시:

```text
오늘 2시부터 Main.unity에서 NPC 배치 수정하겠습니다.
```

### 6.2 프리팹 단위로 나누기

가능하면 씬에 모든 것을 직접 넣지 말고 프리팹으로 나눕니다.

- 플레이어: `Assets/Prefabs/Characters`
- NPC 공룡: `Assets/Prefabs/Characters`
- 상호작용 아이템: `Assets/Prefabs/Items`
- 섬 오브젝트: `Assets/Prefabs/Environment`
- UI: `Assets/Prefabs/UI`

프리팹 단위로 작업하면 씬 충돌을 줄일 수 있습니다.

### 6.3 작업 담당 영역 정하기

팀원끼리 담당 영역을 나누면 충돌이 줄어듭니다.

| 담당 | 예시 작업 |
| --- | --- |
| Player | 이동, 점프, 카메라, 입력 |
| NPC | 공룡 배치, 대화 트리거, 대화 데이터 |
| World | 섬 지형, 길, 환경 오브젝트 |
| UI | 대화창, 메뉴, 엔딩 화면 |
| Audio | BGM, SFX, 환경음 |
| Story | 대사, 이벤트 순서, 멸망 연출 |

---

## 7. Unity 프로젝트에서 Git에 올리는 파일

Git에 올려야 하는 주요 폴더:

```text
Assets/
Packages/
ProjectSettings/
README/
.gitignore
```

Git에 올리지 않는 폴더:

```text
Library/
Temp/
Logs/
UserSettings/
.vscode/
Build/
Builds/
```

이 폴더들은 Unity나 개인 개발 환경에서 자동 생성됩니다. 용량이 크거나 사용자마다 내용이 다르기 때문에 Git에 올리지 않습니다.

---

## 8. 프로젝트 폴더 구조

```text
Dinosaur_Game/
├─ Assets/
│  ├─ Animations/          # 캐릭터, NPC, UI 애니메이션
│  ├─ Art/                 # 3D 모델, 텍스처, UI 이미지, 이펙트
│  │  ├─ Characters/       # 플레이어와 공룡 캐릭터 리소스
│  │  ├─ Environments/     # 섬, 지형, 나무, 바위 등 환경 리소스
│  │  ├─ Props/            # 소품
│  │  └─ UI/               # UI 이미지
│  ├─ Audio/
│  │  ├─ BGM/              # 배경 음악
│  │  └─ SFX/              # 효과음
│  ├─ Materials/           # 머티리얼
│  ├─ Prefabs/             # 재사용 오브젝트
│  │  ├─ Characters/       # 플레이어, NPC 공룡
│  │  ├─ Environment/      # 섬 환경 오브젝트
│  │  ├─ Items/            # 상호작용 아이템
│  │  └─ UI/               # UI 프리팹
│  ├─ Scenes/
│  │  ├─ Main/             # 메인 게임 씬
│  │  └─ Test/             # 테스트 씬
│  ├─ Scripts/
│  │  ├─ Camera/           # 카메라 제어
│  │  ├─ Core/             # 공통 시스템
│  │  ├─ Managers/         # 게임 매니저, 씬 매니저 등
│  │  ├─ Player/           # 플레이어 로직
│  │  └─ UI/               # UI 로직
│  ├─ Settings/            # 렌더 파이프라인 및 프로젝트 에셋 설정
│  └─ Shaders/             # 셰이더
├─ Docs/                   # 기획 문서
├─ Packages/               # Unity 패키지 목록
├─ ProjectSettings/        # Unity 프로젝트 설정
├─ README/                 # 팀 가이드 문서
└─ .gitignore              # Git 제외 파일 목록
```

---

## 9. 충돌이 났을 때

충돌이 발생하면 당황하지 말고 바로 팀원에게 공유합니다.

특히 아래 파일에서 충돌이 나면 혼자 해결하지 않는 것을 권장합니다.

- `.unity` 씬 파일
- `.prefab` 프리팹 파일
- `.asset` 설정 파일
- `ProjectSettings` 내부 파일

해결 순서:

1. 어떤 파일에서 충돌이 났는지 확인합니다.
2. 누가 같은 파일을 작업했는지 팀 채팅에 공유합니다.
3. 필요한 경우 Unity를 끄고 해결합니다.
4. 해결 후 Unity에서 씬과 프리팹이 정상적으로 열리는지 확인합니다.
5. 문제가 없을 때만 커밋합니다.

---

## 10. 작업 전후 체크리스트

작업 시작 전:

- GitHub Desktop에서 `Fetch origin`을 눌렀는가?
- `Pull origin`이 필요하면 눌렀는가?
- 내가 수정할 씬이나 프리팹을 다른 팀원이 작업 중이 아닌가?
- 오늘 작업할 범위를 팀원에게 공유했는가?

커밋 전:

- Unity Console에 에러가 없는가?
- 실수로 `Library`, `Temp`, `Logs` 같은 폴더가 포함되지 않았는가?
- 변경된 파일 목록을 확인했는가?
- 커밋 메시지가 구체적인가?

푸시 후:

- GitHub에 정상적으로 올라갔는가?
- 팀원에게 어떤 작업을 완료했는지 공유했는가?
- 다음 작업자가 알아야 할 주의점이 있는가?

---

## 11. 자주 생기는 문제

### Q1. GitHub Desktop에 파일이 너무 많이 보여요.

Unity가 자동 생성한 파일일 수 있습니다. `Library`, `Temp`, `Logs`, `UserSettings`, `.vscode`는 커밋하지 않습니다.

### Q2. Pull을 했더니 Conflict가 떠요.

같은 파일을 여러 명이 수정한 상태입니다. 특히 씬이나 프리팹 충돌은 혼자 해결하기 어렵기 때문에 팀원에게 먼저 공유합니다.

### Q3. Unity에서 열었더니 씬이 깨졌어요.

최근 Pull 받은 변경사항에 씬, 프리팹, 머티리얼, 메타 파일 누락이 있을 수 있습니다. GitHub Desktop에서 변경 내역을 확인하고, 누락된 `.meta` 파일이 있는지 봅니다.

### Q4. 실수로 잘못 커밋했어요.

바로 팀원에게 말합니다. Git은 대부분 되돌릴 수 있지만, 이미 Push한 커밋을 수정할 때는 팀원 작업에 영향을 줄 수 있습니다.

### Q5. Unity `.meta` 파일도 올려야 하나요?

네. Unity의 `.meta` 파일은 에셋 연결 정보를 담고 있으므로 반드시 함께 커밋해야 합니다.

---

## 12. AI 도구와 함께 Git 사용하기

AI 도구를 사용할 때도 Git 기록을 잘 남겨야 합니다.

AI에게 커밋 메시지를 요청할 때는 변경 내용을 구체적으로 알려줍니다.

예시:

```text
다음 변경 내용에 맞는 Git 커밋 메시지를 영어로 짧게 작성해줘.

- PlayerController.cs에서 이동 속도 조절 로직 추가
- NPCDialogueTrigger.cs에서 공룡 NPC와 대화 시작 조건 구현
- Main.unity에 트리케라톱스 NPC 배치
```

예상 결과:

```text
Add dinosaur NPC dialogue trigger
```

AI가 만든 코드를 바로 커밋하지 말고 Unity에서 실행해 확인합니다.

확인할 것:

- Unity Console 에러가 없는가?
- 플레이어가 정상적으로 움직이는가?
- NPC 대화가 의도한 위치에서 시작되는가?
- 씬 저장 후 변경 파일이 너무 많아지지 않았는가?

---

## 13. 마지막 원칙

Git은 실수를 막는 도구가 아니라 실수를 기록하고 되돌릴 수 있게 해주는 도구입니다.

작업 전에는 `Pull`, 작업 후에는 `Commit`과 `Push`, 충돌이 나면 혼자 해결하지 말고 공유하는 습관을 지키면 팀 프로젝트가 훨씬 안정적으로 진행됩니다.
