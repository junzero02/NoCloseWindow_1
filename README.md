# NoCloseWindow_v2

- .NET WinForms으로 만든 “닫기 방지” 윈도우 프로그램입니다.

## 사용 기술

- 언어-Languages: C#
- 프레임워크-Framework: Microsoft .NET SDK 10.0.102 (x64)

## 주의사항
- 실행 여부를 반드시 확인한 뒤, 관리자 권한으로 실행하세요.
- 이 프로그램은 시스템에 직접적인 손상을 주지 않도록 설계되었으나, 예기치 못한 상황에 따라 손상이 발생할 가능성이 있습니다.

## How to build

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true