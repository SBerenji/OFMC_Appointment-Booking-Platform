# Example of PVS-Studio rules configuration file. 
# Full documentation is available at
# https://pvs-studio.com/en/docs/manual/full/
# https://pvs-studio.com/en/docs/manual/0040/
#
# Filtering out messages by specifying a fragment from source code:
# //-V:textFromSourceCode:3001,3002,3003
#
# Turning off specific analyzer rules:
# //-V::3021,3022
#
# Changing in analyzer's output message:
# //+V3022:RENAME:{oldText0:newText0},{oldText1:newText1}
#
# Appends message to analyzer's message:
# //+V3023:ADD:{Message}
#
# Excluding directories from the analysis:
# //V_EXCLUDE_PATH \thirdParty\
# //V_EXCLUDE_PATH C:\TheBestProject\thirdParty
# //V_EXCLUDE_PATH *\UE4\Engine\*
#
# Redefining levels:
# //V_LEVEL_1::501,502
# //V_LEVEL_2::522,783,579
# //V_LEVEL_3::773
#
# Disabling groups of diagnostics:
# //-V::GA
# //-V::GA,OWASP
#
# Disabling groups of diagnostics with specified warning levels:
# //-V::GA:3
# //-V::GA,OWASP:3
# //-V::GA,OWASP:2,3
#
# Disabling messages with specified warning levels:
# //-V::3002:3
# //-V::3002,3008:3
# //-V::3002,3008:2,3
# 
# Executing commands from the "CustomBuild" tasks before the analysis:
# //EXECUTE_CUSTOM_BUILD_COMMANDS
#
# Rule filters should be written without '#' character.