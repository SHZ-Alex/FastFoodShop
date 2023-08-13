using AutoMapper;
using FastFood.Services.ProductAPI.Models;
using FastFood.Services.ProductAPI.Models.Dto;
using FastFood.Services.ProductAPI.Repository.IRepository;
using FastFood.Services.ProductAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Services.ProductAPI.Controllers;

[Route("api/product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private ResponseDto _response;
    private IMapper _mapper;

    public ProductController(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var products = await _repository.GetAllAsync();
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var product = await _repository
                .GetAsync(x => x.ProductId == id);
            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }

    [HttpPost]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Post([FromBody] ProductDto request)
    {
        try
        {
            var product = _mapper.Map<Product>(request);
            await _repository.CreateAsync(product);
            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }

        return Ok(_response);
    }


    [HttpPut]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Put([FromBody] ProductDto request)
    {
        try
        {
            var product = _mapper.Map<Product>(request);
            await _repository.UpdateAsync(product);
            _response.Result = _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }

        return Ok(_response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var product = await _repository.GetAsync(x => x.ProductId == id);
            await _repository.RemoveAsync(product);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }
}