using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TallentexResult.Controllers
{
    public class BucketsController : Controller
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly IConfiguration _config;

        public BucketsController(IAmazonS3 amazonS3, IConfiguration config)
        {
            _amazonS3 = amazonS3;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var a = await S3Exists(_amazonS3, "staging/2025/applicantsphoto/online/600001.jpg");

            //ViewBag.Buckets = a.Buckets;
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateBucketAsync(string bucketName)
        //{
        //    var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName);
        //    if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
        //    await _amazonS3.PutBucketAsync(bucketName);
        //    return Created("buckets", $"Bucket {bucketName} created.");
        //}

        [HttpGet]
        public async Task<ListBucketsResponse> GetAllBucketAsync()
        {
            var data = await _amazonS3.ListBucketsAsync();
            //var buckets = data.Buckets.Select(b => { return b.BucketName; });
            return data;
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        //{
        //    await _amazonS3.DeleteBucketAsync(bucketName);
        //    return NoContent();
        //}

        [HttpGet]
        public async Task<IActionResult> Exists()
        {
            try
            {
                var accessKey = _config.GetValue<string>("AWS:AccessKey");
                var seceretKey = _config.GetValue<string>("AWS:SecretKey");
                RegionEndpoint regionEndpoint = RegionEndpoint.APSouth1;
                string bucketName = "tallentex-india";

                var s3Client = new AmazonS3Client(accessKey, seceretKey, new AmazonS3Config { RegionEndpoint = RegionEndpoint.APSouth1 });

                //string urlString = "";
                //try
                //{
                //    GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                //    {
                //        BucketName = bucketName,
                //        Key = "staging/2025/applicantsphoto/online/500001.jpg",
                //        Expires = DateTime.Now.AddMinutes(5)
                //    };
                //    urlString = s3Client.GetPreSignedURL(request1);

                //    return Ok(urlString);
                //}
                //catch (AmazonS3Exception e)
                //{
                //    Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                //}

                //var bucketExists = Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3Client, _config["AWS:BucketName"].ToString());
                //if (bucketExists) return BadRequest($"Bucket {_config["AWS:BucketName"].ToString()} already exists.");


                //var data = await s3Client.ListBucketsAsync();
                //var buckets = data.Buckets.Select(b => { return b.BucketName; });

                var s3Object = await s3Client.GetObjectAsync(bucketName, "staging/2025/applicantsphoto/online/500001.jpg");
                Console.WriteLine(s3Object);
                return Ok(s3Object);
            }
            catch (Exception ex) { return Ok(ex.Message); }
            
        }

        public async Task<bool> S3Exists(IAmazonS3 amazonS3, string fileKey)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _config.GetValue<string>("AWS:BucketName"),
                    Key = fileKey
                };

                await amazonS3.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                throw;
            }
        }
    }
}
