using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthIndicator : MonoBehaviour
{

    public TextMeshProUGUI textField;
    public TMP_FontAsset fontForDead;
    public Color32 fontColorForDead;
    Health health;
    float displayedHealth;


    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        displayedHealth = health.Current - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float value = health.Current;
        if (Mathf.Abs(displayedHealth - value) >= 0.00001f)
        {
            displayedHealth = value;
            if (displayedHealth > 0)
            {
                textField.text = $"{value}";
            }
            else
            {
                textField.font = fontForDead;
                textField.color = fontColorForDead;
                textField.fontSize = 0.5f;
                textField.text = "Dead";
            }
        }
    }
}
