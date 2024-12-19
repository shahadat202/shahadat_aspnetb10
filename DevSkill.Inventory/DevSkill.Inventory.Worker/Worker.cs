using Amazon.S3;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevSkill.Inventory.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly AmazonSQSClient _sqsClient;
        //private readonly AmazonS3Client _s3Client;
        //private readonly string _queueUrl = "https://sqs.us-east-1.amazonaws.com/847888492411/shahadat-queue";
        //private readonly string _bucketName = "s3-bucket-name";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            //_sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.USEast1);
            //_s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);

                // Poll SQS for message
                //var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest()
                //{
                //    QueueUrl = _queueUrl,
                //    MaxNumberOfMessages = 1,
                //    WaitTimeSeconds = 5
                //});

                //foreach (var message in response.Messages)
                //{
                //    try
                //    {
                //        // Deserialize message
                //        var data = JsonConvert.DeserializeObject<ImageMessage>(message.Body);

                //        // Resize image
                //        var resizedImagePath = ResizeImage(data.FilePath);

                //        // Upload to S3
                //        await UploadToS3(resizedImagePath, data.FileName);

                //        // Delete message from queue
                //        await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Error processing SQS message: {MessageId}", message.MessageId);
                //    }
                //}
            }
        }

        //private string ResizeImage(string filePath)
        //{
        //    var resizedPath = Path.Combine(Path.GetDirectoryName(filePath), $"resized_{Path.GetFileName(filePath)}");

        //    using (var image = Image.FromFile(filePath))
        //    {
        //        var resized = new Bitmap(image, new Size(200, 200));
        //        resized.Save(resizedPath, ImageFormat.Jpeg);
        //    }

        //    return resizedPath;
        //}

        //private async Task UploadToS3(string filePath, string fileName)
        //{
        //    using (var stream = new FileStream(filePath, FileMode.Open))
        //    {
        //        await _s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
        //        {
        //            BucketName = _bucketName,
        //            Key = fileName,
        //            InputStream = stream
        //        });
        //    }
        //}
    }
}
