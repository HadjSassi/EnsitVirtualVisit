namespace BackEnd.Model
{
    public class Avatar
    {
        public string avatarName;
        public string description;
        public string jokes;
        public bool existant;
        public bool npc;
        public string sexe;
        public string mail;
        
        public override string ToString()
        {
            return $"Avatar [avatarName: {avatarName}, description: {description}, mail: {mail}, jokes: {jokes}, sexe: {sexe}, npc: {npc}, existant: {existant}]";
        }
        
    }
}