# Orders API

REST API для работы с клиентами и заказами.

Проект сделан на ASP.NET Core с PostgreSQL. Реализованы CRUD-операции, фильтрация, пагинация и отчетные запросы к базе данных.

## Стек технологий

- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL 16
- Docker / Docker Compose
- Swagger / OpenAPI
- xUnit
- Moq
- GitLab CI/CD

## Возможности

### Клиенты

В API реализована работа с клиентами:

- получение списка клиентов;
- получение клиента по id;
- создание клиента;
- редактирование клиента;
- удаление клиента;
- фильтрация;
- постраничный вывод.

### Заказы

Для заказов доступны основные операции:

- получение списка заказов;
- получение заказа по id;
- создание заказа;
- редактирование заказа;
- удаление заказа;
- фильтрация;
- постраничный вывод.

### Отчеты

Отдельно реализованы два отчетных запроса:

1. Сумма выполненных заказов каждого клиента, которые были сделаны в день рождения клиента.
2. Средний чек по каждому часу суток для выполненных заказов.

Эти запросы вынесены в SQL-функции PostgreSQL и вызываются из приложения через отдельный сервис.

## Структура базы данных

### `clients`

| Поле | Тип |
|---|---|
| `id` | integer |
| `first_name` | varchar(100) |
| `last_name` | varchar(100) |
| `birth_date` | date |

### `orders`

| Поле | Тип |
|---|---|
| `id` | integer |
| `amount` | numeric(18, 2) |
| `created_at` | timestamp |
| `status` | smallint |
| `client_id` | integer |

Статусы заказа:

| Значение | Статус |
|---|---|
| `0` | NotProcessed |
| `1` | Cancelled |
| `2` | Completed |

Схема базы данных описана SQL-скриптами:

- `db/init.sql`
- `db/procedures.sql`

EF migrations в проекте не используются.

## REST API

### Клиенты

| Метод | Endpoint |
|---|---|
| GET | `/api/Clients` |
| GET | `/api/Clients/{id}` |
| POST | `/api/Clients` |
| PUT | `/api/Clients/{id}` |
| DELETE | `/api/Clients/{id}` |

Параметры фильтрации:

- `id`
- `firstName`
- `lastName`
- `birthDateFrom`
- `birthDateTo`

Пагинация:

- `page`
- `pageSize`

### Заказы

| Метод | Endpoint |
|---|---|
| GET | `/api/Orders` |
| GET | `/api/Orders/{id}` |
| POST | `/api/Orders` |
| PUT | `/api/Orders/{id}` |
| DELETE | `/api/Orders/{id}` |

Параметры фильтрации:

- `id`
- `amountFrom`
- `amountTo`
- `createdFrom`
- `createdTo`
- `status`
- `clientId`

Пагинация:

- `page`
- `pageSize`

### Отчеты

| Метод | Endpoint |
|---|---|
| GET | `/api/Reports/birthday-orders-total` |
| GET | `/api/Reports/hourly-average-check` |

## Запуск через Docker

Проще всего запустить проект через Docker Compose.

Он поднимает сразу два контейнера:

- приложение ASP.NET Core;
- PostgreSQL.

Команда запуска:

```bash
docker compose up -d --build
```

После запуска приложение будет доступно по адресу:

```text
http://localhost:8080
```

Swagger UI:

```text
http://localhost:8080/swagger
```

При первом запуске контейнера базы данных автоматически выполняются SQL-скрипты из папки `db`.

Остановить контейнеры:

```bash
docker compose down
```

Если нужно пересоздать базу данных с нуля:

```bash
docker compose down -v
docker compose up -d --build
```

## Локальный запуск

Для локального запуска без Docker нужно указать строку подключения к PostgreSQL в файле:

```text
src/OrdersApi/appsettings.Development.json
```

После этого можно запустить приложение командой:

```bash
dotnet run --project src/OrdersApi
```

## Тестирование

Запуск тестов:

```bash
dotnet test
```

В проекте есть unit-тесты сервисного слоя.

Интеграционные тесты с реальной PostgreSQL не используются.

## CI/CD

В проект добавлен GitLab CI/CD pipeline.

Этапы pipeline:

1. `build` - сборка проекта;
2. `test` - запуск unit-тестов;
3. `deploy` - развертывание приложения и PostgreSQL через Docker Compose.

Deploy выполняется для ветки `main`.

## Структура проекта

```text
src/OrdersApi              - основное Web API приложение
tests/OrdersApi.Tests      - unit-тесты
db/init.sql                - создание таблиц и индексов
db/procedures.sql          - SQL-функции для отчетов
Dockerfile                 - сборка Docker-образа приложения
docker-compose.yml         - локальный запуск API и PostgreSQL
docker-compose.ci.yml      - запуск API и PostgreSQL в GitLab CI
.gitlab-ci.yml             - pipeline GitLab CI/CD
```
