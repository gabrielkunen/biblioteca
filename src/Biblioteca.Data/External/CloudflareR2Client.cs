using System.Net;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Biblioteca.Application.Interface.External;
using Biblioteca.Application.Models.Cloudflare;
using Biblioteca.Application.Models.Result;
using Biblioteca.Domain.Enums;

namespace Biblioteca.Data.External;

public class CloudflareR2Client : ICloudflareR2Client
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    
    public CloudflareR2Client()
    {
        var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("S3_ACCESS_KEY"),
            Environment.GetEnvironmentVariable("S3_KEY"));
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("S3_URL"),
            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
            ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
        });
        _bucketName = "biblioteca";
    }
    
    public async Task<CustomResultModel<UploadArquivoDto>> UploadArquivo(string chaveArquivo, string contentType, byte[] arquivo)
    {
        using var arquivoStream = new MemoryStream(arquivo);
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = chaveArquivo,
            InputStream = arquivoStream,
            ContentType = contentType,
            DisablePayloadSigning = true
        };

        var response = await _s3Client.PutObjectAsync(request);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            return CustomResultModel<UploadArquivoDto>.Failure(new CustomErrorModel(ECodigoErro.ServiceUnavailable, "Ocorreu um erro ao incluir um arquivo no bucket"));

        var etagTratada = response.ETag.Trim('"');
        return CustomResultModel<UploadArquivoDto>.Success(new UploadArquivoDto(etagTratada, chaveArquivo, contentType));
    }
}