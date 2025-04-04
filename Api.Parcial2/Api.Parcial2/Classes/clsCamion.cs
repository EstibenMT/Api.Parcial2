using Api.Parcial2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Api.Parcial2.Classes
{
	public class clsCamion
	{
        private BDExamen2Entities db = new BDExamen2Entities();
        public Camion camion { get; set; }

        public async Task<(Camion camion, bool estado)> ConsultarXPlacaAsync(string placa)
        {
            try
            {
                Camion camion = await db.Camions.FirstOrDefaultAsync(c => c.Placa.Equals(placa));
                return (camion, true);
            }
            catch (Exception)
            {                
                return (null, false);
            }
        }
    }
}