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
        bool HasChild(int parentCategoryId);
        Task CreateCategory(Category category1);
        Task UpdateCategory(Category category1);
        Task DeleteCategory(int categoryId);
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
            return await _dbContext.Categories.Include("JobAndCategories").Include("CategoryParent").ToListAsync();
        }

        public bool HasChild(int parentCategoryId)
        {
            return  _dbContext.Categories.Any(c => c.CategoryParentID == parentCategoryId);

        }

        public async Task CreateCategory(Category category1)
        {
            if(category1.CategoryParentID== 0)
            {
                category1.CategoryParentID = null;
            }
            _dbContext.Categories.Add(category1);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category1)
        {
            if (category1.CategoryParentID == 0)
            {
                category1.CategoryParentID = null;
            }
            Category a = _dbContext.Categories.Where(c=>c.CategoryID==category1.CategoryID).FirstOrDefault();
            a.CategoryParentID = category1.CategoryParentID;
            a.CategoryName=category1.CategoryName;
            _dbContext.Entry(a).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            Category a = _dbContext.Categories.Where(c => c.CategoryID == categoryId).Include("JobAndCategories").FirstOrDefault();
            foreach(JobAndCategory jac in a.JobAndCategories)
            {
                _dbContext.Remove(jac);
            }
            List<Category> childCategories = _dbContext.Categories.Where(c => c.CategoryParentID == categoryId).ToList();
            foreach (Category c in childCategories)
            {
                c.CategoryParent = null;
            }
            _dbContext.Remove(a);
            await _dbContext.SaveChangesAsync();
        }
    }
}
