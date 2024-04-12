namespace BackEnd.Model
{
    public class Avatar
    {
        public string url;
        public string avatarName;
        public string description;
        public string jokes;
        public bool existant;
        public string sexe;
        public string mail;
        
        public override string ToString()
        {
            return $"Avatar [avatarUrl: {url},avatarName: {avatarName}, description: {description}, mail: {mail}, jokes: {jokes}, sexe: {sexe}, existant: {existant}]";
        }
        
    }
}