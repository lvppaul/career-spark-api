using CareerSpark.BusinessLayer.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace CareerSpark.BusinessLayer.Interfaces
{
    public interface ICloudinaryService
    {
        Task<FileUploadResponse> UploadFileAsync(IFormFile file, string folder = "tests");
        Task<List<FileUploadResponse>> UploadFilesAsync(List<IFormFile> files, string folder = "tests");
        Task<bool> DeleteFileAsync(string publicId);
        Task<List<bool>> DeleteFilesAsync(List<string> publicIds);
        bool IsValidFileType(IFormFile file, List<string> allowedExtensions);
        bool IsValidFileSize(IFormFile file, long maxSizeInBytes);
        (bool IsValid, string ErrorMessage) ValidateDocumentFile(IFormFile file, int maxSizeInMB = 10);
    }
}
