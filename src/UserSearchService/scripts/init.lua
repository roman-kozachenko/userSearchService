local log = require('log')
local fun = require('fun')
require('common')
box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}

function create_users_space()
   --[[
    channelId
    userId,
    text
    textIndex,
    fullName
    --]]
  log.info("Creating users space...")
  users = box.schema.space.create('users', { field_count = 5, format = {
     [1] = {["name"] = "channelId"},
     [2] = {["name"] = "userId"},
     [3] = {["name"] = "text"},
     [4] = {["name"] = "textIndex"},
     [5] = {["name"] = "fullName"}
    }})

  log.info(users.name .. ' space was created.')

  primary = users:create_index('primary', {unique=true, type='HASH', parts={1, 'UNSIGNED', 2,'UNSIGNED', 4, 'UNSIGNED'}})
  log.info(primary.name .. ' index was created.')

  channelId_userId = users:create_index('channelId_userId', {unique=true, type='HASH', parts={1, 'UNSIGNED', 2,'UNSIGNED'}})
  log.info(channelId_userId.name .. ' index was created.')

  -- TODO add channelId to index
  channelId_text = users:create_index('channelId_text', {type='TREE', unique=false, parts={3, 'STR'}})
  log.info(channelId_text.name .. ' index was created.')
end

function replace_user(channelId, userId, userFullName)
 local users = box.space.users;
  local nameParts = split(userFullName, " ")
  for i, part in ipairs(nameParts) do 
    local userNamePart = [channelId, userId, part, i, userFullName]
    users.replace(userNamePart)
  end
end

function remove_user(channelId, userId)
  local index = box.space.users.index.channelId_userId
  local matchingTuples = index:select({channelId, userId})

  for _, tuple in ipairs(matchingTuples) do
        drop_tuple('users', tuple)
    end
end

function search_users(channelId, query, skip, take)
  local index = box.space.users.index.channelId_text
  local matchingTuples = index:select({channelId, query}, {iterator = box.index.GE, limit = take, offset = skip })
  -- Check that text starts from 'query' string
  local queryMatched = fun.filter(function(t) return string.starts(t[3], query) end, matchingTuples)
  local result = fun.map(function(t) return {t[2], t[5], t[4]} end, queryMatched)

  return result
end

box.once('create_users_space', create_users_space)