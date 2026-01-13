using MoonsecDeobfuscator.Bytecode.Models;

namespace MoonsecDeobfuscator.Deobfuscation;

public static class OpCodes
{
    // https://github.com/Trollicus/ironbrew-2/tree/master/IronBrew2/Obfuscator/Opcodes

    public static readonly Dictionary<string, (OpCode, Action<Instruction>?)> StaticOpcodes = new()
    {
        // MOVE A B R(A) := R(B)
        // stk[inst[OP_A]] = stk[inst[OP_B]]
        ["91909190"] = (OpCode.Move, null),

        // LOADK A Bx R(A) := Kst(Bx)
        // stk(inst[OP_A], inst[OP_B])
        ["1419090"] = (OpCode.LoadK, instruction => instruction.B--),

        // LOADBOOL A B C R(A) := (Bool)B; if (C) PC++
        // stk[inst[OP_A]] = inst[OP_B] ~= 0
        ["91902690"] = (OpCode.LoadBool, null),
        // stk[inst[OP_A]] = inst[OP_B] ~= 0; pc = pc + 1
        ["91902690291529"] = (OpCode.LoadBool, instruction => instruction.C = 1),

        // LOADNIL A B R(A) := ... := R(B) := nil
        // for _302 = inst[OP_A], inst[OP_B] do stk[_302] = nil end
        ["13909091"] = (OpCode.LoadNil, null),

        // GETUPVAL A B R(A) := UpValue[B]
        // stk[inst[OP_A]] = upv[inst[OP_B]]
        ["91909290"] = (OpCode.GetUpval, null),

        // GETGLOBAL A Bx R(A) := Gbl[Kst(Bx)]
        // stk[inst[OP_A]] = env[inst[OP_B]]
        ["91909390"] = (OpCode.GetGlobal, instruction => instruction.B--),

        // GETTABLE A B C R(A) := R(B)[RK(C)]
        //stk[inst[OP_A]] = stk[inst[OP_B]][stk[inst[OP_C]]]
        ["9190991909190"] = (OpCode.GetTable, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]][inst[OP_C]]
        ["91909919090"] = (OpCode.GetTable, instruction => instruction.C += 255),

        // SETGLOBAL A Bx Gbl[Kst(Bx)] := R(A)
        // env[inst[OP_B]] = stk[inst[OP_A]]
        ["93909190"] = (OpCode.SetGlobal, instruction => instruction.B--),

        // SETUPVAL A B UpValue[B] := R(A)
        // upv[inst[OP_B]] = stk[inst[OP_A]]
        ["92909190"] = (OpCode.SetUpval, null),

