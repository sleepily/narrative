using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle_BookArray : Puzzle
{
    List<Interactable_Book> books = new List<Interactable_Book>();
    List<Interactable_Book> booksToSwap = new List<Interactable_Book>();

    public int bookCount = 10;
    public float bookWidth = .3f;
    public string solution = "12 34 5678";

    protected override void StartFunctions()
    {
        base.StartFunctions();

        solution = FormatSolutionString();
        ReorderBooks();
        booksToSwap.Clear();
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

        // In case the solution string is the wrong length, use default solution
        if (solution.Length != 10)
            solution = "12 34 5678";

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
        int bookIndexA = books.IndexOf(booksToSwap[0]);
        int bookIndexB = books.IndexOf(booksToSwap[1]);

        Interactable_Book tempBook = books[bookIndexA];
        books[bookIndexA] = books[bookIndexB];
        books[bookIndexB] = tempBook;

        books[bookIndexA].Swapped();
        books[bookIndexB].Swapped();

        booksToSwap.Clear();

        ReorderBooks();
    }

    void ReorderBooks()
    {
        for (int bookIndex = 0; bookIndex < bookCount; bookIndex++)
        {
            Vector3 bookPosition = books[bookIndex].transform.localPosition;
            bookPosition.x = bookWidth * bookIndex;
            books[bookIndex].transform.localPosition = bookPosition;
        }

        PuzzleCheck();
    }

    public override void PuzzleCheck()
    {
        base.PuzzleCheck();

        CompareWithSolution();
    }

    void CompareWithSolution()
    {
        string bookIDs = "";

        // Get current book order
        foreach (Interactable_Book book in books)
            bookIDs += book.GetBookID();

        // Compare book order with solution
        bool isCorrectSolution = (solution == bookIDs);

        if (isCorrectSolution)
            PuzzleSolved();
    }

    public override bool PuzzleSolved()
    {
        // Placeholder solve event
        transform.position += Vector3.up;

        isSolved = true;
        return true;
    }
}
