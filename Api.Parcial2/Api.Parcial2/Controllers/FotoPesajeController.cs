using Api.Parcial2.Classes;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Parcial2.Controllers
{
    [RoutePrefix("api/FotoPesaje")]
    public class FotoPesajeController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> CargarArchivo(HttpRequestMessage request, string Datos, string Proceso)
        {
            clsFotoPesaje upload = new clsFotoPesaje();
            upload.Datos = Datos;
            upload.Proceso = Proceso;
            upload.request = request;
            return await upload.GrabarArchivo(false);
        }
        [HttpGet]
        public HttpResponseMessage ConsultarArchivo(string NombreImagen)
        {
            clsFotoPesaje upload = new clsFotoPesaje();
            return upload.DescargarArchivo(NombreImagen);
        }
        [HttpPut]
        public async Task<HttpResponseMessage> ActualizarArchivo(HttpRequestMessage request)
        {
            clsFotoPesaje upload = new clsFotoPesaje();
            upload.request = request;
            return await upload.GrabarArchivo(true);
        }

        [HttpDelete]
        public HttpResponseMessage EliminarArchivo(string NombreImagen)
        {
            clsFotoPesaje upload = new clsFotoPesaje();
            return upload.EliminarArchivo(NombreImagen);
        }
    }
}