# NoCloseWindow_v1

- .NET WinForms으로 만든 “닫기 방지 + 재실행 감시” 윈도우 앱입니다.
- NoCloseWindow_v1 is a window that cannot be closed.

## 사용 기술

- 언어-Languages: C#
- 프레임워크-Framework: Microsoft .NET SDK 10.0.102 (x64)

## How to build

```bash
dotnet restore
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
