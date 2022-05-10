using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Text;
using Microsoft.Teams.Apps.CompanyCommunicator.Common.Repositories.NotificationData;
using System.Linq;

namespace Microsoft.Teams.Apps.CompanyCommunicator.ReportImage.Func
{
    public class ReportCardViewFunction
    {
        private readonly IMessageReadReportingRepository readReportingRepo;

        public ReportCardViewFunction(
            IMessageReadReportingRepository readReportingRepo
            )
        {
            this.readReportingRepo = readReportingRepo ?? throw new ArgumentNullException(nameof(readReportingRepo));
        }

        [FunctionName("ReportCardViewFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {

            string notificationId = req.Query["notification"];
            string uniqueId = req.Query["random"];

            var userLangs = req.Headers["Accept-Language"].ToString();
            var firstLang = userLangs.Split(',').FirstOrDefault();
            var defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;

            log.LogInformation("Image hit:" + String.Join(",", req.Query) + " - " + (String.IsNullOrEmpty(notificationId) ? "No Notification ID" : "Notification ID" + notificationId));

            var path = System.IO.Path.Combine(context.FunctionAppDirectory, "reportImage.jpg");
            var image = await File.ReadAllBytesAsync(path);

            var notificationRecord = new MessageReadReportingDataEntity
            {
                PartitionKey = notificationId,
                RowKey = uniqueId,
                Id = uniqueId,
                DefaultLanguage = defaultLang
            };
            await this.readReportingRepo.InsertOrMergeAsync(notificationRecord);

            return new FileContentResult(image, "image/jpeg");
        }

        private static MemoryStream StringToMemoryStream(Encoding encoding, string source)
        {
            var content = encoding.GetBytes(source);
            return new MemoryStream(content);
        }
    }
}
