using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TokoBukuAPI.Data;
using TokoBukuAPI.Models;

namespace TokoBukuAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly DbHelper _db;

        public OrdersController(DbHelper db)
        {
            _db = db;
        }

        // GET: api/orders
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var orders = new List<Order>();
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM orders ORDER BY id", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        Id = reader.GetInt32(0),
                        BookId = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        TotalPrice = reader.GetDecimal(3),
                        CustomerName = reader.GetString(4),
                        CreatedAt = reader.GetDateTime(5),
                        UpdatedAt = reader.GetDateTime(6)
                    });
                }
                return Ok(new { status = "success", data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // GET: api/orders/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT * FROM orders WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var order = new Order
                    {
                        Id = reader.GetInt32(0),
                        BookId = reader.GetInt32(1),
                        Quantity = reader.GetInt32(2),
                        TotalPrice = reader.GetDecimal(3),
                        CustomerName = reader.GetString(4),
                        CreatedAt = reader.GetDateTime(5),
                        UpdatedAt = reader.GetDateTime(6)
                    };
                    return Ok(new { status = "success", data = order });
                }
                return NotFound(new { status = "error", message = "Order tidak ditemukan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                // Cek apakah book_id ada
                using var checkCmd = new NpgsqlCommand("SELECT id FROM books WHERE id = @book_id", conn);
                checkCmd.Parameters.AddWithValue("book_id", order.BookId);
                var bookExists = checkCmd.ExecuteScalar();
                if (bookExists == null)
                    return NotFound(new { status = "error", message = "Buku tidak ditemukan" });

                using var cmd = new NpgsqlCommand(
                    "INSERT INTO orders (book_id, quantity, total_price, customer_name, created_at, updated_at) VALUES (@book_id, @quantity, @total_price, @customer_name, NOW(), NOW()) RETURNING id", conn);
                cmd.Parameters.AddWithValue("book_id", order.BookId);
                cmd.Parameters.AddWithValue("quantity", order.Quantity);
                cmd.Parameters.AddWithValue("total_price", order.TotalPrice);
                cmd.Parameters.AddWithValue("customer_name", order.CustomerName);
                var newId = cmd.ExecuteScalar();
                return CreatedAtAction(nameof(GetById), new { id = newId },
                    new { status = "success", message = "Order berhasil ditambahkan", data = new { id = newId } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // PUT: api/orders/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Order order)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "UPDATE orders SET book_id = @book_id, quantity = @quantity, total_price = @total_price, customer_name = @customer_name, updated_at = NOW() WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("book_id", order.BookId);
                cmd.Parameters.AddWithValue("quantity", order.Quantity);
                cmd.Parameters.AddWithValue("total_price", order.TotalPrice);
                cmd.Parameters.AddWithValue("customer_name", order.CustomerName);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Order tidak ditemukan" });
                return Ok(new { status = "success", message = "Order berhasil diupdate" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        // DELETE: api/orders/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM orders WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("id", id);
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    return NotFound(new { status = "error", message = "Order tidak ditemukan" });
                return Ok(new { status = "success", message = "Order berhasil dihapus" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
    }
}