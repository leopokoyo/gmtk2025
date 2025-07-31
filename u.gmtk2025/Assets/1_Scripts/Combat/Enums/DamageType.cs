namespace _1_Scripts.Combat.Enums
{
    public enum DamageType
    {
        Physical = 0,
        Magical = 1,
        
        Normal = Physical | Magical,
        
        Slash = Physical,
        Bludgeoning = Physical,
        Piercing = Physical,
        
        Fire = Magical, 
        Water = Magical,
        Electric = Magical,
    }
}
