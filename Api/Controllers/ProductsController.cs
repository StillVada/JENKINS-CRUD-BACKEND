using Microsoft.AspNetCore.Mvc;
using Api.Interfaces;
using Api.Models;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления продуктами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Получить список всех продуктов
    /// </summary>
    /// <returns>Список всех продуктов</returns>
    /// <response code="200">Успешное получение списка продуктов</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    /// <summary>
    /// Получить продукт по ID
    /// </summary>
    /// <param name="id">Идентификатор продукта</param>
    /// <returns>Продукт с указанным ID</returns>
    /// <response code="200">Продукт найден</response>
    /// <response code="404">Продукт не найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Создать новый продукт
    /// </summary>
    /// <param name="product">Данные нового продукта</param>
    /// <returns>Созданный продукт</returns>
    /// <response code="201">Продукт успешно создан</response>
    /// <response code="400">Некорректные данные</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = await _productRepository.CreateAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    /// <summary>
    /// Обновить существующий продукт
    /// </summary>
    /// <param name="id">Идентификатор продукта</param>
    /// <param name="product">Обновленные данные продукта</param>
    /// <returns>Обновленный продукт</returns>
    /// <response code="200">Продукт успешно обновлен</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="404">Продукт не найден</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedProduct = await _productRepository.UpdateAsync(id, product);

        if (updatedProduct == null)
        {
            return NotFound();
        }

        return Ok(updatedProduct);
    }

    /// <summary>
    /// Удалить продукт
    /// </summary>
    /// <param name="id">Идентификатор продукта для удаления</param>
    /// <response code="204">Продукт успешно удален</response>
    /// <response code="404">Продукт не найден</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productRepository.DeleteAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
