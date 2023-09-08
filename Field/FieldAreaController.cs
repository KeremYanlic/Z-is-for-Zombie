using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class FieldAreaController : MonoBehaviour
{


    private Field field;
    private void Awake()
    {
        //Load components
        field = GetComponentInParent<Field>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Settings.playerTag))
        {
            if (!field.hasEnteredOnce)
            {
                TextPopupUI.Instance.UpdateText(field.fieldDetailsSO.fieldName);
                field.hasEnteredOnce = true;
            }
        }
    }
}
