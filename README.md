# Nexon-Tutorial2-Data

데이터 주도 설계(ScriptableObject) + 세이브/로드로 재구성한 **2D 서바이벌 웨이브 슈터** — AI 활용 신규 패턴 학습 (Unity 6, C#)

## 목적

[Nexon-Tutorial](https://github.com/kv0630028/Nexon-Tutorial)(익숙한 기능의 빠른 구현)에 이어,
**안 써 본 패턴을 생성형 AI로 학습하며 적용**한 프로젝트.

- ScriptableObject 기반 데이터 설계
- JSON 세이브 / 런타임 로드
- 상태머신으로 게임 흐름 관리
- 데이터 조합으로 무기·적·웨이브 구성

## 플레이

| 입력 | 동작 |
|---|---|
| WASD / 화살표 | 이동 |
| 마우스 | 조준 |
| 좌클릭 | 사격 |
| 1 / 2 / 3 | 무기 교체 (Pistol / Shotgun / Burst) |
| 5 / 6 | 저장 / 불러오기 |
| R | 재시작 (새 게임) |

## 핵심 구현

### 1. ScriptableObject 데이터 주도 설계
`PlayerData` · `EnemyData` · `WeaponData` · `WaveData` 로 수치를 코드에서 분리.
`WaveManager` 는 공통 적 프리팹 하나를 스폰하고 `SetData(EnemyData)` 로 이동·공격·비주얼·체력에 주입 — 적 종류별 프리팹/클래스 분기 없음.

### 2. 무기 시스템
`WeaponData.projectilesPerShot` + `spreadAngle` 로 단발/산탄을 데이터로 표현. 무기 추가 = 에셋 하나.

### 3. 세이브 / 로드
`SaveData`(직렬화 전용) → `JsonUtility` → `persistentDataPath/save.json`.
로드 시 **씬 리로드 없이** HP·점수·웨이브를 런타임 복구 (`RestoreState`, `SetScore`, `LoadWave`).

### 4. 게임 상태머신
`GameManager` 가 `Playing / GameOver / GameClear` 를 관리, 진입점 하나로 정지·표시 처리.

### 5. 이벤트 기반 체력·점수·UI
`PlayerHealth.OnDeath`, `WaveManager.OnAllWavesCleared` 등을 구독. `isDead` 가드로 사망 로직 1회 실행 보장.

## Tutorial 1 → 2 개선점

| 항목 | Tutorial 1 | Tutorial 2 |
|---|---|---|
| 수치 관리 | 스크립트에 하드코딩 | ScriptableObject 에셋 |
| 적 다양화 | 프리팹·클래스 복제 | 공통 프리팹 + 데이터 주입 |
| 게임 흐름 | UI 3종이 각자 런타임 생성 | `GameManager` 상태머신 |
| 저장 | 없음 | JSON 세이브 / 런타임 로드 |

## 구조

```
Assets/Scripts/
├─ Data/     PlayerData · EnemyData · WeaponData · WaveData   (ScriptableObject)
├─ Player/   PlayerMovement · PlayerAttack · PlayerBullet · PlayerHealth
├─ Enemy/    EnemyMovement · EnemyAttack · EnemyHealth · EnemyVisual
├─ Wave/     WaveManager
├─ Score/    ScoreManager
├─ Save/     SaveData · SaveManager · RestartManager
└─ Game/     GameManager
```

## 환경

Unity 6 · C# · Input System · TextMeshPro
