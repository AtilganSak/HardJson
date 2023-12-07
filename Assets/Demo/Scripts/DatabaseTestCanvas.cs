using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DatabaseTestCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button saveButton;
    
    [SerializeField] Button loadButton;
    [SerializeField] TMP_Text resultText;

    [SerializeField] Button resetSceneButton;

    private void OnEnable()
    {        
        saveButton.onClick.AddListener(Pressed_Save_Button);
        loadButton.onClick.AddListener(Pressed_Load_Button);
        resetSceneButton.onClick.AddListener(Pressed_ResetScene_Button);
    }

    private void Pressed_ResetScene_Button()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Pressed_Save_Button()
    {
        GameDB.Instance.testString = inputField.text;
        GameDB.Instance.Save();
    }
    private void Pressed_Load_Button()
    {
        resultText.text = GameDB.Instance.testString;
    }
}
