using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Components that implement this interfaces can act as the source for
// dragging a "DragItem"
// /</summary>

public interface IDragSource<T> where T : class 
{
    // <summary>
    // What item type currently resides in this source?
    // </summary>
    T GetItem();

    // <summary>
    // What is the quantity of items in this source
    // </summary>
    int GetNumber();

    // <summary>
    // Remove a given number of items from the source. Number variable should never exceed the number returned by "GetNumber".
    // </summary>
    void RemoveItems(int number);

}
