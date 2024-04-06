namespace BackEnd.Model
{
    public class Affiche
    {
        public int idAffiche;
        public string titre;
        public string sujet;
        public string description;
        public int localisationAffiche;
        public string image;
        public string couverture;
        public double prix;
        public string lien;
        public bool existant;
        
        public override string ToString()
        {
            return $"Affiche [id: {idAffiche}, titre: {titre}, sujet: {sujet}, description: {description}, localisation: {localisationAffiche}, image: {image}, couverture: {couverture}, prix: {prix}, lien: {lien}, existant: {existant}]";
        }
    }
}