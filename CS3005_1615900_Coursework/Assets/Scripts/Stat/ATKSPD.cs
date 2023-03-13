using UnityEngine;

[System.Serializable]
public class ATKSPD : Stat
{
    public ATKSPD(float givenATKSPD)
    {
        this.Value = givenATKSPD;
        this.Name = "ATK SPD";
        this.RatingPerStat = 8f;
        this.Description = "The character's attack speed, it determines how fast a character attacks";
    }
}