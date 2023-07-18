using AutoMapper;
using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
using server.Constant;
using server.CronJob;
using server.Dto.Base;
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
        private readonly IPaymentInfoRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobController(IJobRepository jobRepository, IMapper mapper, ISchedulerFactory schedulerFactory, ILogRepository logRepository, IPaymentInfoRepository paymentRepository)
        {
            this._jobRepository = jobRepository;
            this._mapper = mapper;
            this._schedulerFactory = schedulerFactory;
            this._logRepository = logRepository;
            this._paymentRepository = paymentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Search()
        {
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);
            return await Task.FromResult<IActionResult>(Ok(_jobRepository.FindByUserId(payload.UserId)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if(id == null)
            {
                throw new ApplicationException("Không tìm thấy job");
            }
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);
            return await Task.FromResult<IActionResult>(Ok(_mapper.Map<JobResponse>(_jobRepository.FindByUserIdAndJobId(payload.UserId, id))));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JobCreateRequest request)
        {
            if(request == null)
            {
                throw new ApplicationException("Không thể tạo job khi request rỗng");
            }

            AccessTokenPayload payload = CommonUtil.GetPayload(HttpContext.Request);

            List<Job> jobs = _jobRepository.FindByUserId(payload.UserId);

            List<PaymentInfo>? paymentInfos = _paymentRepository.FindByUserId(payload.UserId);


            if (jobs.Count >= 1 && (paymentInfos == null || paymentInfos.Count <= 0 
                || paymentInfos.Any(paymentInfo => !PaymentStatusConstant.ACTIVE.Equals(paymentInfo?.Status))
                || paymentInfos.Any(paymentInfo => PaymentStatusConstant.ACTIVE.Equals(paymentInfo?.Status) && paymentInfo.EndDate <= DateTime.Now)))
            {
                throw new ApplicationException("Không thể tạo job! Bạn vui lòng nâng cấp tài khoản.");
            }

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

            job.Expression = request.Expression;
            job.Header = request.Header;
            job.Method = request.Method;
            job.Name = request.Name;
            job.Description = request.Description;
            job.UpdatedAt = DateTime.Now;
            job.Payload = request.Payload;
            job.Status = request.Status;
            job.Webhook = request.Webhook;

            _jobRepository.Update(job);

            try
            {
                IScheduler scheduler = await _schedulerFactory.GetScheduler();
                await scheduler.DeleteJob(new JobKey(job.JobId.ToString(), payload.UserId.ToString()));

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

            return await Task.FromResult<IActionResult>(Ok(_mapper.Map<JobResponse>(job)));
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
            _jobRepository.Delete(job);

            return await Task.FromResult<IActionResult>(Ok(new BaseResponse { isSuccess = true, Message = "Xóa job thành công"}));
        }

        private bool JobExists(int id)
        {
            return _jobRepository.FindById(id) != null;
        }
    }
}
