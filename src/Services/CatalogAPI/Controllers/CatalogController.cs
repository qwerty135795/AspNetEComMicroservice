using System.Net;
using CatalogAPI.Entities;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository productRepository,
        ILogger<CatalogController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();

        return Ok(products);
    }
    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var product = await _productRepository.GetProduct(id);
        if (product is null)
        {
            _logger.LogError($"Product with id: {id}, NotFound");
            return NotFound();
        }
        return Ok(product);
    }
    [HttpGet]
    [Route("[action]/{category}")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var products = await _productRepository.GetProductsByCategory(category);
        return Ok(products);
    }
    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateProduct(Product product)
    {
        await _productRepository.CreateProduct(product);

        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }
    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct(Product product)
    {
        return Ok(await _productRepository.UpdateProduct(product));
    }
    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        return Ok(await _productRepository.DeleteProduct(id));
    }
}