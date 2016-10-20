local log = require('log')
local common = require('common')

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

  channelId_userId = users:create_index('channelId_userId', {unique=false, type='TREE', parts={1, 'UNSIGNED', 2,'UNSIGNED'}})
  log.info(channelId_userId.name .. ' index was created.')

  channelId_text = users:create_index('channelId_text', {type='TREE', unique=false, parts={1,'UNSIGNED', 3, 'STR'}})
  log.info(channelId_text.name .. ' index was created.')
end

function replace_user(channelId, userId, userFullName)
  local users = box.space.users;
  log.info('Replacing user ' .. userFullName .. ' with id=' .. userId .. ' in channel with id=' .. channelId)
  local nameParts = common.split_string(userFullName, ' ')
  for i, part in ipairs(nameParts) do 
    local userNamePart = {channelId, userId, part, i, userFullName}
    users:replace(userNamePart)
  end
end

function remove_user(channelId, userId)
  local index = box.space.users.index.channelId_userId
  local matchingTuples = index:select({channelId, userId})

  for _, tuple in ipairs(matchingTuples) do
        common.drop_tuple('users', tuple)
    end
end

function search_users(channelId, query, skip, take)
  local index = box.space.users.index.channelId_text
  log.info(string.format('Looking for %u, %q, %u, %u', channelId, query, skip, take))
  local matchingTuples = index:select({channelId, query}, {iterator = box.index.GE, limit = take, offset = skip })
  local result = {}
  for i, t in ipairs(matchingTuples) do
    if common.start_with(t[3], query) then
      result[#result + 1] = {t[2], t[5], t[4]} 
    end
  end
  if #result == 0 then
    result = {0, 'Users not found!', 0}
  end
  return result
end

box.once('create_users_space', create_users_space)