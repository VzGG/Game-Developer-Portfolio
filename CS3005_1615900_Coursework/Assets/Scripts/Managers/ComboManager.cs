using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;

    [SerializeField] private GameObject _comboPanel;
    [SerializeField] private GameObject _prefabComboTextPanel;

    private void Update()
    {
        if (_weapon == null) { return; }

        if (Input.GetKeyDown(KeyCode.J))
        {
            DisplayOrHideCombo();
        }
    }

    public void DisplayOrHideCombo()
    {
        // Show or hide the panel
        _comboPanel.SetActive(!_comboPanel.activeInHierarchy);

        for (int i = 0; i < _comboPanel.transform.childCount; i++)
        {
            var combo = _comboPanel.transform.GetChild(i);
            var comboName = combo.transform.GetChild(0).gameObject;
            var metaText = comboName.transform.GetChild(0).gameObject.GetComponent<Text>();
            Text comboTextComponent = metaText.GetComponent<Text>();
            if (i < _weapon.GetAirComboList().Length)
            {
                string[] s = _weapon.GetAirComboList()[i].Split(':');
                // Show the air combo list
                comboTextComponent.text = s[0];
                DisplayComboHelper(combo.gameObject, s[1]);
            }
            else if (i >= _weapon.GetAirComboList().Length && i < _weapon.GetAirComboList().Length + _weapon.GetGroundedComboList().Length)
            {
                // Show the grounded combo list
                string[] s = _weapon.GetGroundedComboList()[Mathf.Abs(_weapon.GetAirComboList().Length - i)].Split(':');
                comboTextComponent.text = s[0];
                DisplayComboHelper(combo.gameObject, s[1]);
            }
            else
            {
                combo.gameObject.SetActive(!combo.gameObject.activeInHierarchy);
            }
        }
    }

    private void DisplayComboHelper(GameObject combo, string inputs)
    {
        var moves = inputs.Split(' ');
        for (int j = 1; j < combo.transform.childCount; j++)
        {
            var inputName = combo.transform.GetChild(j).gameObject;
            var metaText = inputName.transform.GetChild(0).gameObject;
            var metaTextComponent = metaText.GetComponent<Text>();

            if (j < moves.Length)
            {
                metaTextComponent.text = moves[j];
            }
            else
            {
                inputName.gameObject.SetActive(!inputName.gameObject.activeInHierarchy);
            }
        }
    }
}
