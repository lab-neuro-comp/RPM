#light
module Infra.Calculator

open System
open System.Linq

/// Gets the column the age belongs to.
let getColumn (intervals : string[]) (age : int) =
    let rec getColumnLoop index =
        if index = intervals.Length
            then -1
            else let raw = intervals.[index] |> (fun it -> it.Split(' '))
                 let lower = raw.[0] |> int
                 let upper = raw.[1] |> int
                 if age >= lower && age <= upper
                    then index
                    else getColumnLoop (index+1)
    getColumnLoop 1

/// Gets the percentile based on choosen column and on the score
let getPercentile (table : string[][]) (score : int) (column : int) =
    let rec getPercentileLoop row =
        if row >= table.Length - 1
            then 1
            else let upper = table.[row].[column] |> int
                 let lower = table.[row+1].[column] |> int
                 if score <= upper && score > lower
                    then table.[row].[0] |> int
                    else getPercentileLoop (row+1)
    getPercentileLoop 1

/// Takes a table and the number of correct answers and turn them into
/// the percentile the subject belongs to.
let CalculateResult (table : string[][]) (score : int) (age : int) : int =
    let column = getColumn table.[0] age
    if column >= 0
        then getPercentile table score column
        else 1

/// TODO Write doc for this function
let RelateSeriesAndAnswers (series : string[]) (answers : bool[]) : Map<string, int> =
    let limit = series.Length
    let isCorrect i =
        if answers.[i]
            then 1
            else 0
    let rec loop map i =
        if i < limit
            then let correct = isCorrect i
                 if Map.containsKey series.[i] map
                     then loop (Map.add series.[i] (map.[series.[i]] + correct) map)
                               (i+1)
                     else loop (Map.add series.[i] correct map) 
                               (i+1)
            else map
    loop Map.empty 0
