# Distributed-World-Evolution
Public repository for our diploma project. It consists of server and clients able to process virtual world evolution.

## Branches
master (treated as "release")
*development*  
server  
client  
module-creator  

## Workflow
                                                   server <== PR
                                                  /
                                                ↙
                        master <-- development <- - client <== PR
                                                ↖
                                                  \
                                                   module-creator <== PR


## Directories
- Server -> everything about backend  
- Client -> everything about frontend  
- ModuleCreator -> Independent app for creating modules  