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
             if age >= lower && age <= upper
                then index
                else getColumnLoop(intervals, index+1, age)

let getColumn(intervals: string[], age: int): int =
    getColumnLoop(intervals, 1, age)

/// Gets the percentile based on choosen column and on the score
// This logic is wrong!!! Must consider the upper and lower bounds
let rec getPercentileLoop(table: string[][], score: int, row: int, column: int): int = 
    if row = table.Length
        then 0
        else let value = int(table.ElementAt(row).ElementAt(column))
             if value = score
                then int(table.ElementAt(row).ElementAt(0))
                else getPercentileLoop(table, score, row+1, column)

let getPercentile(table: string[][], score: int, column: int): int = 
    getPercentileLoop(table, 1, score, column)

/// Takes a table and the number of correct answers and turn them into
/// the percentile the subject belongs to.
let CalculateResult(table: string[][], score: int, age: int): int =
    let column = getColumn(table.ElementAt 0, age)
    if column >= 0
        then getPercentile(table, score, column)
        else 1



