#light
module Infra.ParamExtractor
open System
open System.Linq
open System.Collections.Generic

let GetImages (inlet : string[]) : string[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 0)

let GetNoOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 1)
    |> Array.map(fun it -> int it)

let GetCorrectOptions (inlet : string[]) : int[] =
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 2)
    |> Array.map(fun it -> int it)

let GetSeries (inlet : string[]) : string[] = 
    inlet
    |> Array.map(fun it -> it.Split ' ')
    |> Array.map(fun box -> box.ElementAt 3)

let GenerateTableFromCsv (inlet : string[]) : string[][] =
    inlet
    |> Array.map(fun box -> box.Split ',')

let GetTopResult (table : string[][]) : int = 
    let rec loop i top = 
        if i = table.Length
            then top
            else let maybe = int(table.ElementAt(i).ElementAt(0))
                 if maybe > top then loop (i+1) maybe else loop (i+1) top
    loop 1 0

let GetFloorResult (table : string[][]) : int = 
    let rec loop i top = 
        if i = table.Length
            then top
            else let maybe = int(table.ElementAt(i).ElementAt(0))
                 if maybe < top then loop (i+1) maybe else loop (i+1) top
    loop 1 100000

let GetSeriesList (table : string[][]) : string[] =
    table.ElementAt 0
    |> Array.filter (fun it -> it.Length > 0)