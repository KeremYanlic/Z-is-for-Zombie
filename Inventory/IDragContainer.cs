using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Acts both as a source and destination for dragging. If we are dragging
// between two containers then it is possible to swap items.
// </summary>

public interface IDragContainer<T> : IDragDestination<T>,IDragSource<T> where T : class
{
    
}
