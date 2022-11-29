﻿using ProductMicroservice.Models.Response;
using ProductMicroservice.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Services.Dto;
using AutoMapper;

namespace ProductMicroservice.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : Controller
    {
        protected readonly IProductsService _productsService;
        protected readonly IMapper _mapper;

        public ProductsController(IProductsService productsService,
                                  IMapper mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "User, Administrator")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
                return NotFound();

            var product = await _productsService.GetAllAsync();

            var productMap = _mapper.Map<List<ProductResponse>>(product);

            if(productMap.Any())
            {
                return Ok(productMap);
            }

            return NotFound();
        }

        [HttpGet("{productId}", Name = nameof(GetByIdAsync))]
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetByIdAsync(Guid productId)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            var product = await _productsService.GetByIdAsync(productId);
            var productMap = _mapper.Map<ProductResponse>(product);

            if(productMap == null || string.IsNullOrEmpty(productMap.Name))
            {
                return NotFound();
            }

            return Ok(productMap);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateAsync(ProductModel product)
        {
            if (product == null 
                || string.IsNullOrEmpty(product.Name) 
                || !ModelState.IsValid)
            {
                return NotFound();
            }  

            var productMap = _mapper.Map<ProductDto>(product);

            Guid productId;

            if (productMap != null)
                productId = await _productsService.CreateAsync(productMap);
            else
                return NotFound();

            return CreatedAtRoute(nameof(GetByIdAsync), new {productId}, product);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> UpdateAsync(Guid productId, ProductModel product)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist 
                || product == null 
                || string.IsNullOrEmpty(product.Name) 
                || !ModelState.IsValid)
            {
                return NotFound();
            }
                
            var productMap = _mapper.Map<ProductDto>(product);

            await _productsService.UpdateAsync(productId, productMap);

            return Ok(productId);
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteAsync(Guid productId)
        {
            bool isExist = await _productsService.IsExistAsync(productId);

            if (!isExist || !ModelState.IsValid)
                return NotFound();

            Task<ProductDto> productToDelete = _productsService.GetByIdAsync(productId)!;

            bool isDeleted = await _productsService.DeleteAsync(productId, await productToDelete);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
