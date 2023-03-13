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
                var errores = await response.Content.ReadAsStringAsync();
                LanzarError(errores);
                return false;
            }
            return true;
        }
        public async Task<List<Docentes>> GetDocentes()
        {
            List<Docentes> docentes = null;
            var response = await cliente.GetAsync("api/docente");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                docentes=JsonConvert.DeserializeObject<List<Docentes>>(json);
            }
            if (docentes!=null)
            {
                return docentes;
            }
            else
            {
                return new List<Docentes>();
            }
        }
        public async Task<List<Usuario>> GetUsuarios()
        {
            List<Usuario> docentes = null;
            var response = await cliente.GetAsync("api/usuario");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                docentes = JsonConvert.DeserializeObject<List<Usuario>>(json);
            }
            if (docentes != null)
            {
                return docentes;
            }
            else
            {
                return new List<Usuario>();
            }
        }
        public async Task<bool> InsertUsuario(Usuario u)
        {
            var json = JsonConvert.SerializeObject(u);
            var response = await cliente.PostAsync("api/usuario", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.StatusCode==System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadAsStringAsync();
                LanzarErrorJson(errores);
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
