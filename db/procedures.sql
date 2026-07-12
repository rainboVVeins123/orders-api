-- Запрос 1: сумма заказов со статусом "Выполнен" по каждому клиенту,
-- произведенных в день рождения клиента (совпадают день и месяц)
CREATE OR REPLACE FUNCTION get_birthday_orders_total()
RETURNS TABLE (
    client_id    INT,
    first_name   VARCHAR,
    last_name    VARCHAR,
    total_amount NUMERIC
)
LANGUAGE sql
AS $$
    SELECT c.id,
           c.first_name,
           c.last_name,
           SUM(o.amount) AS total_amount
    FROM clients c
    JOIN orders  o ON o.client_id = c.id
    WHERE o.status = 2  -- Выполнен
      AND EXTRACT(MONTH FROM o.created_at) = EXTRACT(MONTH FROM c.birth_date)
      AND EXTRACT(DAY   FROM o.created_at) = EXTRACT(DAY   FROM c.birth_date)
    GROUP BY c.id, c.first_name, c.last_name
    ORDER BY c.id;
$$;

-- Запрос 2: список часов от 00 до 23 в порядке убывания
-- со средним чеком за каждый час по заказам со статусом "Выполнен".
-- Средний чек = Сумма заказов / Кол-во заказов
CREATE OR REPLACE FUNCTION get_hourly_average_check()
RETURNS TABLE (
    hour          INT,
    average_check NUMERIC
)
LANGUAGE sql
AS $$
    SELECT h.hour::INT,
           ROUND(COALESCE(SUM(o.amount) / NULLIF(COUNT(o.id), 0), 0), 2) AS average_check
    FROM generate_series(0, 23) AS h(hour)
    LEFT JOIN orders o
           ON EXTRACT(HOUR FROM o.created_at) = h.hour
          AND o.status = 2  -- Выполнен
    GROUP BY h.hour
    ORDER BY h.hour DESC;
$$;
