module Infra.Calculator

open System
open System.Linq

/// Gets the column the age belongs to.
let getColumn(intervals: string[], age: int): int =
    let rec getColumnLoop(intervals: string[], index: int, age: int): int =
        if index = intervals.Length
            then -1
            else let raw = intervals.ElementAt(index).Split(' ')
                 let lower = int(raw.ElementAt 0)
                 let upper = int(raw.ElementAt 1)
                 if age >= lower && age <= upper
                    then index
                    else getColumnLoop(intervals, index+1, age)
    getColumnLoop(intervals, 1, age)

/// Gets the percentile based on choosen column and on the score
let getPercentile(table: string[][], score: int, column: int): int = 
    let rec getPercentileLoop(table: string[][], score: int, row: int, column: int): int = 
        Console.WriteLine(score.ToString() + " " + row.ToString() + " " + column.ToString())
        if row >= table.Length - 1
            then 1
            else let upper = int(table.ElementAt(row).ElementAt(column))
                 let lower = int(table.ElementAt(row+1).ElementAt(column))
                 if score <= upper && score >= lower
                    then int(table.ElementAt(row).ElementAt(0))
                    else getPercentileLoop(table, score, row+1, column)
    getPercentileLoop(table, score, 1, column)

/// Takes a table and the number of correct answers and turn them into
/// the percentile the subject belongs to.
let CalculateResult(table: string[][], score: int, age: int): int =
    let column = getColumn(table.ElementAt 0, age)
    if column >= 0
        then getPercentile(table, score, column)
        else 1



