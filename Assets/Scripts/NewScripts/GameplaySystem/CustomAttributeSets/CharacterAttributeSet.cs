using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributeSet : AttributeSet
{
    public GameplayAttribute Health;
    public GameplayAttribute MaxHealth;
    
    public GameplayAttribute Stagger;
    public GameplayAttribute MaxStagger;
    //this is for passing on damage or calculating. not damage that the character does. 
    public GameplayAttribute Damage;

    public GameplayAttribute MoveSpeed;

    

}
