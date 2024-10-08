﻿using Microsoft.AspNetCore.Mvc;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace XuongMayBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        // GET api/product
        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 5)
        {
            var pagedProducts = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize);
            return Ok(pagedProducts);
        }

        // GET api/product/{id}
        /// <summary>
        /// Get a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product != null ? Ok(product) : NotFound();
        }

        // CREATE api/product
        /// <summary>
        /// Create a new product.
        /// Only accessible by users with the Manager role.
        /// </summary>
        /// <param name="product">The product details to create.</param>
        /// <returns>The created product details.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductModelView product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }

            try
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UPDATE api/product/{id}
        /// <summary>
        /// Update an existing product.
        /// Only accessible by users with the Manager role.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product details.</param>
        /// <returns>No content if the update is successful.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductModelView productModel)
        {
            if (productModel == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, productModel);
                return updatedProduct != null ? NoContent() : NotFound("Product not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH api/product/{id} to update status to "Unavailable"
        /// <summary>
        /// Update product status to Unavailable by ID.
        /// Only accessible by users with the Manager role.
        /// </summary>
        /// <param name="id">The ID of the product to update status.</param>
        /// <returns>No content if the update is successful.</returns>
        [Authorize(Roles = "Manager")]
        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var isUnavailable = await _productService.DeleteProductAsync(id);
                return isUnavailable ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
