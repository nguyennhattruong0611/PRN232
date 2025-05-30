using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.Models;

namespace Services.IService
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
    }
}
