using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Consumer
{
    public static class CRUD<T>
    {
        public static string Endpoint { get; set; }
        public static List<T> GetAll()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(Endpoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} ");
                }
            }
        }
        public static T GetById(int id)
        {
            using (var cliente = new HttpClient())
            {
                var response = cliente.GetAsync($"{Endpoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} ");
                }
            }
        }
        public static T Create(T item)
        {
            using (var cliente = new HttpClient())
            {
                var response = cliente.PostAsync(Endpoint,
                    new StringContent(JsonConvert.SerializeObject(item),
                    Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} ");
                }
            }
        }
        public static bool Update(int id, T item)
        {
            using (var cliente = new HttpClient())
            {
                var response = cliente.PutAsync(
                    $"{Endpoint}/{id}", new StringContent(JsonConvert.SerializeObject(item),
                    Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} ");

                }
            }
        }
        public static bool Delete(int id)
        {
            using (var cliente = new HttpClient())
            {
                var response = cliente.DeleteAsync($"{Endpoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode} ");
                }
            }
        }
    }
}
