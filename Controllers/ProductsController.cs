using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VendingMachineApp.Data;
using VendingMachineApp.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IRepository<Product> Object { get; }

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public ProductsController(IRepository<Product> @object)
    {
        Object = @object;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("purchase/{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> PurchaseProduct(int id, [FromBody] decimal amount)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null || product.QuantityAvailable <= 0 || amount < product.Price)
        {
            return BadRequest("Invalid purchase request.");
        }

        product.QuantityAvailable -= 1;
        _context.Products.Update(product);

        var transaction = new Transaction
        {
            ProductId = id,
            Amount = amount,
            Date = DateTime.Now,
            User = await _context.Users.FindAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value)
        };
        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return Ok();
    }

    private bool ProductExists(int id)

    {
        return _context.Products.Any(e => e.Id == id);
    }

    internal async Task Index()
    {
        throw new NotImplementedException();
    }
}
