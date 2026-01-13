using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    void moveSpawner();
    void setEnabled(bool val);
    float getZPos();
    void speedUpSpawner(float val);
}
