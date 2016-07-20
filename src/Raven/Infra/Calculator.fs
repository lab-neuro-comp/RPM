module Infra.Calculator

open System
open System.Linq

/// Gets the column the age belongs to.
let rec getColumnLoop(intervals: string[], index: int, age: int): int =
    if index = intervals.Length
        then -1
        else let raw = intervals.ElementAt(index).Split(' ')
             let lower = int(raw.ElementAt 0)
             let upper = int(raw.ElementAt 1)
             Console.WriteLine(sprintf "%d %d %d" lower upper age)
             if age >= lower && age <= upper
                then index
                else getColumnLoop(intervals, index+1, age)

let getColumn(intervals: string[], age: int): int =
    getColumnLoop(intervals, 1, age)

/// Takes a table and the number of correct answers and turn them into
/// the percentile the subject belongs to.
let CalculateResult(table: string[][], score: int, age: int): int =
    getColumn(table.ElementAt 0, age)


