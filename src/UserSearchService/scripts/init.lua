local log = require('log')
local common = require('common')

box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}

local max_letters_count = 10
local min_letter_count = 3

local get_partial_names_space_name = function(letters_count)
  return "partial_names_" .. letters_count
end

local create_partial_names_space = function(letters_count)
   --[[
    channelId,
    text,
    userId,
    textIndeces
    --]]
    local space_name = get_partial_names_space_name(letters_count)

    log.info("Creating " .. space_name .. " space...")
    space = box.schema.space.create(space_name, { field_count = 4, format = {
      [1] = {["name"] = "channelId"},
      [3] = {["name"] = "text"},
      [2] = {["name"] = "userId"},
      [4] = {["name"] = "textIndeces"}
    }})

    log.info(space.name .. ' space was created.')

    primary = space:create_index('primary', {unique=true, type='TREE', parts={1, 'UNSIGNED', 2, 'STR', 3, 'UNSIGNED'}})
    log.info(primary.name .. ' index was created.')

   userId_channelId = space:create_index('userId_channelId', {unique=false, type='TREE', parts={3, 'UNSIGNED', 1,'UNSIGNED'}})
   log.info(userId_channelId.name .. ' index was created.')
end

local create_full_names_space = function()
   --[[
    channelId
    text,
    userId,
    textIndex
    --]]

  local space_name = "full_names"

  log.info("Creating " .. space_name .. " space...")
  space = box.schema.space.create(space_name, { field_count = 4, format = {
     [1] = {["name"] = "channelId"},
     [2] = {["name"] = "text"},
     [3] = {["name"] = "userId"},
     [4] = {["name"] = "textIndex"}
    }})

  log.info(space.name .. ' space was created.')

  primary = space:create_index('primary', {unique=true, type='TREE', parts={1, 'UNSIGNED', 3, 'UNSIGNED', 4, 'UNSIGNED'}})
  log.info(primary.name .. ' index was created.')

  channelId_text = space:create_index('channelId_text_userId', {unique=false, type='TREE', parts={1, 'UNSIGNED', 2, 'STR', 3, 'UNSIGNED'}})
  log.info(channelId_text.name .. ' index was created.')

  userId_channelId = space:create_index('userId_channelId', {unique=false, type='TREE', parts={3, 'UNSIGNED', 1,'UNSIGNED'}})
  log.info(userId_channelId.name .. ' index was created.')
end


local create_user_names_space = function()
   --[[
    userId,
    fullName
    --]]

  local space_name = "user_names"

  log.info("Creating " .. space_name .. " space...")
  space = box.schema.space.create(space_name, { field_count = 2, format = {
     [1] = {["name"] = "userId"},
     [2] = {["name"] = "fullName"}
    }})

  log.info(space.name .. ' space was created.')

  primary = space:create_index('primary', {unique=true, type='HASH', parts={1, 'UNSIGNED'}})
  log.info(primary.name .. ' index was created.')
end

local create_spaces = function()
  box.once('create_user_names_space', create_user_names_space)
  box.once('create_full_names_space', create_full_names_space)

  for i=min_letter_count, max_letters_count do
    local f = function()
      create_partial_names_space(i)
    end
    box.once('create_partial_names_space_' .. i, f)
  end
end

