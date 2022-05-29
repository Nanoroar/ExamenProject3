using AutoMapper;
using ExamenProject3.Data;
using ExamenProject3.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace ExamenProject3.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product product);

        Task<IEnumerable<Product>> GetAllAsync();
        Task<bool> DeleteAsync(string articaleNumber);

        Task<Product> GetProductAsync(string articaleNumber);

        Task<ProductEntity> UpdateProductAsync(string artnr, ProductUpdate product);  

    }
    public class ProductService : IProductService
    {
        private readonly DataContext _db;

        private readonly IMapper _map;
        public ProductService(DataContext db, IMapper map)
        {
            _db = db;
            _map = map;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            //var productEntity = new ProductEntity
            //{
            //    ArticleNr = product.ArticleNumber,
            //    Name = product.Name,
            //    Price = product.Price,

            //};


            if (!await _db.Products.AnyAsync(x => x.ArticleNr == product.ArticleNumber))
            {
                var productEntity = _map.Map<ProductEntity>(product);

                var productCategoryEntity = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryName == product.CategoryName);

                if (productCategoryEntity == null)
                {
                    productEntity.Category = new ProductCategoryEntity
                    {
                        CategoryName = product.CategoryName
                    };
                }
                else
                {
                    productEntity.CategoryId = productCategoryEntity.Id;
                }

                await _db.Products.AddAsync(productEntity);
                await _db.SaveChangesAsync();

                //return new Product
                //{
                //    ArticleNumber = productEntity.ArticleNr,
                //    Name = productEntity.Name,
                //    Price = productEntity.Price,
                //    CategoryName = productEntity.Category.CategoryName
                //};

                return _map.Map<Product>(productEntity);
            }
            return null!;
        }



        //===================Delete========================

        public async Task<bool> DeleteAsync(string articaleNumber)
        {
           var productEntity =  await _db.Products.FindAsync(articaleNumber);
            if(productEntity != null)
            {
                _db.Products.Remove(productEntity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }


        //=======================Get All===========================================
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            //return _map.Map<IEnumerable<Product>>(await _db.Products.ToListAsync());
            return _map.Map<IEnumerable<Product>>(await _db.Products.Include( x => x.Category).ToListAsync());
            //var category = new ProductCategoryEntity { CategoryName =}
            //var items = new List<Product>();
            //foreach (var item in await _db.Products.ToListAsync())
            //{
            //    items.Add(new Product { ArticleNumber = item.ArticleNr, Name = item.Name, Price = item.Price, CategoryName = new ProductCategoryEntity { CategoryName = categ } );
            //}

            //return items;
        }


        //====================================Get with articalenumber=====================
        public async Task<Product> GetProductAsync(string articaleNumber)
        {
           return  _map.Map<Product>(await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x=>x.ArticleNr == articaleNumber));
        }




        //===============================Update============================

        public async Task<ProductEntity> UpdateProductAsync(string artnr, ProductUpdate product)
        {

            var productEntity = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ArticleNr == artnr);

            if ( productEntity != null)
            {
                

                if (productEntity.Name != product.Name)
                    productEntity.Name = product.Name;

                if (productEntity.Price != product.Price)
                    productEntity.Price = product.Price;

                if(productEntity.Category.CategoryName != product.CategoryName && !string.IsNullOrEmpty(product.CategoryName))
                {
                    var categoryEntity = await _db.Categories.FirstOrDefaultAsync(x => x.CategoryName == product.CategoryName);

                    if (categoryEntity == null)
                    {
                        categoryEntity = new ProductCategoryEntity { CategoryName = product.CategoryName };
                        _db.Categories.Add(categoryEntity);
                        await _db.SaveChangesAsync();   
                    }
                    productEntity.CategoryId = categoryEntity.Id;
                }

                _db.Entry(productEntity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return productEntity;
                 
            }
           
            else 
                return null!;  
        }
    }
}
