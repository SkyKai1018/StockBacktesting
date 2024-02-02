using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockBacktesting.Models;


namespace StockBacktesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly TestConnDBContext _context;

        public ItemsController(TestConnDBContext context)
        {
            _context = context;
        }

        // 获取所有项目
        [HttpGet]
        public ActionResult<List<Connection>> GetAll() =>
            _context.Connection.ToList();

        // 通过ID获取项目
        [HttpGet("{id}")]
        public ActionResult<Connection> GetById(int id)
        {
            var item = _context.Connection.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // 创建新项目
        [HttpPost]
        public ActionResult<Connection> Create(Connection connection)
        {
            _context.Connection.Add(connection);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = connection.ConnectionId }, connection);
        }

        // 更新项目
        [HttpPut("{id}")]
        public IActionResult Update(int id, Connection connection)
        {
            if (id != connection.ConnectionId)
            {
                return BadRequest();
            }

            _context.Entry(connection).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Connection.Any(e => e.ConnectionId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 删除项目
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var connection = _context.Connection.Find(id);
            if (connection == null)
            {
                return NotFound();
            }

            _context.Connection.Remove(connection);
            _context.SaveChanges();

            return NoContent();
        }
    }

}

