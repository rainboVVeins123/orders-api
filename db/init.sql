-- Схема БД: клиенты и заказы

CREATE TABLE IF NOT EXISTS clients (
    id          SERIAL PRIMARY KEY,
    first_name  VARCHAR(100) NOT NULL,
    last_name   VARCHAR(100) NOT NULL,
    birth_date  DATE         NOT NULL
);

-- Статусы заказа: 0 - Не обработан, 1 - Отменен, 2 - Выполнен
CREATE TABLE IF NOT EXISTS orders (
    id          SERIAL PRIMARY KEY,
    amount      NUMERIC(18, 2) NOT NULL CHECK (amount >= 0),
    created_at  TIMESTAMP      NOT NULL DEFAULT now(),
    status      SMALLINT       NOT NULL DEFAULT 0 CHECK (status IN (0, 1, 2)),
    client_id   INT            NOT NULL REFERENCES clients (id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_orders_client_id ON orders (client_id);
CREATE INDEX IF NOT EXISTS ix_orders_status    ON orders (status);
