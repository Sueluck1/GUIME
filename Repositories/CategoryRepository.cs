using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
     public class CategoryRepository : ICategoryRepository
    {
        public async Task Add(Category category)
        {
            await CategoryDAO.Instance.Add(category);
        }

        public async Task Delete(int id)
        {
            await CategoryDAO.Instance.Delete(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await CategoryDAO.Instance.GetCategoryAll();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await CategoryDAO.Instance.GetCategoryById(id);
        }

        public async Task Update(Category category)
        {
            await CategoryDAO.Instance.Update(category);
        }
		public async Task<int> GetCategoryCount()
		{
			return await CategoryDAO.Instance.GetCategoryCount();
		}
	}
}
