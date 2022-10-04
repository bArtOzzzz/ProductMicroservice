using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Models.Request;
using ProductMicroservice.Models.Response;
using Services.Abstract;
using Services.Dto;

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;

        public ProductsController(IProductsService productsService,
                                  IMapper mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        public async Task<ActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();

            var product = await _productsService.GetAllAsync();

            return Ok(_mapper.Map<List<ProductResponse>>(product));
        }

        [HttpGet("{productId}", Name = "GetByIdAsync")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetByIdAsync(Guid productId)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            var product = await _productsService.GetByIdAsync(productId);

            return Ok(_mapper.Map<ProductResponse>(product));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateAsync(ProductModel product)
        {
            if (product == null || !ModelState.IsValid)
                return NotFound();

            var productMap = _mapper.Map<ProductDto>(product);

            Guid productId;

            if (productMap != null)
                productId = await _productsService.CreateAsync(productMap);
            else
                return NotFound();

            return CreatedAtRoute("GetByIdAsync", new { id = productId }, product);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> UpdateAsync(Guid productId, ProductModel product)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist || product == null || !ModelState.IsValid)
                return NotFound();

            var productMap = _mapper.Map<ProductDto>(product);

            await _productsService.UpdateAsync(productId, productMap);

            return Ok(productId);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteAsync(Guid productId)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            Task<ProductDto> productToDelete = _productsService.GetByIdAsync(productId)!;

            await _productsService.DeleteAsync(productId, await productToDelete);

            return NoContent();
        }
    }
}
