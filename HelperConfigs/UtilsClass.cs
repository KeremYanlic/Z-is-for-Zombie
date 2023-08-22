using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class UtilsClass
{
    public static Camera mainCam;

    public static Vector3 GetWorldMousePosition()
    {
        if (mainCam == null) mainCam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);

        Vector3 worldPosition = mainCam.ScreenToWorldPoint(mousePos);

        worldPosition.z = 0;
        return worldPosition;
    }
    public static float GetAngleFromMousePosition(Vector3 vector)
    {
        float radius = Mathf.Atan2(vector.y, vector.x);
        float angle = radius * Mathf.Rad2Deg;
        return angle;
    }

    //<summary>
    //Get the direction vector from an angle in degrees
    //</summary>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }
    public static AimDirection GetAimDirectionFromAngle(float angle)
    {
        AimDirection aimDirection;

        if (angle >= 22f && angle <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }
        else if (angle > 67f && angle <= 112f)
        {
            aimDirection = AimDirection.Up;
        }
        else if (angle > 112f && angle <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }
        else if ((angle > 158f && angle <= 180f) || (angle > -180f && angle <= -135f))
        {
            aimDirection = AimDirection.Left;
        }
        else if (angle > -135f && angle <= -45f)
        {
            aimDirection = AimDirection.Down;
        }
        else if ((angle > -45f && angle <= 0f) || (angle > 0f && angle < 22f))
        {
            aimDirection = AimDirection.Right;
        }
        else
        {
            aimDirection = AimDirection.Right;
        }
        return aimDirection;
    }

    public static GameObject GetClosestGameobject(Collider2D[] colliders)
    {
        GameObject closestGameobject = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {

            float distance = Vector3.Distance(collider.gameObject.transform.position, GameManager.Instance.GetPlayer().transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestGameobject = collider.gameObject;
            }
            else
            {
                closestGameobject = null;
            }
        }


        return closestGameobject;
    }

    // <summary>
    // Null value debug check
    // </summary>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object valueToCheck)
    {
        bool error = false;

        if (valueToCheck == null)
        {
            Debug.Log(fieldName + " is null and must contain a value in object " + thisObject.name.ToString());
            error = true;
        }
        else
        {
            error = false;
        }

        return error;
    }

    // <summary>
    // Enumerable value debug check
    // </summary>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjecToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjecToCheck == null)
        {
            Debug.Log(fieldName + " is null in object " + thisObject);
            error = true;
        }

        foreach (var item in enumerableObjecToCheck)
        {
            if (item == null)
            {
                Debug.Log(fieldName + " has null values in object " + thisObject);
                error = true;
            }
            else
            {
                count++;
            }
        }
        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject);
            error = true;
        }
        return error;
    }



    // <summary>
    // empty string debug check
    // </summary>

    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        bool error = false;

        if (stringToCheck == "")
        {
            Debug.Log(fieldName + " is empty and it must contain at least one character in object " + thisObject.name.ToString());
            error = true;
        }
        else
        {
            error = false;
        }
        return error;
    }

    // <summary>
    // Positive value debug check. - If zero is allowed , the valuetoCheck can be at least 0. However, if zero is not allowed then it must be bigger than 0.
    // </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + "'s value is smaller than zero. It must be at least 0 in object " + thisObject.name.ToString());
                error = true;
            }
            else { error = false; }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + "'s value is equal or smaller than zero. It must be bigger than 0 in object " + thisObject.name.ToString());
                error = true;
            }
            else { error = false; }
        }
        return error;
    }
    public static bool ValidateCheckPositiveRange(Object thisObject, string minValueFieldName, float minValueToCheck, string maxValueFieldName, float maxValueToCheck)
    {
        bool error = false;

        if (minValueToCheck > maxValueToCheck)
        {
            Debug.Log(minValueToCheck + " must be less than or equal to the " + maxValueToCheck + " in object " + thisObject.name.ToString());
            error = true;
        }

        return error;

    }
}
