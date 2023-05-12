using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Oswald.UI
{
    /// <summary>
    /// An enemy's UI displays the enemies health, break point, and is positioned next to an enemy.
    /// </summary>
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] Canvas enemyCanvas;
        [SerializeField] Image enemyHealthSupportBar;
        [SerializeField] Image enemyHealthBar;
        [SerializeField] Image enemyBreakBar;
        [SerializeField] Vector3 offsetPosition;

        public GameObject GetEnemyCanvas() { return this.enemyCanvas.gameObject; }

        public void UpdateHealth(Health enemyHealth)
        {
            enemyHealthBar.fillAmount = enemyHealth.GetHealthPercentage();
            enemyHealthSupportBar.fillAmount = enemyHealth.GetHealthPercentage() + 0.05f;
        }

        public void UpdateBreakPoint(Break enemyBreakPoint)
        {
            enemyBreakBar.fillAmount = enemyBreakPoint.GetCurrentBreakPointPercentage();
        }

        public void UpdatePosition(Vector3 enemyPosition)
        {
            this.enemyCanvas.transform.position = enemyPosition + offsetPosition;
        }
    }
}


