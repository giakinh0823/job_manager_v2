using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using server.Constant;
using server.Models;
using Quartz;
using DataAccess;
using DataAccess.Repository;
using BusinessObject;

namespace server.CronJob;

public class UserCronJob : IJob
{
    private readonly IUserRepository _userRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ILogRepository _logRepository;
    private readonly ILogger<UserCronJob> _logger;

    public UserCronJob(IUserRepository userRepository, IJobRepository jobRepository, ILogRepository logRepository, ILogger<UserCronJob> logger)
    {
        _userRepository = userRepository;
        _jobRepository = jobRepository;
        _logRepository = logRepository;
        _logger = logger;
    }

    [Obsolete("Obsolete")]
    public async Task Execute(IJobExecutionContext context)
    {
        int jobId = int.Parse(context.JobDetail.Key.Name);
        int userId = int.Parse(context.JobDetail.Key.Group);
        
        Job? job = _jobRepository.FindById(jobId);
        if (job is { Webhook: { }, Method: { } })
        {
            User? user = _userRepository.FindById(userId);
            Log log = new Log()
            {
                JobId = job.JobId,
                UserId = user?.UserId ?? null,
                StartTime = DateTime.Now,
            };
            try
            {
                string? response = null;
                switch (job.Method.ToUpper())
                {
                    case "GET":
                        response = await GetApi(job.Webhook, job.Header);
                        break; 
                    case "POST":
                        response = await PostApi(job.Webhook, job.Header, job.Payload);
                        break;
                    case "PUT":
                        response = await PutApi(job.Webhook, job.Header, job.Header);
                        break;
                    case "DELETE":
                        response = await DeleteApi(job.Webhook, job.Header);
                        break;
                }
                log.Status = LogConstant.SUCESSS;
                log.Output = response;
                _logger.LogInformation($"Success call job {job.Name}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error call job {job.Name}: {e}");
                log.Status = LogConstant.FAILED;
                log.Output = e.Message;
            }
            log.EndTime = DateTime.Now;
            _logRepository.Add(log);
        }
    }

    private static async Task<string> GetApi(string url, string? headers = null)
    {
        using var client = new HttpClient();
        if (!string.IsNullOrEmpty(headers))
        {
            string[] headerLines = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string headerLine in headerLines)
            {
                string[] headerParts = headerLine.Split(':', 2);
                if (headerParts.Length == 2)
                {
                    string headerName = headerParts[0].Trim();
                    string headerValue = headerParts[1].Trim();
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, headerValue);
                }
            }
        }

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
    
    private static async Task<string> PostApi(string url, string? headers = null, string? payload = null)
    {
        using var client = new HttpClient();
        if (!string.IsNullOrEmpty(headers))
        {
            string[] headerLines = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string headerLine in headerLines)
            {
                string[] headerParts = headerLine.Split(':', 2);
                if (headerParts.Length == 2)
                {
                    string headerName = headerParts[0].Trim();
                    string headerValue = headerParts[1].Trim();
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, headerValue);
                }
            }
        }

        var content = new StringContent(payload ?? "", Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
    
    private static async Task<string> PutApi(string url, string? headers = null, string? payload = null)
    {
        using var client = new HttpClient();
        if (!string.IsNullOrEmpty(headers))
        {
            string[] headerLines = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string headerLine in headerLines)
            {
                string[] headerParts = headerLine.Split(':', 2);
                if (headerParts.Length == 2)
                {
                    string headerName = headerParts[0].Trim();
                    string headerValue = headerParts[1].Trim();
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, headerValue);
                }
            }
        }

        var content = new StringContent(payload ?? "", Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
    
    private static async Task<string> DeleteApi(string url, string? headers = null)
    {
        using var client = new HttpClient();
        if (!string.IsNullOrEmpty(headers))
        {
            string[] headerLines = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string headerLine in headerLines)
            {
                string[] headerParts = headerLine.Split(':', 2);
                if (headerParts.Length == 2)
                {
                    string headerName = headerParts[0].Trim();
                    string headerValue = headerParts[1].Trim();
                    client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, headerValue);
                }
            }
        }

        HttpResponseMessage response = await client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
}