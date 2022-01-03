using System.Collections.Generic;

namespace DomeinLaag.Klassen
{
    public class Bos
    {
        #region Properties
        public int Id { get; set; }
        public int Xmin { get; set; }
        public int Xmax { get; set; }
        public int Ymin { get; set; }
        public int Ymax { get; set; }
        public List<Boom> Bomen { get; set; }
        public List<Aap> Apen { get; set; }
        #endregion

        #region Constructors
        public Bos(int id, int xmin, int xmax, int ymin, int ymax)
        {
            Id = id;
            Xmin = xmin;
            Xmax = xmax;
            Ymin = ymin;
            Ymax = ymax;
        }

        public Bos(int id, int xmin, int xmax, int ymin, int ymax, List<Boom> bomen) : this(id, xmin, xmax, ymin, ymax)
        {
            Bomen = bomen;
        }

        public Bos(int id, int xmin, int xmax, int ymin, int ymax, List<Boom> bomen, List<Aap> apen) : this(id, xmin, xmax, ymin, ymax, bomen)
        {
            Apen = apen;
        }

        public Bos(int id, int xmin, int xmax, int ymin, int ymax, List<Aap> apen) : this(id, xmin, xmax, ymin, ymax)
        {
            Apen = apen;
        }
        #endregion
    }
}
