# VRCNetworkDebugger
#### Created as an alternative to the broken VRChat networking panels, VRC Network Debugger allows developers to quickly and easily see network usage and other relevant info about the networked udon behaviours in their world!
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

All relevant listeners and data propagationa are generated on build! If you have your own scripts also generating behaviours on build, you may have to modify the [PostProcessScene] value in the NetworkDebuggerBuildCallback.cs file found in the Editor folder!

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
