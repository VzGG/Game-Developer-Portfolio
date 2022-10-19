using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
    [SerializeField] Canvas enemyCanvas;
    [SerializeField] Image enemyHealthBar;
    [SerializeField] Health enemyHealth;

    public void SetEnemyCanvasActive(bool status) 
    { 
        enemyCanvas.gameObject.SetActive(status); 
    }

    private void Update()
    {
        // if (enemyCanvas || enemyHealthBar == null) { return; }

        enemyHealthBar.fillAmount = enemyHealth.GetHealthPercentage();
    }


}
