using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Attackable : MonoBehaviour
{
    public GameObject brokenObjectToSpawn;

    Interactable interactable;

    public UnityEvent eventToInvoke;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnMouseEnter()
    {
        if (Input.GetKeyDown(KeyCode.F))
            CheckForAttack();
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.F))
            CheckForAttack();
    }

    void CheckForAttack()
    {
        if (!PlayerAimInteraction.IsFocusable(interactable))
            return;

        if (!GameManager.GLOBAL.inventory.GetCurrentItem().itemStats.isWeapon)
            return;

        eventToInvoke.Invoke();
    }

    public void Break() => Destroy(gameObject, .5f);
}
