INFO -------------------------------------------------------------

The VRC Network Debugger is a easy to use tool designed for network testing in VRChat!
All needed listeners and data propagation happens on build, so there is almost no setup required!

SETUP -------------------------------------------------------------

1) Import dependencies
2) Import latest VRCNetworkDebugger Unitypackage
3) Drag and drop the "Network Debugger" prefab into your scene (Make sure to only have 1!)

CHANGELOG -------------------------------------------------------------

v1.1
- Added wing panels with extra info
    Left Panel
        - Shows total serializations per second
        - Shows KBytes out, excluding Udon headers
        - Shows Udon headers in KB
        - Shows the min and max KBytes out, including Udon headers
    Right Panel
        - Shows both total bytes and bytes per second
        - Displays whether or not the behaviour contains arrays and/or strings (Relevant for header calculations)
        - Shows the min and max bytes out for the synced data on the behaviour, with max being with headers
        - Displays time since last serialization
        - Shows the bytes per second of the Udon header data
        
- Fixed ownership issues, causing both ownership spam on start and master display showing wrong name

v1.0
- Initial Release

IMPORTANT NOTICE FOR ADVANCED USERS -------------------------------------------------------------

All relevant listeners and data propagation is generated on build! This means that if you have your own scripts also generating behaviours on build, you may have to modify the [PostProcessScene] value in the NetworkDebuggerBuildCallback.cs file found in the Editor folder!

LICENSING -------------------------------------------------------------

Copyright 2024 CentauriCore

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.