using Asm_LeThanhDat.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Asm_LeThanhDat.Services
{
    class AccountService
    {
        // Thực hiện đăng ký người dùng với api, nhận tham số là một đối tượng của lớp Account
        // Chuyển đối tượng về định dạng json sau đó gửi dữ liệu lên api với method POST.
        public async Task<bool> RegisterAsync(Account account)
        {
            try
            {
                var accountJson = Newtonsoft.Json.JsonConvert.SerializeObject(account);
                HttpClient httpClient = new HttpClient();
                // Tạo dữ liệu thô để gửi đi.           
                Debug.WriteLine(accountJson);
                // đóng gói dữ liệu, dán nhãn UTF8, dán format json.
                var httpContent = new StringContent(accountJson, Encoding.UTF8, "application/json");
                // thực hiện gửi dữ liệu sử dụng await, async
                var requestConnection =
                    await httpClient.PostAsync("https://music-i-like.herokuapp.com/api/v1/accounts", httpContent); // gặp vấn đề về độ trễ mạng, băng thông, đường truyền.
                                                                                                                   // chờ phản hồi, lấy kết quả
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var content = await requestConnection.Content.ReadAsStringAsync();
                    //Console.WriteLine("Finish program");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public async Task<Credential> LoginAsync(LoginInformation loginInformation)
        {
            try
            {
                var accountJson = Newtonsoft.Json.JsonConvert.SerializeObject(loginInformation); // stringtify
                HttpClient httpClient = new HttpClient();
                // Tạo dữ liệu thô để gửi đi.           
                Console.WriteLine(accountJson);
                // đóng gói dữ liệu, dán nhãn UTF8, dán format json.
                var httpContent = new StringContent(accountJson, Encoding.UTF8, "application/json");
                // thực hiện gửi dữ liệu sử dụng await, async
                var requestConnection =
                    await httpClient.PostAsync("https://music-i-like.herokuapp.com/api/v1/accounts/authentication", httpContent); // gặp vấn đề về độ trễ mạng, băng thông, đường truyền.
                Console.WriteLine(requestConnection.StatusCode);                                                                                     // chờ phản hồi, lấy kết quả
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // lấy content dạng string.
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    // parse content sang lớp Credential.
                    var credential = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential>(content);
                    //Console.WriteLine("Finish program");
                    await WriteTokenToFile(content);
                    return credential;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<Account> GetInformationAsync()
        {
            // load token từ file.
            var credential = await LoadAccessTokenFromFile();
            // nếu ko có token, trả về null
            if (credential == null)
            {
                return null;
            }
            try
            {
                HttpClient httpClient = new HttpClient();
                // đây là bước đeo thẻ xe buýt vào cổ.
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {credential.access_token}");
                // thực hiện gửi dữ liệu sử dụng await, async
                var requestConnection =
                    await httpClient.GetAsync("https://music-i-like.herokuapp.com/api/v1/accounts"); // gặp vấn đề về độ trễ mạng, băng thông, đường truyền.
                Console.WriteLine(requestConnection.StatusCode);                                                                                     // chờ phản hồi, lấy kết quả
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // lấy content dạng string.
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    Debug.WriteLine("Getting information");
                    Debug.WriteLine(content);
                    // parse content sang lớp Account.
                    var account = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(content);
                    //Console.WriteLine("Finish program");
                    Console.WriteLine(content);
                    return account;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<Account> GetInformationByAccessTokenAsync(string accessToken)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                // đây là bước đeo thẻ xe buýt vào cổ.
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                // thực hiện gửi dữ liệu sử dụng await, async
                var requestConnection =
                    await httpClient.GetAsync("https://music-i-like.herokuapp.com/api/v1/accounts"); // gặp vấn đề về độ trễ mạng, băng thông, đường truyền.
                Console.WriteLine(requestConnection.StatusCode);                                                                                     // chờ phản hồi, lấy kết quả
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // lấy content dạng string.
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    // parse content sang lớp Account.
                    var account = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(content);
                    //Console.WriteLine("Finish program");
                    Console.WriteLine(content);
                    return account;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<Credential> LoadAccessTokenFromFile()
        {
            try
            {
                // read token file.
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.GetFileAsync("milt.txt");
                var fileContent = await FileIO.ReadTextAsync(storageFile);
                var credential = Newtonsoft.Json.JsonConvert.DeserializeObject<Credential>(fileContent);
                return credential;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task WriteTokenToFile(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //// lấy ra file cần làm việc từ trong thư mục đó.
            StorageFile storageFile = await storageFolder.CreateFileAsync("milt.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, content);
        }

        public void Update()
        {

        }


    }
}
