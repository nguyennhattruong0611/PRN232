using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DAO;
using DataAccessObjects.Models;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(Product p) => ProductDAO.DeleteProduct(p);

        public void SaveProduct(Product p) => ProductDAO.SaveProduct(p);

        public void UpdateProduct(Product p) => ProductDAO.UpdateProduct(p);

        public List<Product> GetProducts() => ProductDAO.GetProducts();

        public Product GetProductById(int id) => ProductDAO.GetProductById(id);
    }
}
