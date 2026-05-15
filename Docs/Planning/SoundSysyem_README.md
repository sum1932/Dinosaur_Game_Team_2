# 역할 부여
당신은 유니티(Unity) 엔진 및 C# 아키텍처에 정통한 시니어 클라이언트 개발자입니다. 
제공된 명세서에 따라 확장성 있고 최적화된 사운드 시스템(Sound System) 코드를 작성해 주세요.

---

## 1. 프로젝트 환경 및 기술 스택
*   **엔진 버전:** Unity 6
*   **사용 언어:** C#
*   **주요 기술:** 
    *   리소스 로드: **Addressables** (비동기 로드)
    *   볼륨/채널 제어: **AudioMixer**
    *   최적화: **Object Pooling** (SFX용)
    *   비동기 처리: **UniTask** (또는 Unity Coroutine)

---

## 2. 시스템 아키텍처 및 역할
사운드 시스템은 전역에서 접근 가능한 `SoundManager`를 중심으로, 3개의 하위 컨트롤러가 역할을 분담하는 구조로 설계합니다.

1.  **SoundManager:** 외부에서 사운드를 재생/정지할 때 호출하는 단일 진입점(Facade / Singleton).
2.  **BGMController:** 배경음악 전담. 크로스페이드(Crossfade) 지원.
3.  **SFXController:** 효과음 전담. 다중 재생 및 오브젝트 풀링 관리.
4.  **AudioResourceLoader:** Addressables를 이용한 AudioClip 비동기 로드 및 캐싱.

---

## 3. 모듈별 상세 구현 지침 (How to Implement)

### A. SoundManager (최상위 관리자)
*   `MonoBehaviour`를 상속받는 Singleton 형태로 구현합니다 (`DontDestroyOnLoad` 처리).
*   초기화 시 `AudioMixer` 리소스를 로드하고, Master, BGM, SFX 그룹을 세팅합니다.
*   **API 구성:** `PlayBGM(string key)`, `PlaySFX(string key, Vector3 position = default)`, `SetVolume(AudioType type, float volume)` 등의 퍼블릭 메서드를 제공합니다.
*   데시벨(dB) 변환 로직 적용: UI에서 넘어오는 0.0 ~ 1.0 볼륨 값을 AudioMixer에 맞게 선형 또는 로그 스케일 dB 값(예: -80dB ~ 0dB)으로 변환하는 수식을 포함하세요.

### B. BGMController (배경음 제어)
*   동시 재생을 위해 2개의 `AudioSource`를 배열 또는 리스트로 가집니다.
*   **크로스페이드 구현:** 새로운 BGM 재생 요청이 오면, 현재 재생 중인 `AudioSource`의 볼륨은 `Mathf.Lerp`를 사용하여 지정된 시간(예: 1.5초) 동안 서서히 0으로 줄이고(Fade-Out), 다른 `AudioSource`에 새로운 클립을 할당하여 볼륨을 0에서 목표 볼륨까지 올립니다(Fade-In).
*   비동기 페이징 처리를 위해 UniTask나 Coroutine을 사용하세요.

### C. SFXController & AudioPool (효과음 제어)
*   **Object Pooling 구현:** 게임 시작 시 기본 개수(예: 10개)의 `GameObject`(AudioSource 컴포넌트 포함)를 생성하여 `Queue`에 보관합니다. 풀이 부족하면 동적으로 추가 생성(Expand)하도록 구현합니다.
*   **재생 및 반환:** 
    1. 풀에서 `AudioSource`를 꺼냅니다.
    2. 할당받은 `AudioClip`을 넣고 `Play()`를 호출합니다.
    3. 클립의 길이(`clip.length`)만큼 대기한 후, 자동으로 풀에 반환(Return)하는 로직을 작성하세요.
*   **동시 재생 제한 방어 코드:** 동일한 `AudioClip` 키가 0.1초 이내에 너무 많이(예: 5개 이상) 호출되면 무시하거나 볼륨을 낮춰서 재생하는 스로틀링(Throttling) 로직을 추가하세요. (사운드 찢어짐 방지)

### D. AudioResourceLoader (리소스 관리)
*   `Addressables.LoadAssetAsync<AudioClip>(key)`를 사용하여 클립을 비동기로 로드합니다.
*   **캐싱(Caching):** 한 번 로드된 SFX 클립은 `Dictionary<string, AudioClip>`에 캐싱해두어 다음 호출 시 로드 시간을 없앱니다.
*   BGM처럼 용량이 크고 씬에 종속적인 클립은 재생이 끝난 후 메모리 누수가 없도록 명시적으로 언로드(Release)하는 로직을 작성하세요.

---

## 4. 제약 사항 및 코딩 컨벤션
*   모든 코드는 `SoundSystem`이라는 `namespace` 안에 작성하세요.
*   하드코딩된 문자열을 지양하고, 오디오 타입(BGM, SFX, Voice)은 `enum`으로 정의하세요.
*   NullReferenceException을 방지하기 위해 로드된 클립이나 딕셔너리에 대한 방어 코드를 철저히 작성하세요.
*   각 클래스의 역할이 뚜렷하게 분리되도록, `SoundManager` 클래스 하나에 모든 로직을 몰아넣는(God Class) 것을 절대 금지합니다.

위 명세에 따라 각 스크립트의 전체 C# 코드를 작성하고, 적용 방법에 대한 짧은 가이드를 함께 제공해 주세요.