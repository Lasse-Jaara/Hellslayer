using UnityEngine;
using UnityEngine.UI;

public class hp_bar_ui : MonoBehaviour
{
    public Slider slider;
    public void hp_bar_set(int hp)
    {
        slider.value = hp;
    }

    /// <summary>
    /// Sets the maximum value of the slider and sets the value to the new maximum
    /// </summary>
    /// <param name="hp">The new maximum value of the slider</param>
    public void hp_bar_max(int hp)  
    {
        slider.maxValue = hp;
        slider.value = hp;
    }
}