        // SETTABLE A B C R(A)[RK(B)] := RK(C)
        // stk[inst[OP_A]][stk[inst[OP_B]]] = stk[inst[OP_C]]
        ["9919091909190"] = (OpCode.SetTable, null),
        // stk[inst[OP_A]][stk[inst[OP_B]]] = inst[OP_C]
        ["99190919090"] = (OpCode.SetTable, instruction => instruction.C += 255),
        // stk[inst[OP_A]][inst[OP_B]] = stk[inst[OP_C]]
        ["99190909190"] = (OpCode.SetTable, instruction => instruction.B += 255),
        // stk[inst[OP_A]][inst[OP_B]] = inst[OP_C]
        ["991909090"] = (OpCode.SetTable, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // NEWTABLE A B C R(A) := {} (size = B,C)
        // stk[inst[OP_A]] = {}
        ["919033"] = (OpCode.NewTable, null),

        // SELF A B C R(A+1) := R(B); R(A) := R(B)[RK(C)]
        /*
            local _17 = inst[OP_A]
            local _18 = stk[inst[OP_B]]
            stk[_17 + 1] = _18
            stk[_17] = _18[inst[OP_C]]
        */
        ["909190911591990"] = (OpCode.Self, instruction => instruction.C += 255),
        /*
            local _511 = inst[OP_A]
            local _512 = stk[inst[OP_B]]
            stk[_511 + 1] = _512
            stk[_511] = _512[stk[inst[OP_C]]]
        */
        ["90919091159199190"] = (OpCode.Self, null),

        // ADD A B C R(A) := RK(B) + RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] + stk[inst[OP_C]]
        ["91901591909190"] = (OpCode.Add, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]] + inst[OP_C]
        ["919015919090"] = (OpCode.Add, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] + stk[inst[OP_C]]
        ["919015909190"] = (OpCode.Add, instruction => instruction.B += 255),
        // stk[inst[OP_A]] = inst[OP_B] + inst[OP_C]
        ["9190159090"] = (OpCode.Add, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // SUB A B C R(A) := RK(B) – RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] - stk[inst[OP_C]]
        ["91901691909190"] = (OpCode.Sub, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]] - inst[OP_C]
        ["919016919090"] = (OpCode.Sub, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] - stk[inst[OP_C]]
        ["919016909190"] = (OpCode.Sub, instruction => instruction.B += 255),
        // stk[inst[OP_A]] = inst[OP_B] - inst[OP_C]
        ["9190169090"] = (OpCode.Sub, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // MUL A B C R(A) := RK(B) * RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] * stk[inst[OP_C]]
        ["91901791909190"] = (OpCode.Mul, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]] * inst[OP_C]
        ["919017919090"] = (OpCode.Mul, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] * stk[inst[OP_C]]
        ["919017909190"] = (OpCode.Mul, instruction => instruction.B += 255),
        // stk[inst[OP_A]] = inst[OP_B] * inst[OP_C]
        ["9190179090"] = (OpCode.Mul, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // DIV A B C R(A) := RK(B) / RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] / stk[inst[OP_C]]
        ["91901891909190"] = (OpCode.Div, null),
        //stk[inst[OP_A]] = stk[inst[OP_B]] / inst[OP_C]
        ["919018919090"] = (OpCode.Div, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] / stk[inst[OP_C]]
        ["919018909190"] = (OpCode.Div, instruction => instruction.B += 255),
        // stk[inst[OP_A]] = inst[OP_B] / inst[OP_C]
        ["9190189090"] = (OpCode.Div, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // MOD A B C R(A) := RK(B) % RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] % stk[inst[OP_C]]
        ["91901991909190"] = (OpCode.Mod, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]] % inst[OP_C]
        ["919019919090"] = (OpCode.Mod, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] % stk[inst[OP_C]]
        ["919019909190"] = (OpCode.Mod, instruction => instruction.B += 255),
        // stk[inst[OP_A]] = inst[OP_B] % inst[OP_C]
        ["9190199090"] = (OpCode.Mod, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // POW A B C R(A) := RK(B) ^ RK(C)
        // stk[inst[OP_A]] = stk[inst[OP_B]] ^ stk[inst[OP_C]]
        ["91902091909190"] = (OpCode.Pow, null),
        // stk[inst[OP_A]] = stk[inst[OP_B]] ^ inst[OP_C]
        ["919020919090"] = (OpCode.Pow, instruction => instruction.C += 255),
        // stk[inst[OP_A]] = inst[OP_B] ^ stk[inst[OP_C]]
        ["919020909190"] = (OpCode.Pow, instruction => instruction.B += 255),
        // // stk[inst[OP_A]] = inst[OP_B] ^ inst[OP_C]
        ["9190209090"] = (OpCode.Pow, instruction =>
        {
            instruction.B += 255;
            instruction.C += 255;
        }),

        // UNM A B R(A) := -R(B)
        // stk[inst[OP_A]] = -stk[inst[OP_B]]
        ["9190319190"] = (OpCode.Unm, null),

        // NOT A B R(A) := not R(B)
        // stk[inst[OP_A]] = not stk[inst[OP_B]]
        ["9190349190"] = (OpCode.Not, null),

        // LEN A B R(A) := length of R(B)
        // stk[inst[OP_A]] = #stk[inst[OP_B]]
        ["9190289190"] = (OpCode.Len, null),

        // CONCAT A B C R(A) := R(B).. ... ..R(C)
        /*
            local _216 = inst[OP_B]
            local _217 = stk[_216]
            for _218 = _216 + 1, inst[OP_C] do
                _217 = _217 .. stk[_218]
            end
            stk[inst[OP_A]] = _217
        */
        ["909113159032919190"] = (OpCode.Concat, null),

        // JMP sBx PC += sBx
        // pc = inst[OP_B]
        ["2990"] = (OpCode.Jmp, instruction => instruction.B -= instruction.PC + 1),

        // EQ A B C if ((RK(B) == RK(C)) ~= A) then PC++
        // if stk[inst[OP_A]] == stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["101225919091902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
        }),
        // if stk[inst[OP_A]] == inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012259190902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if inst[OP_A] == stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012259091902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
        }),
        // if inst[OP_A] == inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["10122590902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if stk[inst[OP_A]] ~= stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["101226919091902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
        }),
        // if stk[inst[OP_A]] ~= inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012269190902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
            instruction.C += 255;
        }),
        // if inst[OP_A] ~= stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012269091902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
        }),
        // if inst[OP_A] ~= inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["10122690902915292990"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
            instruction.C += 255;
        }),
        // pc = ((stk[inst[OP_A]] == stk[inst[OP_C]]) and inst[OP_B]) or (1 + pc)
        ["2936352591909190901529"] = (OpCode.Eq, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
        }),

        // LT A B C if ((RK(B) < RK(C)) ~= A) then PC++
        // if stk[inst[OP_A]] < stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["101221919091902915292990"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
        }),
        // if inst[OP_A] < stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012219091902915292990"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
        }),
        // if stk[inst[OP_A]] < inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012219190902915292990"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if inst[OP_A] < inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["10122190902915292990"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if stk[inst[OP_A]] < stk[inst[OP_C]] then pc = inst[OP_B] else pc = pc + 1 end
        ["101221919091902990291529"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
        }),
        // if inst[OP_A] < stk[inst[OP_C]] then pc = inst[OP_B] else pc = pc + 1 end
        ["1012219091902990291529"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
        }),
        // if stk[inst[OP_A]] < inst[OP_C] then pc = inst[OP_B] else pc = pc + 1 end
        ["1012219190902990291529"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
            instruction.C += 255;
        }),
        // if inst[OP_A] < inst[OP_C] then pc = inst[OP_B] else pc = pc + 1 end
        ["10122190902990291529"] = (OpCode.Lt, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
            instruction.C += 255;
        }),

        // LE A B C if ((RK(B) <= RK(C)) ~= A) then PC++
        // if stk[inst[OP_A]] <= stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["101223919091902915292990"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
        }),
        // if inst[OP_A] <= stk[inst[OP_C]] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012239091902915292990"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
        }),
        // if stk[inst[OP_A]] <= inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["1012239190902915292990"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if inst[OP_A] <= inst[OP_C] then pc = pc + 1 else pc = inst[OP_B] end
        ["10122390902915292990"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 0;
            instruction.C += 255;
        }),
        // if stk[inst[OP_A]] <= stk[inst[OP_C]] then pc = inst[OP_B] else pc = pc + 1 end
        ["101223919091902990291529"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
        }),
        // if stk[inst[OP_A]] <= inst[OP_C] then pc = inst[OP_B] else pc = pc + 1 end
        ["1012239190902990291529"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A;
            instruction.A = 1;
            instruction.C += 255;
        }),
        // if inst[OP_A] <= stk[inst[OP_C]] then pc = inst[OP_B] else pc = pc + 1 end
        ["1012239091902990291529"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
        }),
        // if inst[OP_A] <= inst[OP_C] then pc = inst[OP_B] else pc = pc + 1 end
        ["10122390902990291529"] = (OpCode.Le, instruction =>
        {
            instruction.B = instruction.A + 255;
            instruction.A = 1;
            instruction.C += 255;
        }),

        // TEST A C if not (R(A) <=> C) then PC++
        // if stk[inst[OP_A]] then pc = pc + 1 else pc = inst[OP_B] end
        ["101291902915292990"] = (OpCode.Test, instruction =>
        {
            instruction.B = 0;
            instruction.C = 0;
        }),
        // if not stk[inst[OP_A]] then pc = pc + 1 else pc = inst[OP_B] end
        ["10123491902915292990"] = (OpCode.Test, instruction =>
        {
            instruction.B = 0;
            instruction.C = 1;
        }),

        // TESTSET A B C if (R(B) <=> C) then R(A) := R(B) else PC++
        // local _3 = stk[inst[OP_C]] if _3 then pc = pc + 1 else stk[inst[OP_A]] = _3 pc = inst[OP_B] end
        ["9190101229152991902990"] = (OpCode.TestSet, instruction =>
        {
            instruction.B = instruction.C;
            instruction.C = 0;
        }),
        // local _14 = stk[inst[OP_C]] if not _14 then pc = pc + 1 else stk[inst[OP_A]] = _14 pc = inst[OP_B] end
        ["919010123429152991902990"] = (OpCode.TestSet, instruction =>
        {
            instruction.B = instruction.C;
            instruction.C = 1;
        }),

        // CALL A B C R(A), ... ,R(A+C-2) := R(A)(R(A+1), ... ,R(A+B-1))
        // stk[inst[OP_A]]()
        ["149190"] = (OpCode.Call, null),
        // local _52 = inst[OP_A]; stk[_52] = stk[_52](stk[_52 + 1])
        ["909114919115"] = (OpCode.Call, null),
        // local _19 = inst[OP_A]; stk[_19] = stk[_19](unpack(stk, _19 + 1, inst[OP_B]))
        ["9091149114511590"] = (OpCode.Call, instruction => instruction.B -= instruction.A - 1),
        // local _354 = inst[OP_A]; stk[_354](stk[_354 + 1])
        ["9014919115"] = (OpCode.Call, null),
        // local _194 = inst[OP_A]; stk[_194](unpack(stk, _194 + 1, inst[OP_B]))
        ["90149114511590"] = (OpCode.Call, instruction => instruction.B -= instruction.A - 1),
        // local _80 = inst[OP_A]; stk[_80] = stk[_80](unpack(stk, _80 + 1, top))
        ["9091149114511530"] = (OpCode.Call, null),
        /*
            local _324 = inst[OP_A]
            local _325 = {
                stk[_324](stk[_324 + 1])
            }
            local _326 = 0
            for _327 = _324, inst[OP_C] do
                _326 = _326 + 1
                stk[_327] = _325[_326]
            end
        */
        ["903314919115139015919"] = (OpCode.Call, instruction => instruction.C -= instruction.A - 2),
        // local _85 = inst[OP_A]; stk[_85] = stk[_85]()
        ["90911491"] = (OpCode.Call, null),
        /*
            local _75 = inst[OP_A]
            local _76, _77 = _R(stk[_75]())
            top = (_77 + _75) - 1
            local _78 = 0
            for _79 = _75, top do
                _78 = _78 + 1
                stk[_79] = _76[_78]
            end
        */
        ["90141491301615133015919"] = (OpCode.Call, null),
        /*
            local _63 = inst[OP_A]
            local _64, _65 = _R(stk[_63](stk[_63 + 1]))
            top = (_65 + _63) - 1
            local _66 = 0
            for _67 = _63, top do
                _66 = _66 + 1
                stk[_67] = _64[_66]
            end
        */
        ["901414919115301615133015919"] = (OpCode.Call, instruction => instruction.B -= instruction.A - 1),
        /*
            local _423 = inst[OP_A]
            local _424, _425 = _R(stk[_423](unpack(stk, _423 + 1, inst[OP_B])))
            top = (_425 + _423) - 1
            local _426 = 0
            for _427 = _423, top do
                _426 = _426 + 1
                stk[_427] = _424[_426]
            end
        */
        ["9014149114511590301615133015919"] = (OpCode.Call, instruction => instruction.B -= instruction.A - 1),
        // local _133 = inst[OP_A]; stk[_133](unpack(stk, _133 + 1, top))
        ["90149114511530"] = (OpCode.Call, null),
        /*
            local _57 = inst[OP_A]
            local _58 = {
                stk[_57](unpack(stk, _57 + 1, inst[OP_B]))
            }
            local _59 = 0
            for _60 = _57, inst[OP_C] do
                _59 = _59 + 1
                stk[_60] = _58[_59]
            end
        */
        ["9033149114511590139015919"] = (OpCode.Call, instruction =>
        {
            instruction.B -= instruction.A - 1;
            instruction.C -= instruction.A - 2;
        }),
        /*
            local _1066 = inst[OP_A]
            local _1067 = {
                stk[_1066](unpack(stk, _1066 + 1, top))
            }
            local _1068 = 0
            for _1069 = _1066, inst[OP_C] do
                _1068 = _1068 + 1
                stk[_1069] = _1067[_1068]
            end
        */
        ["9033149114511530139015919"] = (OpCode.Call, instruction => instruction.C -= instruction.A - 2),
        /*
            local _511 = inst[OP_A]
            local _512 = {
                stk[_511]()
            }
            local _513 = inst[OP_C]
            local _514 = 0
            for _515 = _511, _513 do
                _514 = _514 + 1
                stk[_515] = _512[_514]
            end
        */
        ["90331491901315919"] = (OpCode.Call, instruction => instruction.C -= instruction.A - 2),
        /*
            local _189 = inst[OP_A]
            local _190, _191 = _R(stk[_189](unpack(stk, _189 + 1, top)))
            top = (_191 + _189) - 1
            local _192 = 0
            for _193 = _189, top do
                _192 = _192 + 1
                stk[_193] = _190[_192]
            end
        */
        ["9014149114511530301615133015919"] = (OpCode.Call, null),

        // TAILCALL A B C return R(A)(R(A+1), ... ,R(A+B-1))
        // local _64 = inst[OP_A]; do return stk[_64](unpack(stk, _64 + 1, inst[OP_B])) end
        ["9027149114511590"] = (OpCode.TailCall, instruction => instruction.B -= instruction.A - 1),
        // local _135 = inst[OP_A]; do return stk[_135](unpack(stk, _135 + 1, top)); end
        ["9027149114511530"] = (OpCode.TailCall, null),
        // do return stk[inst[OP_A]](); end
        ["27149190"] = (OpCode.TailCall, null),

        // RETURN A B return R(A), ... ,R(A+B-2)
        // do return; end
        ["27"] = (OpCode.Return, null),
        // do return stk[inst[OP_A]]; end
        ["279190"] = (OpCode.Return, null),
        // local _1003 = inst[OP_A]; do return unpack(stk, _1003, top); end
        ["9027145130"] = (OpCode.Return, null),
        // local _879 = inst[OP_A]; do return unpack(stk, _879, _879 + inst[OP_B]); end
        ["902714511590"] = (OpCode.Return, instruction => instruction.B += 2),
        // local _601 = inst[OP_A]; do return stk[_601], stk[_601 + 1]; end
        ["9027919115"] = (OpCode.Return, null),

        /*
            FORLOOP A sBx R(A) += R(A+2)
            if R(A) <?= R(A+1) then {
                PC += sBx; R(A+3) = R(A)
            }
        */
        /*
            local _144 = inst[OP_A]
            local _145 = stk[_144 + 2]
            local _146 = stk[_144] + _145
            stk[_144] = _146
            if _145 > 0 then
                if _146 <= stk[_144 + 1] then
                    pc = inst[OP_B]
                    stk[_144 + 3] = _146
                end
            elseif _146 >= stk[_144 + 1] then
                pc = inst[OP_B]
                stk[_144 + 3] = _146
            end
        */
        ["909115159191101122102391152990911524911529909115"] = (OpCode.ForLoop, instruction =>
            instruction.B -= instruction.PC + 1),

        // FORPREP A sBx R(A) -= R(A+2); PC += sBx
        /*
            local _200 = inst[OP_A]
            local _201 = stk[_200]
            local _202 = stk[(_200 + 2)]
            if (_202 > 0) then
                if (_201 > stk[(_200 + 1)]) then
                    pc = inst[OP_B]
                else
                    stk[(_200 + 3)] = _201
                end
            elseif (_201 < stk[(_200 + 1)]) then
                pc = inst[OP_B]
            else
                stk[(_200 + 3)] = _201
            end
        */
        ["909191151011122210122291152990911521911529909115"] = (OpCode.ForPrep, instruction =>
            instruction.B -= instruction.PC + 2),

        /*
            TFORLOOP A C R(A+3), ... ,R(A+2+C) := R(A)(R(A+1), R(A+2));
            if R(A+3) ~= nil then {
                R(A+2) = R(A+3);
            } else {
                PC++;
            }
        */
        /*
            local _180 = inst[OP_A]
            local _181 = inst[OP_C]
            local _182 = _180 + 2
            local _183 = {
                stk[_180](stk[_180 + 1], stk[_182])
            }
            for _184 = 1, _181 do
                stk[_182 + _184] = _183[_184]
            end
            local _185 = _183[1]
            if _185 then
                stk[_182] = _185
                pc = inst[OP_B]
            else
                pc = pc + 1
            end
        */
        ["909015331491911591139115991012912990291529"] = (OpCode.TForLoop, instruction => instruction.B = 0),

        // SETLIST A B C R(A)[(C-1)*FPF+i] := R(A+i), 1 <= i <= B
        // I know long table SETLIST is missing.
        // local _273 = inst[OP_A]; local _275 = stk[_273]; for _277 = (_273 + 1), top do; table.insert(_275, stk[_277]); end
        ["909113153014691"] = (OpCode.SetList, null),
        // local _504 = inst[OP_A]; local _505 = stk[_504]; for _506 = (_504 + 1), inst[OP_B] do table.insert(_505, stk[_506]); end
        ["909113159014691"] = (OpCode.SetList, instruction => instruction.B -= instruction.A),

        // CLOSE A close all variables in the stack up to (>=) R(A)
        /*
            local _277 = {}
            for _278 = 1, #lupv do
                local _279 = lupv[_278]
                for _280 = 0, #_279 do
                    local _281 = _279[_280]
                    local _282 = _281[1]
                    local _283 = _281[2]
                    if (_282 == stk) and (_283 >= inst[OP_A]) then
                        _277[_283] = _282[_283]
                        _281[1] = _277
                    end
                end
            end
        */
        ["3313289132899910352512490999"] = (OpCode.Close, null),

        // CLOSURE A Bx R(A) := closure(KPROTO[Bx], R(A), ... ,R(A+n))
        /*
            SNIP
        */
        ["990331473333927999999913902915299291012259916331991633299152891901483"] = (OpCode.Closure, null),
        // stk[inst[OP_A]] = wrap_proto(protos[inst[OP_B]], nil, env)
        ["91901489903"] = (OpCode.Closure, null),

        // VARARG A B R(A), R(A+1), ..., R(A+B-1) = vararg
        /*
            local _273 = inst[OP_A]
            top = ((_273 + varargz) - 1)
            for _276 = _273, top do
                local _274 = vararg[(_276 - _273)]
                stk[_276] = _274
            end
        */
        ["903016151330941691"] = (OpCode.VarArg, null),
        /*
            local _328 = inst[OP_A]
            local _329 = inst[OP_B]
            for _330 = _328, _329 do
                stk[_330] = vararg[_330 - _328]
            end
        */
        ["909013919416"] = (OpCode.VarArg, instruction => instruction.B -= instruction.A - 1)
    };
}