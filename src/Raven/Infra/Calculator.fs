module Infra.Calculator

open System
open System.Linq

let CalculatePercentile(total: int, piece: int): int = 
    int(100.0 * float(piece) / float(total))

let getRaw(interval: string): string[] =
    interval.Split ' '

let rec getColumnLoop(intervals: string[], index: int, limit: int, age: int): int =
    let raw = getRaw(intervals.ElementAt index)
    let lower = int(raw.ElementAt 0)
    let upper = int(raw.ElementAt 1)
    if index = limit
        then -1
        else if age >= lower && age < upper
            then index
            else getColumnLoop(intervals, index+1, limit, age)

let getColumn(intervals: string[], age: int): int =
    getColumnLoop(intervals, 1, intervals.Length+1, age)

let CalculateResult(table: string[][], age: int, percentile: int): int =
    getColumn(table.ElementAt 0, age)


