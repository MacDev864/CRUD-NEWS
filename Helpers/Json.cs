using CRUD_NEWS.Models;
using System;
using System.Collections.Generic;

namespace CRUD_NEWS.Helpers
{
    public class Json
    {
        public static object Create(IEnumerable<NewsModel> data, string message, string messageEx, bool success, object errors)
        {
            // Convert IEnumerable<NewsModel> to List<NewsModel>
            var dataList = data != null ? new List<NewsModel>(data) : new List<NewsModel>();

            // Create an anonymous object representing JSON data
            var jsonData = new
            {
                Data = dataList,   // Use the provided data collection or an empty list if not provided
                Message = message,
                MessageEx = messageEx,
                Success = success,
                Errors = errors
            };

            // Return the JSON data directly
            return jsonData;
        }

        public static object Success(IEnumerable<NewsModel> data, string message = "", string messageEx = "")
        {
            // Call the Create method with success set to true and return the JSON data directly
            return Create(data, message, messageEx, true, null);
        }

        public static object Errors(object errors = null, string message = "", string messageEx = "")
        {
            // Call the Create method with success set to false and return the JSON data directly
            return Create(null, message, messageEx, false, errors);
        }
    }
}
