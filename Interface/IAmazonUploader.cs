namespace TallentexResult.Interface
{
    public interface IAmazonUploader
    {
        public string srcphoto { get; }
        public string srcdoc { get; }


        public Task<bool> S3Exists(string fileKey);

        public Task<string> GeneratePreSignedURL(string fileKey);

        public Task<bool> UploadFileToS3(IFormFile file, string fileKey);
    }
}
