using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class hp_system : MonoBehaviour
{
    public int max_hp = 100;
    public int current_hp = 0;
    public Animator anim;

    public hp_bar_ui hp_bar; // reference to hp_bar_ui script




    void Start()
    {
        current_hp = max_hp;
        hp_bar.hp_bar_max(max_hp);
    }
    public void take_damage(int amount)  // void is a function that does not return anything
    {
        current_hp -= amount;
        
        hp_bar.hp_bar_set(current_hp);

        if(current_hp <= 0)
        {
            Debug.Log("Player died!");
        }
    }
}
