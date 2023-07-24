using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Constant;
using server.Dto.Base;
using server.Dto.Payment;
using server.Entity;
using server.Models;
using server.Utils;
using Stripe.Checkout;


namespace server.Controllers
{
    [ApiController]
    [Route("/api/v1/payment")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "USER")]
    public class BillingController : Controller
    {
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public BillingController(IPaymentInfoRepository paymentInfoRepository)
        {
            _paymentInfoRepository = paymentInfoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);
            PaymentInfo? paymentInfo = _paymentInfoRepository.FindOneByUserId(payload.UserId);
            if (paymentInfo != null && PaymentStatusConstant.ACTIVE.Equals(paymentInfo?.Status)
                && paymentInfo.EndDate > DateTime.Now)
            {
                return Ok(new PaymentResponse
                {
                    PaymentInfo = "Expire time " + paymentInfo?.EndDate.ToString("dd/MM/yyyy"),
                    NumberOfSchedulers = "Unlimited Schedulers",
                });
            }
            return BadRequest(new PaymentResponse()
            {
                IsSuccess = false,
                Message = "Không tìm thấy thông tin thanh toán"
            });
        }

        [HttpGet("success/{id}")]
        public async Task<IActionResult> PaymentSucess(int? id)
        {
            if(id == null)
            {
                throw new ApplicationException("Thông tin thanh toán không đúng");
            }
            PaymentInfo? paymentInfo = _paymentInfoRepository.FindById((int)id);
            if (paymentInfo == null)
            {
                throw new ApplicationException("Không tìm thấy thông tin thanh toán");
            }
            if (paymentInfo.Status == PaymentStatusConstant.PENDING)
            {
                paymentInfo.Status = PaymentStatusConstant.ACTIVE;
                _paymentInfoRepository.Update(paymentInfo);
                return Ok(new PaymentResponse()
                {
                    Status = getStatus(PaymentStatusConstant.ACTIVE),
                    Message = "Thanh toán thành công"
                });
            }
            throw new ApplicationException("Không tìm thấy thông tin thanh toán");
        }

        [HttpGet("cancel/{id}")]
        public async Task<IActionResult> PaymentCancel(int? id)
        {
            if (id == null)
            {
                throw new ApplicationException("Thông tin thanh toán không đúng");
            }
            PaymentInfo? paymentInfo = _paymentInfoRepository.FindById((int)id);
            if (paymentInfo == null)
            {
                throw new ApplicationException("Không tìm thấy thông tin thanh toán");
            }
            if(paymentInfo.Status == PaymentStatusConstant.PENDING)
            {
                paymentInfo.Status = PaymentStatusConstant.CANCEL;
                _paymentInfoRepository.Update(paymentInfo);
                return Ok(new PaymentResponse()
                {
                    Status = getStatus(PaymentStatusConstant.CANCEL),
                    Message = "Thanh toán thất bại"
                });
            }
            throw new ApplicationException("Không tìm thấy thông tin thanh toán");
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRequest request)
        {
            if (request == null || request.Month == null)
            {
                throw new ApplicationException("Thông tin thanh toán không đúng");
            }

            int price = (int)(request.Month * 39000);

            AccessTokenPayload? payload = CommonUtil.GetPayload(HttpContext.Request);

            PaymentInfo? paymentInfo = _paymentInfoRepository.FindOneByUserId(payload.UserId);

            if (paymentInfo == null)
            {
                paymentInfo = new PaymentInfo()
                {
                    UserId = (int)payload.UserId,
                    PaymentAmount = price,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths((int)request.Month),
                    Status = PaymentStatusConstant.PENDING,
                };
                _paymentInfoRepository.Add(paymentInfo);
            }
            else
            {
                if (paymentInfo.EndDate > DateTime.Now && PaymentStatusConstant.ACTIVE.Equals(paymentInfo?.Status))
                {
                    return Ok(new PaymentResponse()
                    {
                        IsSuccess = false,
                        Status = getStatus(PaymentStatusConstant.ACTIVE),
                        Message = "Thanh toán thành công"
                    });
                }
                paymentInfo.UserId = (int)payload.UserId;
                paymentInfo.PaymentAmount = price;
                paymentInfo.StartDate = DateTime.Now;
                paymentInfo.EndDate = DateTime.Now.AddMonths((int)request.Month);
                paymentInfo.Status = PaymentStatusConstant.PENDING;
                _paymentInfoRepository.Update(paymentInfo);
            }

            var conf = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
            var domain = conf["Payment:Domain"];

            var options = new SessionCreateOptions()
            {
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = price,
                        Currency = "vnd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Payment for " + request.Month + " month for " + payload!.Email,
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = domain + "/api/billing/success/" + paymentInfo.PaymentId,
                CancelUrl = domain + "/api/billing/cancel/" + paymentInfo.PaymentId,
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new PaymentResponse
            {
                IsSuccess = true,
                Url = session.Url,
            });
        }

        private string getStatus(int aCTIVE)
        {
            switch (aCTIVE)
            {
                case 1:
                    return "ACTIVE";
                case 2:
                    return "PENDING";
                case 3:
                    return "EXPIRE";
                case 4:
                    return "CANCEL";
                default:
                    return "";
            }
        }
    }
}