local remove_records = function(channelId, userId, space_name)
  log.info(string.format('Removing records for userId=%u, channelId=%u from %q',userId, channelId, space_name))

  local index = box.space[space_name].index.userId_channelId
  local matchingTuples = index:select({userId, channelId})

  for _, tuple in ipairs(matchingTuples) do
        common.drop_tuple(space_name, tuple)
  end
  log.info(string.format('%u records removed',#matchingTuples ))
end

local insert_full_name_if_needed = function(channelId, userId, text, textIndex)
  local textLength = string.len(text)

  if textLength > max_letters_count then
    box.space.full_names:insert({channelId, text, userId, textIndex})
  end
end

local initialize_letter_combinations = function()
  local combinations = {}
  for i=min_letter_count, max_letters_count do
    combinations[i]={}
  end

  return combinations
end

local add_letter_combinations = function(combinations, text, textIndex)
  for i=min_letter_count, math.min(string.len(text), max_letters_count) do
    local namePart = string.sub(text, 1, i)
    if combinations[i][namePart] == null then
      combinations[i][namePart] = {}
    end

    table.insert(combinations[i][namePart], textIndex)
  end
end

local search_full_names = function(channelId, query, queryLength, skip, take)
  local pre_result = {}
  local index_search_key = query
  local index = box.space.full_names.index.channelId_text_userId
  local number_of_tuples_in_result_set = 0
  local previous_tuple_field = ""
  local text_field_no = 2
  while true do
    local number_of_tuples_since_last_yield = 0
    local is_time_for_a_yield = false
    -- SEE NOTE #4 "INNER LOOP: ITERATOR"
    for _, tuple in index:pairs({channelId, index_search_key},{iterator = box.index.GE}) do

      -- SEE NOTE #5 "INNER LOOP: BREAK IF INDEX KEY IS TOO GREAT"
      if string.sub(tuple[text_field_no], 1, queryLength) > index_search_key then
        break
      end

      -- SEE NOTE #6 "INNER LOOP: BREAK AFTER EVERY 10 TUPLES -- MAYBE"
      number_of_tuples_since_last_yield = number_of_tuples_since_last_yield + 1
      if (number_of_tuples_since_last_yield >= 10
          and tuple[text_field_no] ~= previous_tuple_field) then
        index_search_key = tuple[text_field_no]
        is_time_for_a_yield = true
        break
      end

      previous_tuple_field = tuple[text_field_no]
      -- SEE NOTE #7 "INNER LOOP: ADD TO RESULT SET IF PATTERN MATCHES"
      if string.sub(tuple[text_field_no],1, queryLength) == query then
        number_of_tuples_in_result_set = number_of_tuples_in_result_set + 1
        pre_result[number_of_tuples_in_result_set] = tuple
      end
    end
    -- SEE NOTE #8 "OUTER LOOP: BREAK, OR YIELD AND CONTINUE"
    if (is_time_for_a_yield ~= true) then
      break
    end
    require('fiber').yield()
  end

  local grouped_results = {}
  
  for i, t in ipairs(pre_result) do
    if (i < skip + 1) or (i > skip + take) then
      break
    end
    local userId = t[3]
    if grouped_results[userId] == null then
      local user = box.space.user_names:get{userId}
      grouped_results[userId] = {userId, user[2], {}}
    end

    table.insert(grouped_results[userId][3], t[4])
  end

  local result = {}
  for userId, t in pairs(grouped_results) do
    table.insert(result, t)
  end

  return result
end

function search_names(channelId, query, skip, take, queryLength)
  local space = box.space[get_partial_names_space_name(queryLength)]
  local index = space.index.primary

  local matchingTuples = index:select({channelId, query}, {limit = take, offset = skip})
  
  local result = {}
  for i, tuple in ipairs(matchingTuples) do
    local userId = tuple[3]
    local user = box.space.user_names:get{userId}
    local fullName = user[2]
    table.insert(result, {userId, fullName, tuple[4]})
  end
  return result
end

function replace_user(channelId, userId, userFullName)
  log.info(string.format('Replacing user %q with userId=%u in channelId=%u', userFullName, userId, channelId))

  remove_user(channelId, userId)

  box.space.user_names:replace({userId, userFullName})

  local nameParts = common.split_string(userFullName, ' ')
  local letter_combinations = initialize_letter_combinations()
  for textIndex, text in ipairs(nameParts) do
    insert_full_name_if_needed(channelId, userId, text, textIndex)
    add_letter_combinations(letter_combinations, text, textIndex)
  end

  for i=min_letter_count, max_letters_count do
    local textIndeces = letter_combinations[i]
    if textIndeces ~= {} then
      for k, v in pairs(textIndeces) do
        box.space[get_partial_names_space_name(i)]:insert({channelId, k, userId, v})
      end
    end
  end
end

function remove_user(channelId, userId)
  remove_records(channelId, userId, 'full_names')

  for i=min_letter_count, max_letters_count do
    remove_records(channelId, userId, get_partial_names_space_name(i))
  end

  local remained_records_count = box.space.full_names.index.userId_channelId:count{userId}
  if remained_records_count == 0 then
    log.info('User with id=%u is not used anymore, removing...', userId)
    box.space.user_names:delete({userId})
  end
end

function search_users(channelId, query, skip, take)
  log.info(string.format('Looking for %u, %q, %u, %u', channelId, query, skip, take))
  local queryLength = string.len(query)

  if queryLength < min_letter_count then
    return {}
  end

  if queryLength > max_letters_count  then
    return search_full_names(channelId, query, queryLength, skip, take)
  end

  return search_names(channelId, query, skip, take, queryLength)
end

create_spaces()
replace_user(0, 1, "Caleb Cadman Caleb Corwin Bartholomew Chandler")
replace_user(0, 2, "Caleb Cadman Corwin Bartholomew Chandler")
replace_user(0, 3, "Bartholomew Cadman Caleb Corwin  Chandler")
replace_user(0, 4, "Bartholomew Bartholomew Caleb Corwin  Chandler")
replace_user(0, 5, "Bartholomew Cadman Bartholomew Corwin  Chandler")
replace_user(0, 6, "Bartholomew Cadman Caleb Bartholomew  Chandler")
replace_user(0, 7, "Bartholomew Cadman Caleb Corwin  Bartholomew")
replace_user(0, 8, "Bartholomew Cadman Caleb Corwin  Chandler")

log.info(table.tostring(search_users(0, "Cal", 0, 10)))
log.info(table.tostring(search_users(0, "Cal", 0, 1)))
log.info(table.tostring(search_users(0, "Cal", 1, 1)))
log.info(table.tostring(search_users(0, "Cal", 2, 1)))

log.info(table.tostring(search_users(0, "Bartholomew", 0, 10)))
