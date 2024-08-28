using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using TallentexResult.Interface;

namespace TallentexResult.Servies
{
    public class AmazonUploader : IAmazonUploader
    {
        private readonly IAmazonS3 _amazonS3;   // injecting the IAmazonS3
        private readonly IConfiguration _config;    // injecting the Configuration to get AWS details from appsetting.json

        public AmazonUploader(IAmazonS3 amazonS3, IConfiguration config)    
        {
            _amazonS3 = amazonS3;
            _config = config;
        }

        // Photo Path
        public string srcphoto { get { return _config.GetValue<string>("AWS:ProfileName") + "/" + LNResources.Session + "/" + "applicantsphoto/{0}.jpg"; } }

        // Verification Document Path
        public string srcdoc { get { return _config.GetValue<string>("AWS:ProfileName") + "/" + LNResources.Session + "/result/" + "{0}.jpg"; } }

        /// <summary>
        /// Determines whether a file exists within the specified bucket
        /// </summary>
        /// <param name="fileKey">Match files that begin with this prefix</param>
        /// <returns>True if the file exists</returns>
        public async Task<bool> S3Exists(string fileKey)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _config.GetValue<string>("AWS:BucketName"),    // The name of the bucket to search
                    Key = fileKey
                };

                await _amazonS3.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                //status wasn't not found, so throw the exception
                throw;
            }
        }

        /// <summary>
        /// Generate a presigned URL that can be used to access the file named
        /// in the objectKey parameter for the amount of time specified in the
        /// duration parameter.
        /// </summary>
        /// <param name="fileKey">The name of the object to access with the presigned URL.</param>
        /// <returns>A string representing the generated presigned URL.</returns>
        public async Task<string> GeneratePreSignedURL(string objectKey)
        {
            try
            {
                string urlString = "";
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _config.GetValue<string>("AWS:BucketName"),
                    Key = objectKey,
                    Expires = DateTime.Now.AddMinutes(5)     // Specify how long the presigned URL lasts, in minute
                };

                urlString = await _amazonS3.GetPreSignedURLAsync(request);
                return urlString;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Uploads a single file to an S3 bucket.
        /// </summary>
        /// <param name="file">the file to upload.</param>
        /// <param name="fileKey">The name of the file to upload.</param>
        /// <returns>A boolean value indicating the success of the action.</returns>
        public async Task<bool> UploadFileToS3(IFormFile file, string fileKey)
        {
            try
            {
                fileKey = fileKey.Replace("apps", "staging");
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest    // The transfer initialized TransferUtility object.
                    {
                        InputStream = newMemoryStream,
                        Key = fileKey,
                        BucketName = _config.GetValue<string>("AWS:BucketName"),    // The name of the S3 bucket where the file will be stored.
                        //CannedACL = S3CannedACL.PublicRead
                    };

                    // initialise the transfer/upload tools
                    var fileTransferUtility = new TransferUtility(_amazonS3);
                    // initiate the file upload
                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
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
