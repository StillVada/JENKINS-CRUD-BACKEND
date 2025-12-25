using System.Data;
using System.Data.SqlClient;
using Api.Interfaces;
using Api.Models;

namespace Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Name, Description, Price, CreatedAt, UpdatedAt FROM Products ORDER BY Id",
            connection);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = reader.GetDateTime(5)
            });
        }

        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "SELECT Id, Name, Description, Price, CreatedAt, UpdatedAt FROM Products WHERE Id = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = reader.GetDateTime(5)
            };
        }

        return null;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            @"INSERT INTO Products (Name, Description, Price, CreatedAt, UpdatedAt)
              OUTPUT INSERTED.Id, INSERTED.CreatedAt, INSERTED.UpdatedAt
              VALUES (@Name, @Description, @Price, @CreatedAt, @UpdatedAt)",
            connection);

        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", product.Description);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);
        command.Parameters.AddWithValue("@UpdatedAt", product.UpdatedAt);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            product.Id = reader.GetInt32(0);
            product.CreatedAt = reader.GetDateTime(1);
            product.UpdatedAt = reader.GetDateTime(2);
        }

        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            @"UPDATE Products
              SET Name = @Name, Description = @Description, Price = @Price, UpdatedAt = @UpdatedAt
              WHERE Id = @Id;
              SELECT Id, Name, Description, Price, CreatedAt, UpdatedAt FROM Products WHERE Id = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", product.Description);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = reader.GetDateTime(5)
            };
        }

        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
            "DELETE FROM Products WHERE Id = @Id",
            connection);

        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
}
