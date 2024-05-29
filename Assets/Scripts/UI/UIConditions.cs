using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uIConditions = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
