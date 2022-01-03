using DomeinLaag.Exceptions;
using DomeinLaag.Klassen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomeinLaag.Beheerders
{
    public class AapBeheerder
    {
        public static List<Aap> GenereerApen(int aantal, List<string> namen)
        {
            try
            {
                List<Aap> apen = new();

                if (aantal > 4)
                {
                    throw new AapBeheerderException("Het aantal mag niet groter zijn dan 4.");
                }

                if (aantal > namen.Count)
                {
                    throw new AapBeheerderException("Het aantal kan niet groter zijn dan het aantal beschikbare namen.");
                }

                if (namen == null)
                {
                    throw new AapBeheerderException("Er moet minstens 1 naam zijn.");
                }

                for (int i = 0; i < aantal; i++)
                {
                    Aap aap = new(i + 1, namen[i]);
                    apen.Add(aap);
                }

                return apen;
            }
            catch (Exception ex)
            {
                throw new AapBeheerderException("GenereerApen is niet gelukt.", ex);
            }
        }

        public static List<Aap> GenereerApen(List<string> namen)
        {
            try
            {
                List<Aap> apen = new();
                
                if (namen == null)
                {
                    throw new AapBeheerderException("Er moet minstens 1 naam zijn.");
                }

                for (int i = 0; i < namen.Count; i++)
                {
                    Aap aap = new(i + 1, namen[i]);
                    apen.Add(aap);
                }

                return apen;
            }
            catch (Exception ex)
            {
                throw new AapBeheerderException("GenereerApen is niet gelukt.", ex);
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task Spring(Bos bos, List<Boom> bomen, List<Aap> apen)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            int ontsnapt = 0;
            while (ontsnapt < apen.Count)
            {
                foreach (Aap aap in apen)
                {
                    int x = aap.Bomen[^1].X;
                    int y = aap.Bomen[^1].Y;
                    if (x != -1)
                    {
                        double tempAfstand = Math.Max(bos.Xmax, bos.Ymax) * 2;
                        Boom gekozenBoom = null;

                        foreach (Boom boom in bomen)
                        {
                            if (boom != aap.Bomen[^1])
                            {
                                bool isNieuweBoom = true;
                                foreach (Boom boomAap in aap.Bomen)
                                {
                                    if (boomAap.Id == boom.Id)
                                    {
                                        isNieuweBoom = false;
                                    }
                                }

                                foreach (Aap aap1 in apen)
                                {
                                    foreach (Boom boom1 in aap1.Bomen)
                                    {
                                        if (boom1 == boom)
                                        {
                                            isNieuweBoom = false;
                                        }
                                    }
                                }

                                if (isNieuweBoom)
                                {
                                    double afstand = Math.Sqrt(Math.Pow(x - boom.X, 2) + Math.Pow(y - boom.Y, 2));
                                    if (afstand < tempAfstand)
                                    {
                                        tempAfstand = afstand;
                                        gekozenBoom = boom;
                                    }
                                }
                            }
                        }

                        double afstandTotRand = new List<double>() { bos.Ymax - y, bos.Xmax - x, y - bos.Ymin, x - bos.Xmin }.Min();

                        if (tempAfstand < afstandTotRand)
                        {
                            aap.Bomen.Add(gekozenBoom);
                        }
                        else
                        {
                            aap.Bomen.Add(new Boom(-1, -1, -1));
                            ontsnapt++;
                        }
                    }

                }
            }
        }
        public static async Task ProcessAap(Bos bos)
        {
           Spring(bos, bos.Bomen, bos.Apen);
        }
    }
}
