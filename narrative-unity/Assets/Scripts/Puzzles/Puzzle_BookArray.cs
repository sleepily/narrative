using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle_BookArray : Puzzle
{
    List<Interactable_Book> books = new List<Interactable_Book>();
    List<Interactable_Book> booksToSwap = new List<Interactable_Book>();

    int bookCount = 10;

    [Tooltip("Controls how far the books will spread.")]
    public float bookDistance = .3f;

    protected override void StartFunctions()
    {
        base.StartFunctions();

        // Format the string for the methods to use it
        solution = FormatSolutionString();

        // Order books by scene hierarchy
        ReorderBooks();
    }

    protected override void GetAllComponents()
    {
        base.GetAllComponents();

        books = GetComponentsInChildren<Interactable_Book>().ToList();
        books.Capacity = bookCount;
    }

    string FormatSolutionString()
    {
        string formattedSolution = "";

        // Trim the string in case there are spaces before/after contents
        solution.Trim(' ');

        foreach (char letter in solution)
        {
            if (letter == ' ')
            {
                formattedSolution += 0;
                continue;
            }

            formattedSolution += letter;
        }

        return formattedSolution;
    }

    public void AddBookToSwitch(Interactable_Book book)
    {
        booksToSwap.Add(book);

        if (booksToSwap.Count > 1)
            SwapBooks();
    }

    public bool RemoveBookToSwitch(Interactable_Book book)
    {
        return booksToSwap.Remove(book);
    }

    public void SwapBooks()
    {
        if (isSolved)
        {
            PuzzleSolvedReminder();
            ResetSwappedBooks();

            return;
        }

        // Get book indices for swapping
        int bookIndexA = books.IndexOf(booksToSwap[0]);
        int bookIndexB = books.IndexOf(booksToSwap[1]);

        // Swap books
        Interactable_Book tempBook = books[bookIndexA];
        books[bookIndexA] = books[bookIndexB];
        books[bookIndexB] = tempBook;

        ResetSwappedBooks();
        ReorderBooks();
    }

    void ResetSwappedBooks()
    {
        booksToSwap[0].Swapped();
        booksToSwap[1].Swapped();

        booksToSwap.Clear();
    }

    void ReorderBooks()
    {
        for (int bookIndex = 0; bookIndex < bookCount; bookIndex++)
        {
            Vector3 bookPosition = books[bookIndex].transform.localPosition;
            bookPosition.z = bookDistance * bookIndex;
            books[bookIndex].transform.localPosition = bookPosition;
        }

        PuzzleCheck();
    }

    public override bool PuzzleCheck()
    {
        base.PuzzleCheck();

        string bookIDs = "";

        // Get current book order
        foreach (Interactable_Book book in books)
            bookIDs += book.GetBookID();

        // Compare book order with solution
        bool isCorrectSolution = (solution == bookIDs);

        if (isCorrectSolution)
            PuzzleSolved();

        return isCorrectSolution;
    }

    public override void PuzzleSolved()
    {
        flowchart.SetBooleanVariable("isSolved", true);

        base.PuzzleSolved();
    }

    public override void PuzzleSolvedReminder()
    {
        base.PuzzleSolvedReminder();

        TriggerDialogue();
    }
}
