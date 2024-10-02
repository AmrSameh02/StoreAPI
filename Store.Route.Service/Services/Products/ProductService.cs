using AutoMapper;
using Store.Route.Core;
using Store.Route.Core.DTOs.Products;
using Store.Route.Core.Entities;
using Store.Route.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Store.Route.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mappedBrands = _mapper.Map<IEnumerable<TypeBrandDto>>(brands);
            return mappedBrands;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
         => _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.Repository<Product, int>().GetAllAsync());
        
        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        => _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
        
        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _unitOfWork.Repository<Product, int>().GetAsync(id);
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }
    }
}
