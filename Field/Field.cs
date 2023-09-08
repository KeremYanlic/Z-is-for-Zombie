using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldController))]
[DisallowMultipleComponent]
public class Field : MonoBehaviour
{
    public FieldDetailsSO fieldDetailsSO;

    public bool hasEnteredOnce = false;


}
