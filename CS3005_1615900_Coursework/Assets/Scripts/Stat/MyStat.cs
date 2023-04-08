
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
    public float ATKSPD = 1f;

    public MyStat()
    {

    }

    public MyStat(float givenHP, float givenEN, float givenATK, float givenDEF, float givenENRGN, float givenATKSPD)
    {
        HP = givenHP;
        EN = givenEN;
        ATK = givenATK;
        DEF = givenDEF;
        ENRGN = givenENRGN;
        ATKSPD = givenATKSPD;
    }
}