using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asm_LeThanhDat.Services
{
    public class FileServices
    {
        private static string CloudName = "datdepzai";
        private static string CloudApiKey = "0123456789";
        private static string CloudApiSecret = "gOOT_72AMyn9TQz1Hd4MxyGRjxY";
        // tạo đối tượng upload cloudinary.            
        static CloudinaryDotNet.Account account;
        static CloudinaryDotNet.Cloudinary cloud;

        public FileServices()
        {
            // kiểm tra để khởi tạo account 1 lần duy nhất.
            if (account == null)
            {
                account = new CloudinaryDotNet.Account
                {
                    Cloud = CloudName,
                    ApiKey = CloudApiKey,
                    ApiSecret = CloudApiSecret
                };
            }
            // tạo đối tượng upload cloudinary.   
            if (cloud == null)
            {
                cloud = new CloudinaryDotNet.Cloudinary(account);
                cloud.Api.Secure = true;
            }
        }

        public async Task<string> UploadFile(Windows.Storage.StorageFile file)
        {
            if (file != null)
            {
                Debug.WriteLine(file.Path);
                CloudinaryDotNet.Actions.RawUploadParams imageUploadParams
                   = new CloudinaryDotNet.Actions.RawUploadParams
                   {
                       File = new CloudinaryDotNet.FileDescription(file.Name, await file.OpenStreamForReadAsync())
                   };
                // thực hiện upload, lấy thông tin về.
                CloudinaryDotNet.Actions.RawUploadResult result = await cloud.UploadAsync(imageUploadParams);
                return result.SecureUrl.OriginalString;
            }
            return null;
        }
    }
}
