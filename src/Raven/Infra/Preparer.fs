#light
module Infra.Preparer

open System.IO
open System.Collections.Generic
open System.Linq

let file2strings(input: string): list<string> =
    let inlet = new StreamReader(input)
    let rec loop(line: string, box: list<string>): list<string> =
        if obj.ReferenceEquals(line, null) 
            then box 
            else loop(inlet.ReadLine(), box @ [line])
    loop(inlet.ReadLine(), List.empty)

let GetPaths(input: string): obj[] =
    input
    |> file2strings
    |> List.map(fun line -> line.Split(' ').GetValue(0))
    |> List.toArray

//let GetTests(input: string) =
//    input
//    |> file2strings
//    |> List.map(fun line -> 
//        [ for bit in line.Split(' ') -> bit ]
//        |> List.tail
//        |> List.reduce(fun (acc, it) -> acc.Concat(" ").Concat(it)))
//    |> List.toArray

