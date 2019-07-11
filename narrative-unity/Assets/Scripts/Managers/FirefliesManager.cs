using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FirefliesManager : MonoBehaviour
{
    List<Fireflies> fireflySwarms;

    private void Start()
    {
        fireflySwarms = FindObjectsOfType<Fireflies>().ToList();
    }

    public void MakeAllGlow()
    {
        foreach (Fireflies fireflies in fireflySwarms)
            fireflies.StartGlowing();
    }

    public void MakeAllStop()
    {
        foreach (Fireflies fireflies in fireflySwarms)
            fireflies.StopGlowing();
    }
}
