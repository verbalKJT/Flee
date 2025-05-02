# 👥 Flee 프로젝트 기여 가이드 (GitHub Collaborator용)

## 📌 브랜치 전략

- `main`: 최종 완성 및 배포 브랜치 (직접 작업 금지)
- `dev`: 기능 병합용 개발 브랜치
- `feature/기능명`: 개별 기능 개발 브랜치
- `fix/버그명`: 버그 수정 브랜치
- `refactor/이름`: 구조 변경, 리팩토링 브랜치

### 예시
- feature/slowmotion
- fix/door-collision
- refactor/player-movement

---

## 🧑‍💻 작업 순서

1. 최신 dev 브랜치 기준으로 새 브랜치 생성
```bash
git checkout dev
git pull
git checkout -b feature/기능명

⸻

🧠 Flee 프로젝트 Git 협업 관리 가이드

⸻

🔁 브랜치 구조

main            ← 배포용 (절대 직접 작업 금지)
└── dev          ← 개발 통합 브랜치
    ├── feature/character/tae     ← 캐릭터 담당
    ├── feature/environment/jae  ← 배경 담당
    ├── feature/gimmick/sang     ← 기믹 담당

⸻

🏷 브랜치 네이밍 규칙
•feature/파트/이름  → 기능 개발용
•fix/파트/이름      → 버그 수정용
•refactor/파트/이름 → 리팩토링용

예시:
•feature/character/tae
•fix/gimmick/door-trigger
•refactor/environment/lighting

⸻

🧑‍💻 작업 순서 (팀원용)

git checkout dev
git pull
git checkout -b feature/character/tae

# 작업
git add .
git commit -m "✨ 캐릭터 이동 구현"
git push -u origin feature/character/tae

→ 이후 GitHub에서 Pull Request(PR) 생성 → 대상 브랜치: dev

⸻

📥 Pull Request 규칙
•타겟: dev
•PR 제목 예시:
[✨ 기능] 캐릭터 애니메이션 구현
[🐛 버그] 문 트리거 충돌 수정
•Merge 후: Delete branch 클릭해서 정리

⸻

💬 커밋 메시지 규칙 (이모지 사용)

이모지	의미
✨	기능 추가
🐛	버그 수정
♻️	리팩토링
🔥	코드 제거
📝	문서
🚧	작업 중
✅	테스트 완료



⸻

📈 GitHub 브랜치 흐름 확인
1.저장소 → Insights 탭
2.왼쪽 메뉴 → Network
→ 브랜치 간 병합 흐름 시각적으로 확인 가능

⸻

🧼 브랜치 정리 방법
•GitHub PR 병합 후 Delete branch
•로컬: git branch -d 브랜치명
•원격: git push origin --delete 브랜치명

⸻

💡 관리 꿀팁
•main은 오직 배포용, 직접 작업 ❌
•각자 작업 전 git pull 필수
•기능 단위로 커밋/푸시 → PR
•병합 후 브랜치 삭제까지

⸻