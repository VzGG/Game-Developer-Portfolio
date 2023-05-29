
/// <summary>
/// This is the actual stats of the character. These stats are used and pulled from all the other components
/// like Energy.cs, Health.cs, etc. The health.cs, energy.cs is the real component that determines how the character lives, it just grabs the stats from this class
/// </summary>
/// 
[System.Serializable]
public class MyStat
{
    // We have a base stat + stats from equipment
    public float HP = 100f;
    public float EN = 100f;
    public float ATK = 5f;
    public float DEF = 0f;
    public float ENRGN = 5f;
    public float ATKSPD = 100f;

    public float HPPercent = 100f;
    public float ENPercent = 100f;
    public float ATKPercent = 100f;
    public float DEFPercent = 100f;
    public float ENRGNPercent = 100f;
    public float ATKSPDPercent = 100f;

    public float HealthRegen = 0f;              // Helmet
    public float DamageReduction = 0f;
    // Plate
    public float CriticalChance = 0f;           // Glove
    public float CriticalDamage = 1f;           // Glove
    public float EvasionRate = 0f;              // Boots

    public float DisplayDamageReduction()
    {
        if (DamageReduction > 90f)
            return 90f;
        else
            return DamageReduction;
    }

    public float DisplayCriticalChance()
    {
        if (CriticalChance > 100f)
            return 100f;
        else
            return CriticalChance;
    }

    public float DisplyEvasionRate()
    {
        if (EvasionRate > 90f)
            return 90f;
        else
            return EvasionRate;
    }

    public MyStat()
    {

    }
}