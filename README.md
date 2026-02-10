# NoCloseWindow_v1

- .NET WinForms으로 만든 “닫기 방지” 윈도우 프로그램입니다.

## 사용 기술

- 언어-Languages: C#
- 프레임워크-Framework: Microsoft .NET SDK 10.0.102 (x64)

## 주의사항
- 실행 여부를 반드시 확인한 뒤, 관리자 권한으로 실행하세요.
- 이 프로그램은 시스템에 심각한 손상을 일으킬 수 있습니다.

## How to build

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true