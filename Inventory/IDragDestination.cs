using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Components that implement this interfaces can act as the destination for
// dragging a 'DragItem'.
// </summary>

public interface IDragDestination<T> where T : class
{
    // <summary>
    // How many of the given item can be accepted.If there is not limit Int.MaxValue should be returned.
    // </summary>
    int MaxAcceptable(T item);

    // <summary>
    // Update the UI and any data to reflect adding the item to this destination. 
    // </summary>
    void AddItems(T item, int number);
}
