using Antlr.Runtime.Misc;
using Api.Parcial2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Api.Parcial2.Classes
{
    public class clsPesaje
    {
        private BDExamen2Entities db = new BDExamen2Entities();
        public Pesaje pesaje { get; set; }
        public async Task<HttpResponseMessage> Insertar()
        {
            try
            {
                clsCamion clsC = new clsCamion();
                Camion camion = await db.Camions.FirstOrDefaultAsync(c => c.Placa.Equals(pesaje.Camion.Placa));
                if (camion == null)
                {
                    db.Camions.Add(pesaje.Camion);
                }
                else
                {
                    pesaje.Camion = camion;
                }
                var maxId = 0;
                maxId = db.Pesajes.DefaultIfEmpty().Max(p => p == null ? 1 : p.id + 1);
                pesaje.id = maxId;
                pesaje.PlacaCamion = pesaje.Camion.Placa;

                db.Pesajes.Add(pesaje);
                await db.SaveChangesAsync();
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"Pesaje insertado correctamente")
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error al insertar el Pesaje: " + ex.Message)
                };
            }
        }

        public async Task<HttpResponseMessage> ConsultarXPlaca(string placa)
        {
            try
            {
                var query = await (from c in db.Camions
                                   where c.Placa == placa
                                   select new
                                   {
                                       Placa = c.Placa,
                                       NumeroEjes = c.NumeroEjes,
                                       Marca = c.Marca,

                                       Pesajes = (from p in db.Pesajes
                                                  where p.PlacaCamion == c.Placa
                                                  select new
                                                  {
                                                      Id = p.id,
                                                      Fecha = p.FechaPesaje,
                                                      Peso = p.Peso,

                                                      Fotos = (from f in db.FotoPesajes
                                                               where f.idPesaje == p.id
                                                               select new
                                                               {
                                                                   IdFotoPesaje = f.idFotoPesaje,
                                                                   ImagenVehiculo = f.ImagenVehiculo
                                                               }).ToList()
                                                  }).ToList()
                                   }).FirstOrDefaultAsync();

                if (query == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error: " + ex.Message)
                };
            }
        }

        public string GrabarImagenPesaje(int id, List<string> Imagenes)
        {
            try
            {
                foreach (string imagen in Imagenes)
                {
                    FotoPesaje pImage = new FotoPesaje();
                    pImage.idPesaje = id;
                    pImage.ImagenVehiculo = imagen;
                    db.FotoPesajes.Add(pImage);
                    db.SaveChanges();
                }
                return "Se grabó la información en la base de datos";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<HttpResponseMessage> ListarImagenes(int id)
        {
            try
            {
                var query = await (from P in db.Set<Pesaje>()
                                   join I in db.Set<FotoPesaje>() on P.id equals I.idPesaje
                                   where P.id == id
                                   orderby I.ImagenVehiculo
                                   select new
                                   {
                                       IdPesaje = P.id,
                                       Placa = P.PlacaCamion,
                                       IdImagen = I.idFotoPesaje,
                                       Imagen = I.ImagenVehiculo
                                   }).ToListAsync();

                if (query == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error: " + ex.Message)
                };
            }
        }
    }
}