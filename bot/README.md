# Moonsec Deobfuscator
This project aims to deobfuscate Lua scripts obfuscated using **MoonSec V3**. The deobfuscation process produces a **Lua 5.1 bytecode file**, which you can then decompile with your favorite Lua decompiler.  Optionally, the tool can also generate a **disassembly** of the bytecode.

## Usage
### Installation & Building
To install and build run the following commands.
```bash
git clone https://github.com/tupsutumppu/MoonsecDeobfuscator.git
cd MoonsecDeobfuscator
dotnet build -c Release
```
### Command-Line
```plaintext
Devirtualize and dump bytecode to file:
	-dev -i <path to input> -o <path to output>

Devirtualize and dump bytecode disassembly to file:
	-dis -i <path to input> -o <path to output>
```
## Examples
Here we have a Lua script. Let's see what it looks like after deobfuscation.
```Lua
local chars = { "H", "e", "l", "l", "o ", "W", "o", "r", "l", "d", "!" }
local result = ""
for i = 1, #chars do
    result = result .. chars[i]
end
print(result)
```
After deobfuscation
```Lua
local v0 =  {
    "H",
    "e",
    "l",
    "l",
    "o ",
    "W",
    "o",
    "r",
    "l",
    "d",
    "!"
}; 
local v1 =  "";
for v2 =  1, #v0 do
    v1 = v1 .. v0[v2];
end;
print(v1);
```
Disassembly
```Lua
function func_f99e02d1(...)
    [Slots: 12, Upvalues: 0, Constants: 12]
    [   0]	    NewTable	|    0 |   11 |    0 |	R0 = {}
    [   1]	       LoadK	|    1 |    0 |    0 |	R1 = "H"
    [   2]	       LoadK	|    2 |    1 |    0 |	R2 = "e"
    [   3]	       LoadK	|    3 |    2 |    0 |	R3 = "l"
    [   4]	       LoadK	|    4 |    2 |    0 |	R4 = "l"
    [   5]	       LoadK	|    5 |    3 |    0 |	R5 = "o "
    [   6]	       LoadK	|    6 |    4 |    0 |	R6 = "W"
    [   7]	       LoadK	|    7 |    5 |    0 |	R7 = "o"
    [   8]	       LoadK	|    8 |    6 |    0 |	R8 = "r"
    [   9]	       LoadK	|    9 |    2 |    0 |	R9 = "l"
    [  10]	       LoadK	|   10 |    7 |    0 |	R10 = "d"
    [  11]	       LoadK	|   11 |    8 |    0 |	R11 = "!"
    [  12]	     SetList	|    0 |   11 |    1 |	
    [  13]	       LoadK	|    1 |    9 |    0 |	R1 = ""
    [  14]	       LoadK	|    2 |   10 |    0 |	R2 = 1
    [  15]	         Len	|    3 |    0 |    0 |	R3 = #R0
    [  16]	       LoadK	|    4 |   10 |    0 |	R4 = 1
    [  17]	     ForPrep	|    2 |    3 |    0 |	R2 -= R4; PC += 3
    [  18]	        Move	|    6 |    1 |    0 |	R6 = R1
    [  19]	    GetTable	|    7 |    0 |    5 |	R7 = R0[R5]
    [  20]	      Concat	|    1 |    6 |    7 |	R1 = R6 .. R7
    [  21]	     ForLoop	|    2 |   -4 |    0 |	R2 += R4; if loop continues then PC += -4; R5 = R2;
    [  22]	   GetGlobal	|    2 |   11 |    0 |	R2 = print
    [  23]	        Move	|    3 |    1 |    0 |	R3 = R1
    [  24]	        Call	|    2 |    2 |    1 |	R2(R3)
    [  25]	      Return	|    0 |    1 |    0 |	return
end
```
