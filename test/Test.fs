module Test

open Fable.Core
open Fable.Core.JsInterop
open Fable.RegexProvider

type IAssert =
    [<Emit("$0.true($1)")>]  abstract isTrue: bool -> unit
    [<Emit("$0.false($1)")>] abstract isFalse: bool -> unit

let test(name: string, f: IAssert->unit): unit = importDefault "ava"

test("SafeRegex works", fun t ->
    let reg1 = SafeRegex.Create<"^hello world!?$">()
    let reg2 = SafeRegex.Create<"^hello world!?$", ignoreCase=true>()
    reg1.IsMatch("hello world!") |> t.isTrue
    reg1.IsMatch("Hello World!") |> t.isFalse
    reg2.IsMatch("Hello World!") |> t.isTrue
)
