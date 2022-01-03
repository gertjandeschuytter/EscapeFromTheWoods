using System;
using System.Collections.Generic;

namespace DomeinLaag.Klassen
{
    public class Boom
    {
        #region Properties
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        #endregion

        #region Constructors
        public Boom(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
        #endregion

        #region Methods
        public static void ZetAapInBoom(List<Aap> apen, List<Boom> bomen)
        {
            foreach (Aap aap in apen)
            {
                Random random = new();
                int boomId = random.Next(0, bomen.Count);
                bool magInBoom = true;
                foreach (Aap aap1 in apen)
                {
                    if (aap1.Bomen == null)
                    {
                        if (boomId == aap1.Bomen[0].Id)
                        {
                            magInBoom = false;
                        }
                    }
                }
                if (magInBoom)
                {
                    aap.Bomen.Add(bomen[boomId]);
                }
            }
        }
        public override string ToString()
        {
            return $"Id:{Id} | X:{X} | Y:{Y}";
        }
        #endregion
    }
}
