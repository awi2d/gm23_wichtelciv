using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Clickable
{
    public abstract void OnClick(GameObject lastObject);
    public abstract void unselect();
}
