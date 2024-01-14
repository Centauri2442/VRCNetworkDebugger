INFO -------------------------------------------------------------

The VRC Network Debugger is a easy to use tool designed for network testing in VRChat!
All needed listeners and data propagation happens on build, so there is almost no setup required!

Features -------------------------------------------------------------

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

SETUP -------------------------------------------------------------

1) Import dependencies
2) Import latest VRCNetworkDebugger Unitypackage
3) Drag and drop the "Network Debugger" prefab into your scene (Make sure to only have 1!)

CHANGELOG -------------------------------------------------------------

v1.0
- Initial Release

IMPORTANT NOTICE FOR ADVANCED USERS -------------------------------------------------------------

All relevant listeners and data propagation is generated on build! This means that if you have your own scripts also generating behaviours on build, you may have to modify the [PostProcessScene] value in the NetworkDebuggerBuildCallback.cs file found in the Editor folder!

LICENSING -------------------------------------------------------------

Copyright 2024 CentauriCore

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.