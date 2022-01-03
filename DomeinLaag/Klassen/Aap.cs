using System.Collections.Generic;

namespace DomeinLaag.Klassen
{
    public class Aap
    {
        #region Properties
        public int Id { get; set; }
        public string Naam { get; set; }
        public List<Boom> Bomen { get; set; } = new();
        #endregion

        #region Constructors
        public Aap(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Id:{Id} | Naam:{Naam}";
        }
        #endregion
    }
}
