# NoCloseWindow_1

.NET WinForms으로 만든 “닫기 방지 + 재실행 감시” 윈도우 앱입니다.

## 사용 기술

- 언어: C#
- 프레임워크: Microsoft .NET SDK 10.0.102 (x64)

## 빌드 방법

```bash
dotnet restore
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
