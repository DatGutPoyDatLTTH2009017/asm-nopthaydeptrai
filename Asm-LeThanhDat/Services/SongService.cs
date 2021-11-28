using Asm_LeThanhDat.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Asm_LeThanhDat.Services
{
    class SongService
    {
        private const string ApiBaseUrl = "https://music-i-like.herokuapp.com";
        private const string ApiSongPath = "/api/v1/songs";
        // gọi lên api, lấy danh sách bài hát và trả về cho view.

        public async Task<List<Song>> GetLatestSongAsync()
        {
            Debug.WriteLine("Start getting songs from api.");
            List<Song> result = new List<Song>();
            try
            {
                HttpClient httpClient = new HttpClient();
                // đây là bước đeo thẻ xe buýt vào cổ.
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {credential.access_token}");
                // thực hiện gửi dữ liệu sử dụng await, async
                Debug.WriteLine("Sending request.");
                var requestConnection =
                    await httpClient.GetAsync(ApiBaseUrl + ApiSongPath); // gặp vấn đề về độ trễ mạng, băng thông, đường truyền.                                                                                                // chờ phản hồi, lấy kết quả
                Debug.WriteLine("Got connection. " + requestConnection.StatusCode);
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Debug.WriteLine("Got data.");
                    // lấy content dạng string.
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    // parse content sang lớp Account.
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Song>>(content);
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Got error.");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.Data);
            }
            Debug.WriteLine("Return result.");
            return result;
        }

        public async Task<Song> CreateSongAsync(Song song)
        {
            throw new NotImplementedException();
        }
    }
}

