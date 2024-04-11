namespace BackEnd.Model
{
    public class Stand
    {
        public int idStand;
        public int standType;
        public string nom;
        public string sujet;
        public string description;
        public string image;
        public double prix;
        public bool existant;
        public string lien;
        
        public override string ToString()
        {
            return $"Stand [id: {idStand}, titre: {nom}, sujet: {sujet}, description: {description}, typestand: {standType}, image: {image}, prix: {prix}, lien: {lien}, existant: {existant}]";
        }
    }
}