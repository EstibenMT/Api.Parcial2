using Api.Parcial2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Parcial2.Classes
{
	public class clsPartialClass
	{
        private BDExamen2Entities db = new BDExamen2Entities();
        public PartialClass PCl { get; set; }

        public string GrabarImagenProducto(int id, List<string> Imagenes)
        {
            try
            {
                foreach (string imagen in Imagenes)
                {
                    PartialImage pImage = new PartialImage();
                    pImage.IdClass = id;
                    pImage.NombreImagen = imagen;
                    db.PartialImages.Add(pImage);
                    db.SaveChanges();
                }
                return "Se grabó la información en la base de datos";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public IQueryable ListarImagenes(int id)
        {
            return from P in db.Set<PartialClass>()
                   join I in db.Set<PartialImage>() on P.IdClass equals I.IdImagen
                   where P.IdClass == id
                   orderby I.NombreImagen
                   select new
                   {
                       Id = P.IdClass,
                       ClName = P.Name,
                       Imagen = I.NombreImagen
                   };
        }  
    }
}