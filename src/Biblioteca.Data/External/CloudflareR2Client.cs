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
    public async Task<CustomResultModel<UploadArquivoDto>> UploadArquivo(string chaveArquivo, string contentType, byte[] arquivo)
    {
        var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("S3_ACCESS_KEY"),
            Environment.GetEnvironmentVariable("S3_KEY"));
        var s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("S3_URL"),
            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
            ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
        });
        const string bucketName = "biblioteca";
        
        using var arquivoStream = new MemoryStream(arquivo);
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = chaveArquivo,
            InputStream = arquivoStream,
            ContentType = contentType,
            DisablePayloadSigning = true
        };

        var response = await s3Client.PutObjectAsync(request);
        if (response.HttpStatusCode != HttpStatusCode.OK)
            return CustomResultModel<UploadArquivoDto>.Failure(new CustomErrorModel(ECodigoErro.ServiceUnavailable, "Ocorreu um erro ao incluir um arquivo no bucket"));

        var etagTratada = response.ETag.Trim('"');
        return CustomResultModel<UploadArquivoDto>.Success(new UploadArquivoDto(etagTratada, chaveArquivo, contentType));
    }
}