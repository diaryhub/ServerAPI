# 1. 빌드 스테이지
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . ./

# 패키지 복원(restore)과 압축(publish)을 한 번에 실행
RUN dotnet publish -c Release -o /out

# 2. 실행 스테이지
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /out .
ENTRYPOINT ["dotnet", "ServerAPI.dll"]