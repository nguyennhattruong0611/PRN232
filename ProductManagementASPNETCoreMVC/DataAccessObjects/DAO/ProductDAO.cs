using System;
using Microsoft.EntityFrameworkCore; 

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.Models;

namespace DataAccessObjects.DAO
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using var db = new Lab1Prn232Context();
                listProducts = db.Products.Include(f => f.Category).ToList();
            }
            catch (Exception e)
            {
            }
            return listProducts;
        }
        public static void SaveProduct(Product p)
        {
            try
            {
                using var context = new Lab1Prn232Context();
                context.Products.Add(p); 
                context.SaveChanges();   
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateProduct(Product p)
        {
            try
            {
                using var context = new Lab1Prn232Context();
                context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void DeleteProduct(Product p)
        {
            try
            {
                using var context = new Lab1Prn232Context();
                var p1 = context.Products.SingleOrDefault(c => c.ProductId == p.ProductId);
                context.Products.Remove(p1);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Product GetProductById(int id)
        {
            using var db = new Lab1Prn232Context();
            return db.Products.FirstOrDefault(c => c.ProductId.Equals(id));
        }

    }
}
