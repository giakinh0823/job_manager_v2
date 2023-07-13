using AutoMapper;
using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using server.Constant;
using server.CronJob;
using server.Dto.Job;
using server.Models;
using server.Utils;

namespace server.Controllers
{
    [ApiController]
    [Route("/api/v1/jobs")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "USER")]
    public class JobController : Controller
    {

        private readonly IJobRepository _jobRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobController(IJobRepository jobRepository, IMapper mapper, ISchedulerFactory schedulerFactory, ILogRepository logRepository)
        {
            this._jobRepository = jobRepository;
            this._mapper = mapper;
            this._schedulerFactory = schedulerFactory;
            this._logRepository = logRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Search()
        {
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);
            return await Task.FromResult<IActionResult>(Ok(_jobRepository.FindByUserId(payload.UserId)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JobCreateRequest request)
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
            _jobRepository.Add(job);

            if (JobConstant.Status.ACTIVE.Equals(job.Status.ToUpper()))
            {
                if (job.Expression != null)
                {
                    IScheduler scheduler = await _schedulerFactory.GetScheduler();
                    IJobDetail jobDetail = JobBuilder.Create<UserCronJob>()
                        .WithIdentity(job.JobId.ToString(), payload.UserId.ToString())
                        .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(job.JobId.ToString(), payload.UserId.ToString())
                        .WithCronSchedule(job.Expression)
                        .Build();

                    await scheduler.ScheduleJob(jobDetail, trigger);
                }
            }

            return await Task.FromResult<IActionResult>(Ok(job));
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] JobUpdateRequest request)
        {
            if (request == null)
            {
                throw new ApplicationException("Không thể tạo job khi request rỗng");
            }

            AccessTokenPayload payload = CommonUtil.GetPayload(HttpContext.Request);
            Job? job = _jobRepository.FindById(request.JobId);
            if(job == null || !job.UserId.Equals(payload.UserId))
            {
                throw new ApplicationException("Không tìm thấy job");
            }
            Job newJob = _mapper.Map<Job>(request);
            newJob.JobId = job.JobId;
            newJob.CreatedAt = job.CreatedAt;
            newJob.UpdatedAt = DateTime.Now;
            _jobRepository.Update(newJob);

            try
            {
                IScheduler scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.DeleteJob(new JobKey(newJob.JobId.ToString(), payload.UserId.ToString()));

                if (JobConstant.Status.ACTIVE.Equals(job.Status.ToUpper()))
                {
                    if (job.Expression != null)
                    {
                        IJobDetail jobDetail = JobBuilder.Create<UserCronJob>()
                            .WithIdentity(job.JobId.ToString(), payload.UserId.ToString())
                            .Build();

                        ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity(job.JobId.ToString(), payload.UserId.ToString())
                            .WithCronSchedule(job.Expression)
                            .Build();

                        await scheduler.ScheduleJob(jobDetail, trigger);
                    }
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!JobExists(request.JobId))
                {
                    return NotFound("Không tìm thấy job");
                }
                else
                {
                    throw e;
                }
            }

            return await Task.FromResult<IActionResult>(Ok(job));
        }


        [HttpDelete("{jobId}")]
        public async Task<IActionResult> Delete([FromRoute] int jobId)
        {
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);

            var job = _jobRepository.FindById(jobId);
            if (job == null || job.UserId != payload.UserId)
            {
                throw new ApplicationException("Không tìm thấy job");
            }

            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.DeleteJob(new JobKey(job.JobId.ToString(), payload.UserId.ToString()));
            List<Log> logs = _logRepository.FindByJobId(job.JobId);
            _logRepository.DeleteAll(logs);

            return await Task.FromResult<IActionResult>(Ok("Success"));
        }

        private bool JobExists(int id)
        {
            return _jobRepository.FindById(id) != null;
        }
    }
}
