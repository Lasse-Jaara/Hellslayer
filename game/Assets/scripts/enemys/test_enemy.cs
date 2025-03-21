using UnityEngine;

public class test_enemy : MonoBehaviour
{

    public hp_system hp_System;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // Get the healt_system component from the player
            hp_system player_hp = other.GetComponent<hp_system>();


            //Debug.Log("Player entered the area!");
            if (player_hp != null)
            {
                player_hp.take_damage(10);
                Debug.Log("Player took damage!");
            }
        }
    }
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //        {
    //        Debug.Log("Player left the area!");
    //        }
    //}
}
