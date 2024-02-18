using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.JobServices
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategory();
        Task<List<Category>> GetAllChildCategory(int categoryId);
        Task<int> CountChild(int parentCategoryId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly OpusLinkDBContext _dbContext;
        public CategoryService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Category>> GetAllChildCategory(int categoryId)
        {
            if(categoryId == 0)
            {
                return await _dbContext.Categories.Where(x => x.CategoryParentID == null ).Include("JobAndCategories").ToListAsync();
            }
            return await _dbContext.Categories.Where(x=>x.CategoryParentID== categoryId).Include("JobAndCategories").ToListAsync();
        }
        public async Task<List<Category>> GetAllCategory()
        {
            return await _dbContext.Categories.Include("JobAndCategories").ToListAsync();
        }

        public async Task<int> CountChild(int parentCategoryId)
        {
            return await _dbContext.Categories.Where(x => x.CategoryParentID == parentCategoryId).CountAsync();
            
        }
    }
}
