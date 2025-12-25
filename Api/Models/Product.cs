namespace Api.Models;

/// <summary>
/// Модель продукта
/// </summary>
public class Product
{
    /// <summary>
    /// Уникальный идентификатор продукта
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название продукта
    /// </summary>
    /// <example>Ноутбук Lenovo</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание продукта
    /// </summary>
    /// <example>Высокопроизводительный ноутбук для разработчиков</example>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Цена продукта
    /// </summary>
    /// <example>1299.99</example>
    public decimal Price { get; set; }

    /// <summary>
    /// Дата создания продукта
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата последнего обновления продукта
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
