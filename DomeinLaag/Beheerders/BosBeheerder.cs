using DomeinLaag.Exceptions;
using DomeinLaag.Klassen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomeinLaag.Beheerders
{
    public class BosBeheerder
    {
        public static Bos GenereerBos(int id, int grootte)
        {
            try
            {
                Console.WriteLine($"Start - Genereer bos {id}");
                Console.WriteLine($"Einde - Genereer bos {id}");
                return new Bos(id, 0, grootte, 0, grootte);
            }
            catch (Exception ex)
            {
                throw new BosBeheerderException("Kan bos niet genereren", ex);
            }
        }

        public static Bos GenereerBos(int id, int breedte, int hoogte)
        {
            try
            {
                Console.WriteLine($"Start - Genereer bos {id}");
                Console.WriteLine($"Einde - Genereer bos {id}");
                return new Bos(id, 0, breedte, 0, hoogte);
            }
            catch (Exception ex)
            {
                throw new BosBeheerderException("Kan bos niet genereren", ex);
            }
        }

        public static Bos GenereerBos(int id, int grootte, int aantalBomen, List<string> apen)
        {
            try
            {
                Console.WriteLine($"Start - Genereer bos {id}");
                Bos bos = new(id, 0, grootte, 0, grootte, AapBeheerder.GenereerApen(apen));
                bos.Bomen = BoomBeheerder.GenereerBomen(bos, aantalBomen);
                Boom.ZetAapInBoom(bos.Apen, bos.Bomen);
                Console.WriteLine($"Einde - Genereer bos {id}");
                return bos;
            }
            catch (Exception ex)
            {
                throw new BosBeheerderException("Kan bos niet genereren", ex);
            }
        }

        public static Bos GenereerBos(int id, int breedte, int hoogte, int aantalBomen, List<string> apen)
        {
            try
            {
                Console.WriteLine($"Start - Genereer bos {id}");
                Bos bos = new(id, 0, breedte, 0, hoogte, AapBeheerder.GenereerApen(apen));
                bos.Bomen = BoomBeheerder.GenereerBomen(bos, aantalBomen);
                Boom.ZetAapInBoom(bos.Apen, bos.Bomen);
                Console.WriteLine($"Einde - Genereer bos {id}");
                return bos;
            }
            catch (Exception ex)
            {
                throw new BosBeheerderException("Kan bos niet genereren", ex);
            }
        }
    }
}
