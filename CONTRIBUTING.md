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