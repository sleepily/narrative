using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class Interactable_Jug : Interactable
{
    Color selectedGlowColor = Color.green;

    public int maxCapacity { get; private set; } = 0;
    bool selected = false;

    [HideInInspector]
    public int currentCapacity = 0;

    Puzzle_Jugs puzzle;

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        puzzle = GetComponentInParent<Puzzle_Jugs>();
        GetCapacity();
    }

    void GetCapacity()
    {
        string id = name;

        // Find integer number in GameObject name
        id = Regex.Match(id, @"\d+").Value;

        maxCapacity = int.Parse(id);

        if (maxCapacity == 8)
            currentCapacity = maxCapacity;
    }

    /*
     * Draw currentCapacity in Gizmos
     */
    void OnDrawGizmos() => Handles.Label(transform.position, $"{maxCapacity.ToString()}:{currentCapacity.ToString()}");

    /*
     * Prevent color glow change if book is selected for swap
     */
    public override void Focus()
    {
        if (puzzle.isSolved)
            return;

        if (selected)
            return;

        base.Focus();
    }

    public override void Interact()
    {
        base.Interact();

        SelectForSwap();
    }

    void SelectForSwap()
    {
        selected = true;
        puzzle.AddJug(this);

        SetGlowColor(selectedGlowColor, isInstantTransition: true);
    }

    void DeselectForSwap()
    {
        selected = false;
        puzzle.RemoveJug(this);

        Focus();
    }

    public void Refilled()
    {
        selected = false;

        SetGlowColor(Color.clear, isInstantTransition: true);
    }
}
