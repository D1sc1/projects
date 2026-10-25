# LightsOutGame — вариант EF

Слоистая архитектура, Repository + DTO + Service, DI через Ninject, WPF/MVVM.
Доступ к данным — Entity Framework 6 (DbContext + миграции).
Открывается файлом **LightsOutGame.sln** (Visual Studio, .NET Framework 4.7.2).

## Проекты
- LightsOutGame.Common         — контракты, модели, DTO, константы, NinjectKernel
- LightsOutGame.Business       — сервисы (игровая логика в GameService)
- LightsOutGame.DataAccess.EF  — DbContext, Entities (Game/Cell/User), репозитории EF
- LightsOutGame.Wpf            — MVVM, стартовый проект
- LightsOutGame.UnitTests      — MSTest, тесты EF-репозиториев и UserService

## Запуск
1. Создать БД PostgreSQL, прописать "EFMain" в LightsOutGame.Wpf/App.config.
2. В Package Manager Console (Default project — LightsOutGame.DataAccess.EF) выполнить
       Update-Database
   Миграция Initial уже лежит в Migrations: она создаёт таблицы Users / Games / Cells
   со связями (каскадное удаление) и индексами. При изменении сущностей:
   Add-Migration <имя>, затем Update-Database.
3. Restore NuGet → F5.
   Пакеты: EntityFramework 6.5.2, EntityFramework6.Npgsql 6.4.3, Npgsql 6.0.13, Ninject 3.3.6.

## Механика (1-в-1 из исходного проекта)
- поле 5x5, клик инвертирует клетку и 4 соседей (крест); победа — все лампы выключены;
- перемешивание — 8..15 нажатий + страховка.
