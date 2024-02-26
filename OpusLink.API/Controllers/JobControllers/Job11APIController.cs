﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job11APIController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;
        public Job11APIController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await categoryService.GetAllCategory();
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categories);
            foreach (var category in result)
            {
                if (await categoryService.CountChild(category.CategoryID) > 0)
                {
                    category.HasChildCategory = true;
                }
            }
            return Ok(result);
        }
        [HttpPost("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory([FromBody] Filter filter)
        {
            int numberOfPage;
            var categories = await categoryService.GetAllCategory();
            var categoriesResultAfterSearch = Search(categories, filter, out numberOfPage);
            List<GetCategoryResponse> result = _mapper.Map<List<GetCategoryResponse>>(categoriesResultAfterSearch);
            result.Add(new GetCategoryResponse() { CategoryName = ""+numberOfPage });
            return Ok(result);
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryRequest category)
        {
            Category category1 = _mapper.Map<Category>(category);
            await categoryService.CreateCategory(category1);
            return Ok();
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] PutCategoryRequest category)
        {
            Category category1 = _mapper.Map<Category>(category);
            await categoryService.UpdateCategory(category1);
            return Ok();
        }
        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            await categoryService.DeleteCategory(categoryId);
            return Ok();
        }
        private List<Category> Search(List<Category> categories, Filter filter, out int numberOfPage)
        {
            List<Category> result = new List<Category>();
            result = categories.ToList();
            ////loc theo category
            //if (filter.CategoryIDs.Count == 0)
            //{
            //    result = jobs.ToList();
            //}
            //else
            //{
            //    foreach (Job job in jobs)
            //    {
            //        foreach (JobAndCategory jac in job.JobAndCategories)
            //        {
            //            bool nextJob = false;
            //            foreach (int categoryID in filter.CategoryIDs)
            //            {
            //                if (categoryID == jac.CategoryID)
            //                {
            //                    result.Add(job); nextJob = true; break;
            //                }
            //            }
            //            if (nextJob)
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}

            ////loc theo status
            //if (filter.Statuses.Count > 0)
            //{
            //    for (int i = result.Count - 1; i >= 0; i--)
            //    {
            //        bool isOke = false;
            //        foreach (int status in filter.Statuses)
            //        {
            //            if (result[i].Status == status)
            //            {
            //                isOke = true; break;
            //            }
            //        }
            //        if (isOke == false)
            //        {
            //            result.RemoveAt(i);
            //        }
            //    }
            //}
            ////max job >= min filter && min job <= max filter
            //for (int i = result.Count - 1; i >= 0; i--)
            //{
            //    if (result[i].BudgetTo >= filter.BudgetMin && result[i].BudgetFrom <= filter.BudgetMax)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        result.RemoveAt(i);
            //    }
            //}
            ////date
            //for (int i = result.Count - 1; i >= 0; i--)
            //{
            //    if (result[i].DateCreated >= filter.DateMin && result[i].DateCreated <= filter.DateMax)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        result.RemoveAt(i);
            //    }
            //}
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].CategoryName.ToLower().Contains(filter.SearchStr.ToLower()))
                    {
                        continue;
                    }
                    else
                    {
                        result.RemoveAt(i);
                    }
                }
            }
            //loc theo page
            numberOfPage = result.Count / 10;
            if (result.Count % 10 > 0)
            {
                numberOfPage++;
            }
            return result.Skip((filter.PageNumber - 1) * 10).Take(10).ToList();
        }
    }
}