using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIEditorMenu : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private List<Button> _navigationButtons;

    #endregion

    #region Methods
    public void UpdateButtonsDisplay(Button btn)
    {
        btn.GetComponent<Image>().color = Color.white;

        foreach (var b in _navigationButtons.Where(b => b != btn))
        {
            b.GetComponent<Image>().color = Color.gray;
        }
    }
    #endregion

    #region Unity-API
    private void Start()
    {
        UpdateButtonsDisplay(_navigationButtons[0]);
    }
    #endregion
}