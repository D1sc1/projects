# LightsOutGame (клиент-сервер)

Игра «Lights Out» («Огни»):
два решения, клиент на WPF (MVVM) и сервер на ASP.NET Web API, общий слой `Common`,
бизнес-логика, доступ к данным через Entity Framework 6 + PostgreSQL, DI через Ninject.

## Правила игры

Поле 5×5 из лампочек (горит / не горит). Нажатие на ячейку **переключает её саму и четырёх
ортогональных соседей** (вверх/вниз/влево/вправо). Цель — погасить все лампочки.
Счётчик ходов = количество нажатий. Новое поле собирается из решённого состояния случайными
нажатиями, поэтому **всегда решаемо**.

## Состав решений

**LightsOutGame.Server.sln** (сервер):
- `LightsOutGame.WebApi` — ASP.NET Web API (контроллеры Games/Users, Ninject-модуль, Swagger)
- `LightsOutGame.Common`, `LightsOutGame.Business`, `LightsOutGame.DataAccess.EF` — слои из
  лабораторной №9 (вариант EF): та же модель игры, те же репозитории и та же схема БД.
  В `Common/BusinessDtos` добавлены запросы и ответы для обмена клиента с сервером
- `LightsOutGame.UnitTests` — тесты репозиториев EF и UserService (из лабораторной №9)

**LightsOutGame.Client.sln** (клиент):
- `LightsOutGame.Wpf` — WPF-клиент (MVVM, окно входа + игровое поле); с сервером работает
  только через `BusinessProxy`, на серверные проекты не ссылается
- `LightsOutGame.Common` — общий слой (модели, контракты сервисов, DTO для HTTP)
- `LightsOutGame.BusinessProxy` — HTTP-прокси к серверу (реализует те же контракты сервисов)
- `LightsOutGame.ClientUnitTests` — тесты клиента (обращаются к запущенному серверу)

## Технологии

.NET Framework 4.7.2, WPF, ASP.NET Web API 5.2.9, Entity Framework 6.5.2,
EntityFramework6.Npgsql 6.4.3 + Npgsql 6.0.13 (PostgreSQL), Ninject 3.3.6,
System.Text.Json, Swashbuckle. **Требуется Windows + Visual Studio** (.NET Framework).

## Как запустить

1. **Восстановить NuGet-пакеты** в обоих решениях.

2. **Создать таблицы в PostgreSQL.** Строка подключения `EFMain` — в `LightsOutGame.WebApi/Web.config`
   (поправьте `Server/Port/Database/User Id/Password`). Затем в Package Manager Console
   (стартовый проект — `LightsOutGame.WebApi`, Default project — `LightsOutGame.DataAccess.EF`)
   выполнить `Update-Database`: миграция `Initial` создаст таблицы Users / Games / Cells.
   Можно использовать ту же БД, что и в лабораторной №9 (вариант EF) — схема совпадает.

3. **Запустить сервер.** `LightsOutGame.WebApi` — стартовый проект в `Server.sln`.
   Запомнить адрес/порт (например `https://localhost:44387/`). Swagger доступен по `/swagger`.

4. **Указать клиенту адрес сервера** в `LightsOutGame.Wpf/App.config`:
   ```xml
   <add key="ServerConnection" value="https://localhost:44387/" />
   ```
   Значение должно совпадать с реальным портом запущенного WebApi.

5. **Запустить клиент.** Открыть `Client.sln`, стартовый проект `LightsOutGame.Wpf`.
   Ввести имя игрока → откроется поле 5×5. Сервер должен быть уже запущен.

## HTTP API

| Метод | Адрес | Что делает |
|---|---|---|
| GET | `api/users/get-all` | все игроки |
| GET | `api/users/get-by-name/{userName}` | игрок по имени (`null`, если нет) |
| POST | `api/users/get-or-create` | найти или создать игрока (`{"UserName": "..."}`) |
| GET | `api/games/get-by-user-id/{userId}` | текущая игра игрока (новая, если её нет) |
| GET | `api/games/get-by-game-id/{gameId}` | игра по Id |
| POST | `api/games/make-move` | ход (`{"GameId": 1, "Row": 2, "Column": 3}`) |
| GET | `api/games/is-over/{gameId}` | закончена ли игра |
| DELETE | `api/games/remove/{gameId}` | удалить игру |

Поле передаётся массивом `Cells` из 25 значений `true/false` по строкам.
