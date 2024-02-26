﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.JobServices;

namespace OpusLink.API.Controllers.JobControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job16APIController : ControllerBase
    {
        private readonly IOfferService offerService;
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;
        public Job16APIController(IOfferService offerService, ICategoryService categoryService, IMapper mapper)
        {
            this.offerService = offerService;
            this.categoryService = categoryService;
            _mapper = mapper;

        }
        [HttpPost("GetAllOffer/{userId}")]
        public async Task<IActionResult> GetAllOffer([FromBody] Filter filter, [FromRoute] int userId)
        {
            int numberOfPage;
            var offers = await offerService.GetAllOffer(userId);
            var resultAfterSearch = Search(offers, filter, out numberOfPage);
            List<GetOfferResponse> result = _mapper.Map<List<GetOfferResponse>>(resultAfterSearch);
            for(int i = 0; i < result.Count; i++)
            {
                GetJobResponse gjr = _mapper.Map<GetJobResponse>(resultAfterSearch[i].Job);
                result[i].GetJobResponse= gjr;
            }
            result.Add(new GetOfferResponse() { OfferID = numberOfPage });
            return Ok(result);
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
        private List<Offer> Search(List<Offer> offers, Filter filter, out int numberOfPage)
        {
            List<Offer> result = new List<Offer>();
            result = offers.ToList();
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].Job.JobTitle.ToLower().Contains(filter.SearchStr.ToLower()) || result[i].Job.JobContent.ToLower().Contains(filter.SearchStr.ToLower()))
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