local log = require('log')

box.cfg
{
    pid_file = nil,
    background = false,
    log_level = 5
}


create_users_space = function ()
   --[[
    ballotId
    userId,
    text
    textIndex
    --]]
  log.info("Creating users space...")
  users = box.schema.space.create('users', { field_count = 5, format = {
     [1] = {["name"] = "ballotId"},
     [2] = {["name"] = "userId"},
     [3] = {["name"] = "text"},
     [4] = {["name"] = "textIndex"},
     [5] = {["name"] = "fullName"}
    }})

  log.info(users.name .. ' space was created.')

  primary = users:create_index('primary', {unique=true, type='HASH', parts={1, 'UNSIGNED', 2,'UNSIGNED', 4, 'UNSIGNED'}})
  log.info(primary.name .. ' index was created.')

  channelId_text = users:create_index('channelId_text', {type='TREE', unique=false, parts={1, 'UNSIGNED', 3, 'STR'}})
  log.info(channelId_text.name .. ' index was created.')
end

box.once('create_users_space', create_users_space)