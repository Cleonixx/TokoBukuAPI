using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TokoBukuAPI.Data;
using TokoBukuAPI.Models;

namespace TokoBukuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly DbHelper _db;

        public BooksController(DbHelper db)
        {
            _db = db;
        }

        // GET: api/books
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var books = new List<Book>();
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM books ORDER BY id", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetInt32(0),
                        CategoryId = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Author = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Stock = reader.GetInt32(5),
                        CreatedAt = reader.GetDateTime(6),
                        UpdatedAt = reader.GetDateTime(7)
                    });
                }
                return Ok(new { status = "success", data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // GET: api/books/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM books WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var book = new Book
                    {
                        Id = reader.GetInt32(0),
                        CategoryId = reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Author = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Stock = reader.GetInt32(5),
                        CreatedAt = reader.GetDateTime(6),
                        UpdatedAt = reader.GetDateTime(7)
                    };
                    return Ok(new { status = "success", data = book });
                }
                return NotFound(new { status = "error", message = "Buku tidak ditemukan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // POST: api/books
        [HttpPost]
        public IActionResult Create([FromBody] Book book)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO books (category_id, title, author, price, stock, created_at, updated_at) VALUES (@category_id, @title, @author, @price, @stock, NOW(), NOW()) RETURNING id", conn);
                cmd.Parameters.AddWithValue("category_id", book.CategoryId);
                cmd.Parameters.AddWithValue("title", book.Title);
                cmd.Parameters.AddWithValue("author", book.Author);
                cmd.Parameters.AddWithValue("price", book.Price);
                cmd.Parameters.AddWithValue("stock", book.Stock);
                var newId = cmd.ExecuteScalar();
                return CreatedAtAction(nameof(GetById), new { id = newId },
                    new { status = "success", message = "Buku berhasil ditambahkan", data = new { id = newId } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // PUT: api/books/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "UPDATE books SET category_id = @category_id, title = @title, author = @author, price = @price, stock = @stock, updated_at = NOW() WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("category_id", book.CategoryId);
                cmd.Parameters.AddWithValue("title", book.Title);
                cmd.Parameters.AddWithValue("author", book.Author);
                cmd.Parameters.AddWithValue("price", book.Price);
                cmd.Parameters.AddWithValue("stock", book.Stock);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Buku tidak ditemukan" });
                return Ok(new { status = "success", message = "Buku berhasil diupdate" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // DELETE: api/books/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM books WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Buku tidak ditemukan" });
                return Ok(new { status = "success", message = "Buku berhasil dihapus" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
    }
}