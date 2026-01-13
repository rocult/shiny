namespace MoonsecDeobfuscator.Bytecode.Models;

public enum OpCode
{
    // MOVE A B R(A) := R(B)
    Move,

    // LOADK A Bx R(A) := Kst(Bx)
    LoadK,

    // LOADBOOL A B C R(A) := (Bool)B; if (C) PC++
    LoadBool,

    // LOADNIL A B R(A) := ... := R(B) := nil
    LoadNil,

    // GETUPVAL A B R(A) := UpValue[B]
    GetUpval,

    // GETGLOBAL A Bx R(A) := Gbl[Kst(Bx)]
    GetGlobal,

    // GETTABLE A B C R(A) := R(B)[RK(C)]
    GetTable,

    // SETGLOBAL A Bx Gbl[Kst(Bx)] := R(A)
    SetGlobal,

    // SETUPVAL A B UpValue[B] := R(A)
    SetUpval,

    // SETTABLE A B C R(A)[RK(B)] := RK(C)
    SetTable,

    // NEWTABLE A B C R(A) := {} (size = B,C)
    NewTable,

    // SELF A B C R(A+1) := R(B); R(A) := R(B)[RK(C)]
    Self,

    // ADD A B C R(A) := RK(B) + RK(C)
    Add,

    // SUB A B C R(A) := RK(B) – RK(C)
    Sub,

    // MUL A B C R(A) := RK(B) * RK(C)
    Mul,

    // DIV A B C R(A) := RK(B) / RK(C)
    Div,

    // MOD A B C R(A) := RK(B) % RK(C)
    Mod,

    // POW A B C R(A) := RK(B) ^ RK(C)
    Pow,

    // UNM A B R(A) := -R(B)
    Unm,

    // NOT A B R(A) := not R(B)
    Not,

    // LEN A B R(A) := length of R(B)
    Len,

    // CONCAT A B C R(A) := R(B).. ... ..R(C)
    Concat,

    // JMP sBx PC += sBx
    Jmp,

    // EQ A B C if ((RK(B) == RK(C)) ~= A) then PC++
    Eq,

    // LT A B C if ((RK(B) < RK(C)) ~= A) then PC++
    Lt,

    // LE A B C if ((RK(B) <= RK(C)) ~= A) then PC++
    Le,

    // TEST A C if not (R(A) <=> C) then PC++
    Test,

    // TESTSET A B C if (R(B) <=> C) then R(A) := R(B) else PC++
    TestSet,

    // CALL A B C R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1))
    Call,

    // TAILCALL A B C return R(A)(R(A+1), ... ,R(A+B-1))
    TailCall,

    // RETURN A B return R(A), ... ,R(A+B-2)
    Return,

    /*
        FORLOOP A sBx R(A) += R(A+2)
        if R(A) <?= R(A+1) then {
            PC += sBx; R(A+3) = R(A)
        }
    */
    ForLoop,

    // FORPREP A sBx R(A) -= R(A+2); PC += sBx
    ForPrep,

    /*
        TFORLOOP A C R(A+3), ... ,R(A+2+C) := R(A)(R(A+1), R(A+2));
        if R(A+3) ~= nil then {
            R(A+2) = R(A+3);
        } else {
            PC++;
        }
    */
    TForLoop,

    // SETLIST A B C R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B
    SetList,

    // CLOSE A close all variables in the stack up to (>=) R(A)
    Close,

    // CLOSURE A Bx R(A) := closure(KPROTO[Bx], R(A), ... ,R(A+n))
    Closure,

    // VARARG A B R(A), R(A+1), ..., R(A+B-1) = vararg
    VarArg,

    Unknown
}