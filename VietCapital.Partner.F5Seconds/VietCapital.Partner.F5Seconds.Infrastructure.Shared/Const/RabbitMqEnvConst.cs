using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const
{
    public static class RabbitMqEnvConst
    {
        public static string Host { get; set; } = "RABBITMQ_HOST";
        public static string Vhost { get; set; } = "RABBITMQ_USER";
        public static string User { get; set; } = "RABBITMQ_USER";
        public static string Pass { get; set; } = "RABBITMQ_PASS";
        public static string productSyncQueue { get; set; } = "RABBITMQ_PRODUCT_SYNC";
    }

    public static class RabbitMqAppSettingConst
    {
        public static string Host { get; set; } = "RabbitMqSettings:Host";
        public static string Vhost { get; set; } = "RabbitMqSettings:vHost";
        public static string User { get; set; } = "RabbitMqSettings:Username";
        public static string Pass { get; set; } = "RabbitMqSettings:Password";
        public static string productSyncQueue { get; set; } = "RabbitMqSettings:productSyncQueue";
    }
}
