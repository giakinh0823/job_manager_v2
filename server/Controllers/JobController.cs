using AutoMapper;
using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto.Job;
using server.Dto.User;
using server.Models;
using server.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace server.Controllers
{
    [ApiController]
    [Route("/api/v1/jobs")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "USER")]
    public class JobController : Controller
    {

        private readonly IJobRepository jobRepository;
        private readonly IMapper _mapper;

        public JobController(IJobRepository jobRepository, IMapper mapper)
        {
            this.jobRepository = jobRepository;
            this._mapper = mapper;
        }


        [HttpGet]
        public Task<IActionResult> Search()
        {
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);

            return Task.FromResult<IActionResult>(Ok(jobRepository.All()));
        }

        [HttpPost]
        public Task<IActionResult> Add([FromBody] JobCreateRequest request)
        {
            if(request == null)
            {
                throw new ApplicationException("Không thể tạo job khi request rỗng");
            }

            AccessTokenPayload payload = CommonUtil.GetPayload(HttpContext.Request);
            Job job = _mapper.Map<Job>(request);
            job.UserId = payload.UserId;
            job.CreatedAt = DateTime.Now;
            job.UpdatedAt = DateTime.Now;
            jobRepository.Add(job);

            return Task.FromResult<IActionResult>(Ok(job));
        }
    }
}
