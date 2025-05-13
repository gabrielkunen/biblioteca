using Biblioteca.Application.Models.Cloudflare;
using Biblioteca.Application.Models.Result;

namespace Biblioteca.Application.Interface.External;

public interface ICloudflareR2Client
{
    Task<CustomResultModel<UploadArquivoDto>> UploadArquivo(string chaveArquivo, string contentType, byte[] arquivo);
}