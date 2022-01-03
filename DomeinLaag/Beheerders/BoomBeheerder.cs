using DomeinLaag.Exceptions;
using DomeinLaag.Klassen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Beheerders
{
    public class BoomBeheerder
    {
        #region Methods
        public static List<Boom> GenereerBomen(Bos bos, int aantal)
        {
            try
            {
                Random r = new();
                List<Boom> bomen = new();

                while (bomen.Count < aantal + 1)
                {
                    bool MagBoomToevoegen = true;
                    Boom boom = new(bomen.Count, r.Next(bos.Xmin, bos.Xmax), r.Next(bos.Ymin, bos.Ymax));
                    foreach (Boom b in bomen)
                    {
                        if (b.X == boom.X && b.Y == boom.Y)
                        {
                            MagBoomToevoegen = false;
                        }
                    }
                    if (MagBoomToevoegen)
                    {
                        bomen.Add(boom);
                    }
                }

                return bomen;
            }
            catch (Exception ex)
            {
                throw new BoomBeheerderException("Bomen genereren niet gelukt.", ex);
            }
        }
        #endregion
    }
}
