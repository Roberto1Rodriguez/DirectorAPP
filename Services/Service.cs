using DirectorAPP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorAPP.Services
{
   public class Service
    {
        HttpClient cliente = new HttpClient
        {
            BaseAddress = new Uri("https://directorapi.sistemas19.com/")
        };
        public event Action<List<string>> Error;

        public async Task<bool> Login(Usuario u)
        {
            var json = JsonConvert.SerializeObject(u);
            var response = await cliente.PostAsync("api/usuario/login", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
                return false;
            }
            if (response.StatusCode==System.Net.HttpStatusCode.NotFound)
            {
                LanzarError("No se encontro el vuelo");
                return false;
            }
            return true;
        }

        void LanzarError(string mensaje)
        {
            Error?.Invoke(new List<string> { mensaje });
        }


        void LanzarErrorJson(string json)
        {

            List<string> obj = JsonConvert.DeserializeObject<List<string>>(json);
            if (obj != null)
            {
                Error?.Invoke(obj);
            }
        }

    }
}
