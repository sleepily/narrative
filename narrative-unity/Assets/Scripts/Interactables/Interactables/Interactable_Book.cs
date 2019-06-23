using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

public class Interactable_Book : Interactable
{
    Color selectedGlowColor = Color.magenta;
    bool selectedForSwap = false;

    Puzzle_BookArray puzzle;
    int bookID = -1;

    public int debugNumber = 0;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        GetBookID();
    }

    /*
     * Debugging the book order
     */
    void OnDrawGizmos()
    {
        Handles.Label(transform.position, debugNumber.ToString());
    }

    public int GetBookID()
    {
        if (bookID >= 0)
            return bookID;

        string id = name;

        // Find integer number in GameObject name
        id = Regex.Match(id, @"\d+").Value;

        bookID = int.Parse(id);

        return bookID;
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        puzzle = GetComponentInParent<Puzzle_BookArray>();
    }

    /*
     * Prevent color glow change if book is selected for swap
     */
    public override void Focus()
    {
        if (puzzle.isSolved)
            return;

        if (selectedForSwap)
            return;

        base.Focus();
    }

    public override void Unfocus()
    {
        if (selectedForSwap)
            return;

        base.Unfocus();
    }

    public override void Interact()
    {
        // Don't call base.Interact(), since no dialogue is needed

        if (puzzle.isSolved)
        {
            puzzle.TriggerDialogue();
            return;
        }

        if (!selectedForSwap)
            SelectForSwap();
        else
            DeselectForSwap();
    }

    void SelectForSwap()
    {
        selectedForSwap = true;
        puzzle.AddBookToSwitch(this);

        SetGlowColor(selectedGlowColor, isInstantTransition: true);
    }

    void DeselectForSwap()
    {
        selectedForSwap = false;
        puzzle.RemoveBookToSwitch(this);

        Focus();
    }

    public void Swapped()
    {
        selectedForSwap = false;

        SetGlowColor(Color.clear, isInstantTransition: true);
    }
}
