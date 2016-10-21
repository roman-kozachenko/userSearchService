function drop_tuple(spaceName, tuple)
    local key = fun.map(
            function(x) return tuple[x.fieldno] end,
            box.space[spaceName].index[0].parts
        ):totable()
    box.space[spaceName]:delete(key)
end

function start_with(String,Start)
   return string.sub(String,1,string.len(Start))==Start
end

function split_string(str, delim, maxNb)
    -- Eliminate bad cases...
    if string.find(str, delim) == nil then
        return { str }
    end
    if maxNb == nil or maxNb < 1 then
        maxNb = 0    -- No limit
    end
    local result = {}
    local pat = "(.-)" .. delim .. "()"
    local nb = 0
    local lastPos
    for part, pos in string.gfind(str, pat) do
        nb = nb + 1
        result[nb] = part
        lastPos = pos
        if nb == maxNb then break end
    end
    -- Handle the last field
    if nb ~= maxNb then
        result[nb + 1] = string.sub(str, lastPos)
    end
    return result
end

return {
  drop_tuple = drop_tuple,
  start_with = start_with,
  split_string = split_string
}