using Core.Models.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Services.GoogleMaps
{
    public interface IMapService
    {
        /// <summary>
        /// Lấy vị trí một điểm theo tên
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<(double Latitude, double Longitude)> GetCoordinatesFromAddressAsync(string address);

        /// <summary>
        /// Tính khoảng cách giữa 2 điểm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<double> CalculateDistanceBetweenPointsAsync(double startLatitude, double startLongitude, double endLatitude, double endLongitude);

        /// <summary>
        /// Lấy khoảng cách và thời gian dự kiến tùy theo phương tiện
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task GetDirections(double originLat, double originLng, double destLat, double destLng, string mode);

        /// <summary>
        /// Kiểm tra phương tiện đã đến đích hay chưa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool AreCoordinatesEqual(double lat1, double lng1, double lat2, double lng2);
    }

    public class MapService : IMapService
    {
        private readonly HttpClient _httpClient;

        public MapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool AreCoordinatesEqual(double lat1, double lng1, double lat2, double lng2)
        {
            const double tolerance = 0.0000449; // Độ sai số cho phép (có thể điều chỉnh) 5m

            // So sánh tọa độ theo độ sai số cho phép
            bool areEqual = Math.Abs(lat1 - lat2) < tolerance && Math.Abs(lng1 - lng2) < tolerance;

            return areEqual;
        }

        public async Task<double> CalculateDistanceBetweenPointsAsync(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
        {
            string apiKey = "AIzaSyB0ao0JqmgBqxsdzitlzCcITvuNsseHoQ4";
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={startLatitude},{startLongitude}&destinations={endLatitude},{endLongitude}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(content);

                if (result.rows != null && result.rows.Count > 0 && result.rows[0].elements != null && result.rows[0].elements.Count > 0)
                {
                    double distance = (double)result.rows[0].elements[0].distance.value;

                    // Chuyển đổi khoảng cách từ mét sang kilômét
                    return distance / 1000;
                }
                else
                {
                    throw new Exception("The response does not contain valid distance information.");
                }
            }
            else
            {
                // Xử lý trường hợp lỗi
                throw new HttpRequestException($"Failed to calculate distance. Status code: {response.StatusCode}");
            }
        }

        public async Task<(double Latitude, double Longitude)> GetCoordinatesFromAddressAsync(string address)
        {
            string apiKey = "AIzaSyB0ao0JqmgBqxsdzitlzCcITvuNsseHoQ4";
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                double latitude = result.results[0].geometry.location.lat;
                double longitude = result.results[0].geometry.location.lng;

                return (latitude, longitude);
            }
            else
            {
                throw new Exception("Failed to get coordinates from address.");
            }
        }

        public async Task GetDirections(double originLat, double originLng, double destLat, double destLng, string mode)
        {
            string apiKey = "AIzaSyB0ao0JqmgBqxsdzitlzCcITvuNsseHoQ4";
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={originLat},{originLng}&destination={destLat},{destLng}&mode={mode}&key={apiKey}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                    // Lấy thông tin về tuyến đường và thời gian dự kiến
                    string distance = result.routes[0].legs[0].distance.text;
                    string duration = result.routes[0].legs[0].duration.text;

                    Console.WriteLine($"Khoảng cách: {distance}");
                    Console.WriteLine($"Thời gian dự kiến: {duration}");
                }
                else
                {
                    Console.WriteLine("Failed to get directions.");
                }
            }
        }
    }
}
