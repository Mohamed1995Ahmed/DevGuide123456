using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using ViewModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
      


    


       
            private readonly IConfiguration _configuration;
        private readonly PaymentManager paymentManager;

        public PaymentController(IConfiguration configuration, PaymentManager paymentManager)
            {
                _configuration = configuration;
            this.paymentManager = paymentManager;
        }
        // [HttpGet("total/{instructorId}")]
        // DON,T NEED MENTOR ID
        [HttpGet("payments/totals")]
        [Authorize]
        public async Task<IActionResult> GetPaymentTotals()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await paymentManager.GetPaymentTotalsForInstructorAsync1(userId);
            return Ok(new
            {
                TotalAllTime = result.totalAllTime,
                TotalLastMonth = result.totalLastMonth,
                TotalCurrentMonth = result.totalCurrentMonth
            });
        }
        // NEED MENTOR ID
        [HttpGet("payments/totals/{instructorId}")]
        public async Task<IActionResult> GetPaymentTotalsForInstructor(string instructorId)
        {
            var result = await paymentManager.GetPaymentTotalsForInstructorAsync(instructorId);
            return Ok(new
            {
                TotalAllTime = result.totalAllTime,
                TotalLastMonth = result.totalLastMonth,
                TotalCurrentMonth = result.totalCurrentMonth
            });
        }



        [HttpPost("create-paypal-payment")]
        public IActionResult CreatePayPalPayment([FromBody] CreatePaymentViewModel _createPayment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var clientId = _configuration["PayPalSettings:ClientID"];
                var secret = _configuration["PayPalSettings:Secret"];
                var mode = _configuration["PayPalSettings:Mode"];

                var config = new Dictionary<string, string>
                {
                    { "mode", mode }
                };

                var apiContext = new APIContext(new OAuthTokenCredential(clientId, secret, config).GetAccessToken())
                {
                    Config = config
                };

                var payment = new PayPal.Api.Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount
                            {
                                total = _createPayment.Amount.ToString("F2"),
                                currency = "USD"
                            },
                            description = "Auction deposit"
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = "http://localhost:4200/paypal",
                        cancel_url = "http://localhost:4200/confirm"
                    }

                };

                var createdPayment = payment.Create(apiContext).GetApprovalUrl();
                return Ok(new { result = createdPayment, status = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Adding payment was not completed", error = ex.Message });
            }
        }
    }
}

//        [HttpPost("create-paypal-payment")]
//        public  IActionResult CreatePayPalPayment([FromBody] CreatePaymentViewModel _createPayment)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var clientId = _configuration["PayPalSettings:ClientID"];
//                var secret = _configuration["PayPalSettings:Secret"];
//                var mode = _configuration["PayPalSettings:Mode"];

//                var config = new Dictionary<string, string>
//            {
//                { "mode", mode }
//            };

//                var apiContext = new APIContext(new OAuthTokenCredential(clientId, secret, config).GetAccessToken())
//                {
//                    Config = config
//                };

//                var payment = new PayPal.Api.Payment
//                {
//                    intent = "sale",
//                    payer = new Payer { payment_method = "paypal" },
//                    transactions = new List<Transaction>
//                {
//                    new Transaction
//                    {
//                        amount = new Amount
//                        {
//                            total = _createPayment.Amount.ToString("F2"),
//                            currency = "USD"
//                        },
//                        description = "Auction deposit"
//                    }
//                },
//                    redirect_urls = new RedirectUrls
//                    {
//                        return_url ="",
//                        cancel_url = ""
//                    }
//                };

//                var createdPayment = payment.Create(apiContext).GetApprovalUrl();
//                return Ok(new { result = createdPayment, status = 200 });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "Adding payment was not completed", error = ex.Message });
//            }
//        }
//    }
//}









//        [HttpPost("create-paypal-payment")]
//        public IActionResult CreatePayPalPayment([FromBody] CreatePaymentViewModel _createPayment)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var apiContext = new APIContext(new OAuthTokenCredential(configuration["PayPalSetting:ClientID"],
//                    configuration["PayPalSetting:Secret"], Config).GetAccessToken());

//                var payment = new PayPal.Api.Payment
//                {
//                    intent = "sale",
//                    payer = new Payer { payment_method = "paypal" },
//                    transactions = new List<Transaction>
//    {
//        new Transaction
//        {
//            amount = new Amount
//            {
//                total = _createPayment.Amount.ToString("F2"),
//                currency = "USD",
//            },
//            description = "Session deposit"
//        }
//    },
//                    redirect_urls = new RedirectUrls
//                    {
//                        return_url = $"http://localhost:4200/user/won-auction?auctionId={_createPayment.SessionID}",
//                        cancel_url = "http://localhost:4200/user/won-auction"
//                    }
//                };

//                var createdPayment = payment.Create(apiContext).GetApprovalUrl();
//                return Ok(new { result = createdPayment, status = 200 });

//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = "adding payment is not completed" });
//            }

//        }
//    }
//}