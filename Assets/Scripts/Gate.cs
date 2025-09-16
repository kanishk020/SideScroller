using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gate1, gate2,gate3;

    [SerializeField] private Color red,green, blue;

    [SerializeField] private string gateType;
    [SerializeField] private ParticleSystem particles;

    /// <summary>
    /// sets gate color on basis of string provided
    /// </summary>
    /// <param name="color"></param>
    public void SetGate(string color)
    {
        gateType = color;
        switch (color)
        {
            case "red": SetGateColor(red);                           
                break;
            case "blue":SetGateColor(blue); 
                break;
            case "green":SetGateColor(green);
                break;
            default : SetGateColor(Color.white);
                break;
        }
    }
    /// <summary>
    /// actual function to set gate and particle color
    /// </summary>
    /// <param name="color"></param>
    void SetGateColor(Color color)
    {
        gate1.color = color;
        gate2.color = color;
        gate3.color = color;
        var mainModule = particles.main;
        mainModule.startColor = color;
    }

    /// <summary>
    /// Checks for gate pass trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gateType == GameManager.instance.currentColor)
            {
                GameManager.instance.ScoreUP();
                particles.Play();
            }
            else
            {
                GameManager.instance.GameOver();
            }
            Debug.Log("CONTACT");
        }
    }
}
