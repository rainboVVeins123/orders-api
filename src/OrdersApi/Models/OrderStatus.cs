namespace OrdersApi.Models;

/// <summary>Статус заказа.</summary>
public enum OrderStatus
{
    /// <summary>Не обработан.</summary>
    NotProcessed = 0,

    /// <summary>Отменен.</summary>
    Cancelled = 1,

    /// <summary>Выполнен.</summary>
    Completed = 2
}
