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
        Task<Category[]> GetAllCategory();
        Task<Category[]> GetAllChildCategory(int categoryId);
        Task<int> CountChild(int parentCategoryId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly OpusLinkDBContext _dbContext;
        public CategoryService(OpusLinkDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category[]> GetAllChildCategory(int categoryId)
        {
            if(categoryId == 0)
            {
                return await _dbContext.Categories.Where(x => x.CategoryParentID == null ).Include("JobAndCategories").ToArrayAsync();
            }
            return await _dbContext.Categories.Where(x=>x.CategoryParentID== categoryId).Include("JobAndCategories").ToArrayAsync();
        }
        public async Task<Category[]> GetAllCategory()
        {
            return await _dbContext.Categories.Include("JobAndCategories").ToArrayAsync();
        }

        public async Task<int> CountChild(int parentCategoryId)
        {
            return await _dbContext.Categories.Where(x => x.CategoryParentID == parentCategoryId).CountAsync();
            
        }
    }
}
