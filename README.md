# VRCNetworkDebugger
#### Created as an alternative to the VRChat networking panels, VRC Network Debugger allows developers to quickly and easily see network usage and other relevant info about the networked udon behaviours in their world!
---
## Features
- Master Display
- Total KBytes sent out per second for all owned objects
- Network Clogged State
- 20 Latest network serializations sent out
- Full List of networked objects
  - Sorts by highest bytes per second
  - Networked object search function
  - Can show both BPS and total bytes sent over entire session
  - Shows time since last serialization for both local and remote users
  - Can toggle between showing object and owner name for remote objects
  - Shows send and receive time of network data sent from remote objects
  - Shows when a serialization has failed

- Wing Panels with extra info
    - Left Panel
        - Shows total serializations per second
        - Shows KBytes out, excluding Udon headers
        - Shows Udon headers in KB
        - Shows the min and max KBytes out, including Udon headers
    - Right Panel
        - Shows both total bytes and bytes per second
        - Displays whether or not the behaviour contains arrays and/or strings (Relevant for header calculations)
        - Shows the min and max bytes out for the synced data on the behaviour, with max being with headers
        - Displays time since last serialization
        - Shows the bytes per second of the Udon header data
---
## Setup

1) Import dependencies
2) Import latest VRCNetworkDebugger Unitypackage
3) Drag and drop the "Network Debugger" prefab into your scene (Make sure to only have 1!)
---
## Limitations
- Due to performance limitations, it is highly recommended to remove the prefab from the scene when not actively doing network testing! It is also not recommended to turn up the iterations per frame inside the debugger.

---
## Requirements
- VRCSDK3 - Latest
- TextMeshPro (Should automatically import)

---
## IMPORTANT NOTICE FOR ADVANCED USERS

All relevant listeners and data propagations are generated on build! If you have your own scripts also generating behaviours on build, you may have to modify the [PostProcessScene] value in the NetworkDebuggerBuildCallback.cs file found in the Editor folder!

---
![image_004_0000](https://github.com/Centauri2442/VRCNetworkDebugger/assets/28989460/d289e596-5b99-427e-9f7d-29af99791e20)
---
Copyright 2024 CentauriCore

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
