using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Clickable
{

    public abstract int get_posx();
    public abstract int get_posy();
    public abstract void OnClick(GameObject lastObject);
    public abstract void unselect();

    public abstract void OnGameTick();
}
