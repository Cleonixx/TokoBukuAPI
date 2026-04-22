using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TokoBukuAPI.Data;
using TokoBukuAPI.Models;

namespace TokoBukuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly DbHelper _db;

        public CategoriesController(DbHelper db)
        {
            _db = db;
        }

        // GET: api/categories
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var categories = new List<Category>();
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM categories ORDER BY id", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CreatedAt = reader.GetDateTime(3),
                        UpdatedAt = reader.GetDateTime(4)
                    });
                }
                return Ok(new { status = "success", data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // GET: api/categories/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM categories WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var category = new Category
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CreatedAt = reader.GetDateTime(3),
                        UpdatedAt = reader.GetDateTime(4)
                    };
                    return Ok(new { status = "success", data = category });
                }
                return NotFound(new { status = "error", message = "Kategori tidak ditemukan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // POST: api/categories
        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO categories (name, description, created_at, updated_at) VALUES (@name, @description, NOW(), NOW()) RETURNING id", conn);
                cmd.Parameters.AddWithValue("name", category.Name);
                cmd.Parameters.AddWithValue("description", (object?)category.Description ?? DBNull.Value);
                var newId = cmd.ExecuteScalar();
                return CreatedAtAction(nameof(GetById), new { id = newId },
                    new { status = "success", message = "Kategori berhasil ditambahkan", data = new { id = newId } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // PUT: api/categories/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "UPDATE categories SET name = @name, description = @description, updated_at = NOW() WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("name", category.Name);
                cmd.Parameters.AddWithValue("description", (object?)category.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Kategori tidak ditemukan" });
                return Ok(new { status = "success", message = "Kategori berhasil diupdate" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // DELETE: api/categories/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM categories WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Kategori tidak ditemukan" });
                return Ok(new { status = "success", message = "Kategori berhasil dihapus" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
    }
}