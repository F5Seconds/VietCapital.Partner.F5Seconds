using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Commands;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
using VietCapital.Partner.F5Seconds.Domain.Entities;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;

namespace VietCapital.Partner.F5Seconds.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/transaction")]
    public class TransactionController : BaseApiController
    {
        private readonly IBus _bus;
        string rabbitHost = "";
        string rabbitvHost = "";
        string voucherTransactionQueue = "";
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;
        public TransactionController(IBus bus, IWebHostEnvironment env, IConfiguration config, IMapper mapper, ILogger<TransactionController> logger)
        {
            _env = env;
            _bus = bus;
            _config = config;
            _mapper = mapper;
            _logger = logger;
            if (_env.IsDevelopment())
            {
                rabbitHost = _config[RabbitMqAppSettingConst.Host];
                rabbitvHost = _config[RabbitMqAppSettingConst.Vhost];
                voucherTransactionQueue = _config[RabbitMqAppSettingConst.voucherTransactionQueue];
            }
            if (_env.IsProduction())
            {
                rabbitHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Host);
                rabbitvHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Vhost);
                voucherTransactionQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.voucherTransactionQueue);
            }

        }

        [HttpPost("buy")]
        public async Task<IActionResult> PostBuyVoucher(CreateTransactionCommand command)
        {
            var result = await Mediator.Send(command);
            Uri uri = new Uri($"rabbitmq://{rabbitHost}/{rabbitvHost}/{voucherTransactionQueue}");
            var endPoint = await _bus.GetSendEndpoint(uri);
            if (result.Succeeded)
            {
                foreach (var item in result.Data)
                {
                    var trans = _mapper.Map<VoucherTransaction>(item, opt => opt.AfterMap((s, d) => {
                        d.ProductId = command.productId;
                        d.CustomerId = command.customerId;
                        d.CustomerPhone = command.customerPhone;
                    }));
                    _logger.LogInformation(JsonConvert.SerializeObject(trans));
                    await endPoint.Send(trans);
                }
            }
            return Ok(result);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetTransaction(string cif,int state)
        {
            return Ok(await Mediator.Send(new GetVoucherTransFilterQuery() { Cif = cif,State = state}));
        }
    }
}
