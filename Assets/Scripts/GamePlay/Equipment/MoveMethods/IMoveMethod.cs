using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveMethod
{
    void Attach(GameObject location, GameObject targetlocation);
	void Stop();

    IEnumerator Move(Transform location, Transform targetlocation);
}
