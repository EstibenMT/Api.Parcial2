using Api.Parcial2.Classes;
using Api.Parcial2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Parcial2.Controllers
{
    [RoutePrefix("api/Pesaje")]
    public class PesajeController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> ConsultarXPlaca(string placa)
        {
            clsPesaje clsP = new clsPesaje();
            return await clsP.ConsultarXPlaca(placa);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CrearPesaje([FromBody]Pesaje pesaje)
        {
            clsPesaje clsP = new clsPesaje();
            clsP.pesaje = pesaje;
            return await clsP.Insertar();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> ConsultarXIdPesaje(int id)
        { 
            clsPesaje clsP = new clsPesaje();
            return await clsP.ListarImagenes(id);
        }
    }
}