using TMPro;
using UnityEngine;

public class UIGeneratorSection : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private TMP_InputField _widthInput;
    [SerializeField] private TMP_InputField _heightInput;

    #endregion

    #region Methods
    public void Generate()
    {
        int.TryParse(_widthInput.text, out int width);
        int.TryParse(_heightInput.text, out int height);

        if(width <= 0 || height <= 0)
        {
            Debug.Log("width and height must be greater then 0");
            return;
        }

        SystemManager.Instance.MapManager.GenerateMap(width, height);
    }

    #endregion
}