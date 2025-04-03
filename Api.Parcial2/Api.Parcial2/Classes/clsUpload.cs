﻿using Api.Parcial2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Api.Parcial2.Classes
{
	public class clsUpload
	{
        private BDExamen2Entities db = new BDExamen2Entities();
        public string Datos { get; set; }
        public string Proceso { get; set; }
        public HttpRequestMessage request { get; set; }
        private List<string> Archivos;
        public async Task<HttpResponseMessage> GrabarArchivo(bool Actualizar)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se envió un archivo para procesar");
            }
            string root = HttpContext.Current.Server.MapPath("~/Archivos");
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                bool Existe = false;
                //Lee el contenido de los archivos
                await request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileData.Count > 0)
                {
                    Archivos = new List<string>();
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        string fileName = file.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        if (File.Exists(Path.Combine(root, fileName)))
                        {
                            if (Actualizar)
                            {
                                //El archivo ya existe en el servidor, no se va a cargar, se va a eliminar el temporal y se devolverá un error
                                File.Delete(Path.Combine(root, fileName));
                                //actualiza el nombre del primer archivo
                                File.Move(file.LocalFileName, Path.Combine(root, fileName));
                                return request.CreateResponse(System.Net.HttpStatusCode.OK, "Se actualizó la imagen");
                            }
                            else
                            {
                                //El archivo ya existe en el servidor, no se va a cargar, se va a eliminar el temporal y se devolverá un error
                                File.Delete(Path.Combine(root, file.LocalFileName));
                                Existe = true;
                            }
                            //return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "El archivo ya existe");
                        }
                        else
                        {
                            if (!Actualizar)
                            {
                                Existe = false;
                                //Agrego en una lista el nombre de los archivos que se cargaron 
                                Archivos.Add(fileName);
                                //Renombra el archivo temporal
                                File.Move(file.LocalFileName, Path.Combine(root, fileName));
                            }
                            else
                            {
                                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "El archivo no existe, se debe agregar");
                            }
                        }
                    }
                    if (!Existe)
                    {
                        //Se genera el proceso de gestión en la base de datos
                        string RptaBD = ProcesarBD();
                        //Termina el ciclo, responde que se cargó el archivo correctamente
                        return request.CreateResponse(System.Net.HttpStatusCode.OK, "Se cargaron los archivos en el servidor, " + RptaBD);
                    }
                    else
                    {
                        return request.CreateErrorResponse(System.Net.HttpStatusCode.Conflict, "El archivo ya existe");
                    }
                }
                else
                {
                    return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, "No se envió un archivo para procesar");
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public HttpResponseMessage DescargarArchivo(string Imagen)
        {
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~/Archivos");
                string Archivo = Path.Combine(Ruta, Imagen);
                if (File.Exists(Archivo))
                {
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    var stream = new FileStream(Archivo, FileMode.Open);
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = Imagen;
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    return response;
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No se encontró el archivo solicitado.")
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error interno del servidor: {ex.Message}")
                };
            }
        }

        public HttpResponseMessage EliminarArchivo(string Imagen)
        {
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~/Archivos");
                string Archivo = Path.Combine(Ruta, Imagen);
                if (File.Exists(Archivo))
                {
                    PartialImage pImage = db.PartialImages.FirstOrDefault(x => x.NombreImagen == Imagen);
                    if (pImage != null)
                    {
                        db.PartialImages.Remove(pImage);
                        db.SaveChanges();
                        File.Delete(Archivo);
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("Se eliminó la imagen")
                        };
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("No se encontró la imagen en la base de datos")
                        };
                    }
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No se encontró el archivo a eliminar.")
                    };
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Error interno del servidor: {ex.Message}")
                };
            }
        }
        private string ProcesarBD()
        {
            switch (Proceso.ToUpper())
            {
                case "PARTIALCLASS":
                    clsPartialClass producto = new clsPartialClass();
                    return producto.GrabarImagenProducto(Convert.ToInt32(Datos), Archivos);
                default:
                    return "No se ha definido el proceso en la base de datos";
            }
        }
    }
}
