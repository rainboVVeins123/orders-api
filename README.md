# OrdersApi

REST API для работы с клиентами и заказами. Учебная практика.

## Стек

- .NET 8, ASP.NET Core Web API
- PostgreSQL 16
- Entity Framework Core (Npgsql)
- xUnit (Unit-тесты)
- GitLab CI (сборка, тестирование, развертывание)

## Структура

```
src/OrdersApi/        - REST API (Controllers -> Services -> EF Core)
tests/OrdersApi.Tests - Unit-тесты сервисного слоя
db/init.sql           - SQL-скрипт создания схемы БД PostgreSQL
db/procedures.sql     - хранимые функции PostgreSQL для отчетных запросов
Dockerfile            - образ приложения для развертывания
docker-compose.yml    - развертывание API вместе с PostgreSQL
docker-compose.ci.yml - развертывание API и PostgreSQL в GitLab CI
.gitlab-ci.yml        - конвейер CI/CD
```

## Запуск

### Docker

Полное развертывание API и PostgreSQL:

```bash
docker compose up -d --build
```

При первом создании контейнера БД автоматически выполняются SQL-скрипты:
`db/init.sql`, затем `db/procedures.sql`.

Swagger UI: http://localhost:8080/swagger

Остановить контейнеры:

```bash
docker compose down
```

### Локально

1. Указать строку подключения (пароль) в
   `src/OrdersApi/appsettings.Development.json`.

2. Создать БД PostgreSQL и выполнить SQL-скрипты из папки `db/`
   в указанном порядке:

```bash
psql -U postgres -d orders_db -f db/init.sql
psql -U postgres -d orders_db -f db/procedures.sql
```

3. Собрать и запустить:

```bash
dotnet restore
dotnet run --project src/OrdersApi
```

Swagger UI: http://localhost:5140/swagger

## Тесты

Unit-тесты (EF InMemory, БД не требуется):

```bash
dotnet test tests/OrdersApi.Tests
```

## CI/CD (GitLab)

Конвейер `.gitlab-ci.yml`: build -> unit-tests -> deploy
(развертывание API и PostgreSQL через Docker Compose).

В deploy-этапе создаются контейнеры приложения и PostgreSQL.
SQL-скрипты `db/init.sql` и `db/procedures.sql` выполняются внутри контейнера БД.

## Docker

```bash
docker compose up -d --build
```

## Архитектура

Применены паттерны: Dependency Injection, Service Layer
(бизнес-логика в `Services`, контроллеры только принимают запросы),
DTO (модели запросов и ответов отделены от сущностей БД).
Роль Repository/Unit of Work выполняет Entity Framework Core
(DbSet / DbContext).

## Методы API

| Метод | Путь | Описание |
|---|---|---|
| GET | /api/clients | Список клиентов (фильтры: id, firstName, lastName, birthDateFrom/To; page, pageSize) |
| GET | /api/clients/{id} | Клиент по id |
| POST | /api/clients | Создание клиента |
| PUT | /api/clients/{id} | Редактирование клиента |
| DELETE | /api/clients/{id} | Удаление клиента |
| GET | /api/orders | Список заказов (фильтры: id, amountFrom/To, createdFrom/To, status, clientId; page, pageSize) |
| GET | /api/orders/{id} | Заказ по id |
| POST | /api/orders | Создание заказа |
| PUT | /api/orders/{id} | Редактирование заказа |
| DELETE | /api/orders/{id} | Удаление заказа |
| GET | /api/reports/birthday-orders-total | Сумма выполненных заказов по клиентам в их день рождения |
| GET | /api/reports/hourly-average-check | Средний чек по часам (00-23, по убыванию) |
