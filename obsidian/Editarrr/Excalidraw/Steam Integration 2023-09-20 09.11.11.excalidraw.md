---

excalidraw-plugin: parsed
tags: [excalidraw]

---
==⚠  Switch to EXCALIDRAW VIEW in the MORE OPTIONS menu of this document. ⚠==


# Text Elements
Steam Integration Plan ^tHKrdLMp

Client ^HQc3H079

Local Storage ^NmyhukvL

Steam Local Storage ^mIlbfNvX

Steam Public Levels ^WYPec3iN

Steam Draft Levels ^WtaDdsyc

Steamworks ^y4jw1G6X

- Steam levels are subscribed/unsubscribed.
- This automatically downloads/removes files in the steam directory ^XcxmvWyC

- Gameplay and Editing ONLY 
affect files in LOCAL storage
- Any games downloaded from steam
must be copied from the STEAM
directory to LOCAL before they
can be played
- Placeholder levels can be created
while we wait for steam for UI
purposes. ^VL3yWYvN

- Draft levels are saved to steam as backups
- Draft levels are not public or searchable
 ^Zf9SMy2H

- When levels are published they will be marked as 
PUBLIC in steam and will be searchable. ^Xf9ics2m

States ^CwzoUDsj

My levels ^SbU8gdzO

Draft ^stEk0gTK

Published ^agcBVTg1

Other levels ^UGf7wqP6

Downloading ^NzmHgCAK

Ready ^uCjtJMbs

Updating ^mADTDDEY

Unplayable ^C3AANHdV

Unplayable ^cDCLL0Lc

Steam Level Data ^HJ9hh8wD

SteamId
SteamState ^Q4ff3bzB

internal enum ItemState
  {
    None = 0,
    Subscribed = 1,
    LegacyItem = 2,
    Installed = 4,
    NeedsUpdate = 8,
    Downloading = 16,
    DownloadPending = 32,
  } ^Y5TzCF7i

enum SteamState
{
  None = 0,
  Ready = 1,
  Downloading = 2,
} ^x6oOVPdg

Download Process ^OTbaxBBp

Query.Items ^gLYkhcLm

Level Browser ^VXEDU1Dv

Level Browser ^a8Gh2lhr

BrowserLevel (pending upload) ^FYu6dFam

BrowserLevel ^Ifh8w40r

BrowserLevel ^fISBhBbc

Download ^nYPnxWVt

BrowserLevel ^gLfS0Xdi

Item.GetAsync ^5ilTJVPx

Ugc.Item ^SdRwdMB7

- Create a local draft ^EqLaFbic

Subscribe() and 
DownloadAsync() ^AY7L2wDx

- Send progress updates to UI ^0Yiyu544

Download
Complete
Event ^5DFEANYc

Copy files from STEAM
to LOCAL ^wKyxyGok

Update local draft to local ready ^fIzLkx6n

Manual Download ^EIwLKrvn

Steam Subscription ^BvlM3ZsK

Search ^oK6f3Ani

Upload Process ^XDYTs1Lr

Level Selection ^mcubIcrl

Upload ^OqZwcDmU

isNew? ^HEH5pT8u

Yes ^q0llEPmh

No ^Ov3BpWPN

Editor.NewCommunityFile ^EWIUa50b

Item.GetAsync ^i0Wloaas

Item.Edit ^RXak331P

- Make changes ^qwunMpiK

Ugc.Editor ^FT0rAJ5K

- Send progress updates to UI ^0IQPQpRo

SubmitAsync() ^N8mq2Geb

- Create a draft search result to 
enhance level browser (tagged as pending)
- This can be removed when the first search 
returns a result for the uploaded level
 ^BlVKw4Sp

- Because of queries  ^lA9g2of0

Level Storage Architecture ^dZ99yE78

LevelPlayManager ^R8RPNIwm

LevelManager ^dOt9MPle

load ^BRtrEi6K

Level Editing ^8LZeNhei

Level Playing ^ZKJm7aAq

Level Browsing ^axTKRCen

Level Selection ^RjwU5yQ1

- See a list of levels
- Delete levels
- Publish
- Go to browse levels
- Go to leaderboard
- Go to play
- Go to edit ^Wn19ZoUq

- Create new levels
- Edit old levels
- Save and Play test
- Save and exit ^qt5x7Wqf

- Searching levels
- Staring levels
- Downloading levels
 ^PQ8HWgSh

- Go back to levels
- Go back to editing
- Submitting scores ^Bva8kykb

LevelManager ^AXYJFgnq

EditorLevelManager ^RIvbbOXY

create/
load/
save ^GztYoZXo

- create or load happens at start
- save is triggered by UI buttons ^PYDMiYnC

create ^70f3AtW9

load ^YOqfI4HL

save ^jEV82e12

LevelSelectionManager ^Q5sqt91K

LevelManager ^cev9qd73

loadAll/
delete ^rKwnMUbe

loadAll ^Tav1bY9s

delete ^4k4su5lV

LOCAL ^9JM0CEmH

REMOTE ^CMF1d6kK

- create only updates state
- no storage happens ^qaak78un

LOCAL ^FNwXlr2K

- We only care about local storage 
for loading.
- When deleting we only delete local
copies
- We never want to delete remote storage
so we have backups in case mistakes happen ^ovxk8LuF

load ^2sywwB2O

LOCAL ^ZYXxaffL

publish ^NqBV9F8d

- Publish should instantly update state
and return to level selection
- We can show a spinner while the upload
is happening in the background.
 ^6xdRekUM

- We probably cant leave the scene 
until the upload is done? ^gHf8xANK

submit score ^tBhWt0JA

submit ^9e1yz22K

REMOTE ^VOBrMJlS

LevelBrowseManager ^KZrUdAZZ

LevelManager ^xGhfipqI

loadAll ^1Nh9xxhw

loadAll ^TSWpnrCO

LOCAL ^DqMO0rfD

download ^zpe0E8nX

REMOTE ^XW3zKp1U

- When loading levels we want to get remote 
levels, but also show if we have downloaded it
already so we need to combine remote and local
state
- When downloading we want to instantly create
a local save stub with limited information which
can be updated with level data once it is downloaded
 ^PBTdJPXB

REMOTE ^64ecc5f0


# Embedded files
732937a81208c78c02abe60dcb84a81250376d13: [[Pasted Image 20230921112749_276.png]]
091d4d4eaeefcb84a8f20c73d6ab3d7ff8516663: [[Pasted Image 20230921112834_283.png]]
82a3cc7bb3b55fde0f8169bb6e43fcdceff8c3a1: [[Pasted Image 20230921113004_285.png]]
39935dd69c02dcc500df361f403d49a88d08bc84: [[Pasted Image 20230921113019_285.png]]
99ea706983684000aea34779dd857be12a73129b: [[Pasted Image 20230921113051_286.png]]
2b0acdc7924745614881e8638e0a7dd1438f665b: [[Pasted Image 20230921113336_289.png]]
61a8ddd77a4eef6d7abdf8d8faf8bb29682e07aa: [[Pasted Image 20230921113437_297.png]]
f23dd068c2fdd958ead25bb9644f0609bae1ce4b: [[Pasted Image 20230921113537_299.png]]

%%
# Drawing
```json
{
	"type": "excalidraw",
	"version": 2,
	"source": "https://github.com/zsviczian/obsidian-excalidraw-plugin/releases/tag/1.9.19",
	"elements": [
		{
			"type": "text",
			"version": 141,
			"versionNonce": 1413114637,
			"isDeleted": false,
			"id": "tHKrdLMp",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1541,
			"y": -312.6345486111111,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 417.7801513671875,
			"height": 45,
			"seed": 1928800030,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404503,
			"link": null,
			"locked": false,
			"fontSize": 36,
			"fontFamily": 1,
			"text": "Steam Integration Plan",
			"rawText": "Steam Integration Plan",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steam Integration Plan",
			"lineHeight": 1.25,
			"baseline": 32
		},
		{
			"type": "rectangle",
			"version": 210,
			"versionNonce": 1933531203,
			"isDeleted": false,
			"id": "pFZdzjjtUHJJzSqkX0m4p",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1212.25,
			"y": -97.63454861111103,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 615.0000000000001,
			"height": 438.75000000000006,
			"seed": 871758942,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 118,
			"versionNonce": 1697874285,
			"isDeleted": false,
			"id": "HQc3H079",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1462.25,
			"y": -150.13454861111103,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 75.74031066894531,
			"height": 35,
			"seed": 1279672578,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404503,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Client",
			"rawText": "Client",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Client",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 231,
			"versionNonce": 1523668963,
			"isDeleted": false,
			"id": "PXfHvvbKErIT2RG4TV1sT",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1257.25,
			"y": -26.384548611110972,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 472.5000000000001,
			"height": 117.49999999999999,
			"seed": 619305630,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "hZL8MXQBd24waKEeuZCug",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 127,
			"versionNonce": 2004536269,
			"isDeleted": false,
			"id": "NmyhukvL",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1268.5,
			"y": -63.88454861111097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 139.1398468017578,
			"height": 25,
			"seed": 425288222,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404503,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Local Storage",
			"rawText": "Local Storage",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Local Storage",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 147,
			"versionNonce": 1799512963,
			"isDeleted": false,
			"id": "2OZ4cbVNu0U1x6_eE-MJJ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1283.5,
			"y": -1.3845486111109722,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 2059211074,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "WfEfJO7e6ZhwDDkhSXPSI",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 167,
			"versionNonce": 724159021,
			"isDeleted": false,
			"id": "5a45xtHx_lzyjBkcUaWCe",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1392.25,
			"y": -2.634548611110972,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1859615262,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "vB7fd6Qg96lAknqP8dqBL",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 167,
			"versionNonce": 1288764195,
			"isDeleted": false,
			"id": "nKck2kAUCTd6IyGyiSOeT",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1497.25,
			"y": -2.634548611110972,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 926810434,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "YwBARnmx3uq1Szp1xg3W0",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 163,
			"versionNonce": 1630141581,
			"isDeleted": false,
			"id": "hJH6yjGCjHC8-lx0kaAKo",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1604.75,
			"y": -1.3845486111109722,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 458032414,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "q-_6j5nyZ01C0Gy-cXckY",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 281,
			"versionNonce": 468426435,
			"isDeleted": false,
			"id": "PlEEyU5cGmnQ7XPUJxtgl",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1263.5,
			"y": 144.86545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 472.5000000000001,
			"height": 117.49999999999999,
			"seed": 2104815874,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "RGSBbl6JyBtoAJTzobTEk",
					"type": "arrow"
				}
			],
			"updated": 1695301404503,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 229,
			"versionNonce": 557114093,
			"isDeleted": false,
			"id": "mIlbfNvX",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1264.75,
			"y": 282.365451388889,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 209.53976440429688,
			"height": 25,
			"seed": 1500741826,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Steam Local Storage",
			"rawText": "Steam Local Storage",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steam Local Storage",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 197,
			"versionNonce": 1664142947,
			"isDeleted": false,
			"id": "13bvX5lpUx9NmASikDgRC",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1289.75,
			"y": 169.86545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 512366722,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "WfEfJO7e6ZhwDDkhSXPSI",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 217,
			"versionNonce": 1070857549,
			"isDeleted": false,
			"id": "gY5aATxEuG0CQdxfOscN6",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1398.5,
			"y": 168.61545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 2088220738,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "vB7fd6Qg96lAknqP8dqBL",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 217,
			"versionNonce": 1433340419,
			"isDeleted": false,
			"id": "Ubd9A2wfKVcgKHhOjFy8W",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1503.5,
			"y": 168.61545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1394302978,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "YwBARnmx3uq1Szp1xg3W0",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 213,
			"versionNonce": 1147194285,
			"isDeleted": false,
			"id": "MVYR_z_cXRhSYBp0ULQR1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1611,
			"y": 169.86545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 779358146,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "q-_6j5nyZ01C0Gy-cXckY",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 273,
			"versionNonce": 1201983907,
			"isDeleted": false,
			"id": "q-_6j5nyZ01C0Gy-cXckY",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1641,
			"y": 159.86545138888903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 2.5,
			"height": 78.75,
			"seed": 359017538,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "MVYR_z_cXRhSYBp0ULQR1",
				"focus": -0.33893062306672556,
				"gap": 10
			},
			"endBinding": {
				"elementId": "hJH6yjGCjHC8-lx0kaAKo",
				"focus": 0.07865665046398586,
				"gap": 10
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					2.5,
					-78.75
				]
			]
		},
		{
			"type": "arrow",
			"version": 287,
			"versionNonce": 251973133,
			"isDeleted": false,
			"id": "YwBARnmx3uq1Szp1xg3W0",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1543.9631375689737,
			"y": 158.8338487349302,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 2.5,
			"height": 78.75,
			"seed": 1052798466,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "Ubd9A2wfKVcgKHhOjFy8W",
				"focus": -0.10574831231553657,
				"gap": 9.781602653958828
			},
			"endBinding": {
				"elementId": "nKck2kAUCTd6IyGyiSOeT",
				"focus": -0.15452566028720313,
				"gap": 10.218397346041172
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					2.5,
					-78.75
				]
			]
		},
		{
			"type": "arrow",
			"version": 149,
			"versionNonce": 1795204419,
			"isDeleted": false,
			"id": "vB7fd6Qg96lAknqP8dqBL",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1452.1236911319197,
			"y": 156.88508028098295,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 2.5,
			"height": 78.75,
			"seed": 1439649694,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "gY5aATxEuG0CQdxfOscN6",
				"focus": 0.18597677387366493,
				"gap": 11.73037110790608
			},
			"endBinding": {
				"elementId": "5a45xtHx_lzyjBkcUaWCe",
				"focus": -0.44625074647640467,
				"gap": 8.26962889209392
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					2.5,
					-78.75
				]
			]
		},
		{
			"type": "arrow",
			"version": 149,
			"versionNonce": 1052345453,
			"isDeleted": false,
			"id": "WfEfJO7e6ZhwDDkhSXPSI",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1333.3736911319197,
			"y": 166.88508028098295,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 2.5,
			"height": 78.75,
			"seed": 992188610,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "13bvX5lpUx9NmASikDgRC",
				"focus": -0.03054996054966689,
				"gap": 2.9803711079060804
			},
			"endBinding": {
				"elementId": "2OZ4cbVNu0U1x6_eE-MJJ",
				"focus": -0.22972401205307283,
				"gap": 17.01962889209392
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					2.5,
					-78.75
				]
			]
		},
		{
			"type": "rectangle",
			"version": 167,
			"versionNonce": 1189151971,
			"isDeleted": false,
			"id": "CldTEIchDAPHQZwlv6q3t",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1946.8100233100233,
			"y": -85.52790525446784,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 532.7272727272732,
			"height": 432.7272727272731,
			"seed": 318885954,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 301,
			"versionNonce": 1464364749,
			"isDeleted": false,
			"id": "2UWfzuyaRFzt6VnXxsKa5",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1977.832750582751,
			"y": 164.13118565462338,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 472.5000000000001,
			"height": 117.49999999999999,
			"seed": 1170458334,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "E__jZma76bW8WrXAdMkBt",
					"type": "arrow"
				},
				{
					"id": "RGSBbl6JyBtoAJTzobTEk",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 260,
			"versionNonce": 1926525059,
			"isDeleted": false,
			"id": "WYPec3iN",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1979.082750582751,
			"y": 301.63118565462344,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 195.25978088378906,
			"height": 25,
			"seed": 363019038,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Steam Public Levels",
			"rawText": "Steam Public Levels",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steam Public Levels",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 215,
			"versionNonce": 659229997,
			"isDeleted": false,
			"id": "lY48qC3qZ4tjTszEscWD1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2004.082750582751,
			"y": 189.13118565462344,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1510614878,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 235,
			"versionNonce": 278489123,
			"isDeleted": false,
			"id": "xbFTkpxueZ9oGVOrSybCh",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2112.832750582751,
			"y": 187.88118565462344,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 55151518,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 236,
			"versionNonce": 1848303501,
			"isDeleted": false,
			"id": "Lt3aMlIy9nmVEay0YYsFc",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2217.832750582751,
			"y": 187.88118565462344,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1334994910,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 232,
			"versionNonce": 102656963,
			"isDeleted": false,
			"id": "_4-rrNCrCGDjGZ1wGqhwx",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2325.332750582751,
			"y": 189.13118565462344,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 250985502,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 305,
			"versionNonce": 1109485037,
			"isDeleted": false,
			"id": "t1isJmJc_rNR_7eWwuGEo",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1970.5600233100236,
			"y": -41.32335979992229,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 472.5000000000001,
			"height": 117.49999999999999,
			"seed": 2085532098,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "E__jZma76bW8WrXAdMkBt",
					"type": "arrow"
				},
				{
					"id": "hZL8MXQBd24waKEeuZCug",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 296,
			"versionNonce": 2005007203,
			"isDeleted": false,
			"id": "WtaDdsyc",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1980.9009324009328,
			"y": -74.73245070901339,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 199.3997802734375,
			"height": 25,
			"seed": 14100866,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Steam Draft Levels",
			"rawText": "Steam Draft Levels",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steam Draft Levels",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 219,
			"versionNonce": 909647949,
			"isDeleted": false,
			"id": "2v0oX0G-YK1UZ-dmPqm8H",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1996.8100233100236,
			"y": -16.323359799922287,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 351971650,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 239,
			"versionNonce": 17995523,
			"isDeleted": false,
			"id": "8foOD6X7lGPaOfyvmRNti",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2105.5600233100236,
			"y": -17.573359799922287,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 538456322,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 240,
			"versionNonce": 1078496941,
			"isDeleted": false,
			"id": "Q5lXz1CzlYopaS0rM8FkN",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2210.5600233100236,
			"y": -17.573359799922287,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1169080514,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 236,
			"versionNonce": 827244195,
			"isDeleted": false,
			"id": "VRuw9CGhXqrPZC1OWcBrU",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2318.0600233100236,
			"y": -16.323359799922287,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.5,
			"height": 72.5,
			"seed": 1633507458,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 267,
			"versionNonce": 1593585933,
			"isDeleted": false,
			"id": "E__jZma76bW8WrXAdMkBt",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2195.9009324009326,
			"y": 94.47209474553227,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 3.6363636363637397,
			"height": 56.36363636363649,
			"seed": 2143144002,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "t1isJmJc_rNR_7eWwuGEo",
				"focus": 0.0661545744697093,
				"gap": 18.295454545454568
			},
			"endBinding": {
				"elementId": "2UWfzuyaRFzt6VnXxsKa5",
				"focus": -0.041232075500514,
				"gap": 13.295454545454618
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					3.6363636363637397,
					56.36363636363649
				]
			]
		},
		{
			"type": "arrow",
			"version": 281,
			"versionNonce": 1122300483,
			"isDeleted": false,
			"id": "hZL8MXQBd24waKEeuZCug",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1757.719114219114,
			"y": 23.56300383644134,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 192.72727272727275,
			"height": 9.090909090909065,
			"seed": 662110,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "PXfHvvbKErIT2RG4TV1sT",
				"focus": 0.05237538458391934,
				"gap": 27.96911421911409
			},
			"endBinding": {
				"elementId": "t1isJmJc_rNR_7eWwuGEo",
				"focus": 0.21528597941623037,
				"gap": 20.113636363636715
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					192.72727272727275,
					-9.090909090909065
				]
			]
		},
		{
			"type": "arrow",
			"version": 274,
			"versionNonce": 2144110445,
			"isDeleted": false,
			"id": "RGSBbl6JyBtoAJTzobTEk",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1957.7191142191143,
			"y": 229.017549290987,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 210.90909090909122,
			"height": 1.8181818181818699,
			"seed": 2018699650,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "2UWfzuyaRFzt6VnXxsKa5",
				"focus": -0.1373064092440097,
				"gap": 20.113636363636715
			},
			"endBinding": {
				"elementId": "PlEEyU5cGmnQ7XPUJxtgl",
				"focus": 0.3529408604749849,
				"gap": 10.810023310023098
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-210.90909090909122,
					-1.8181818181818699
				]
			]
		},
		{
			"type": "text",
			"version": 116,
			"versionNonce": 380875235,
			"isDeleted": false,
			"id": "y4jw1G6X",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1981.3554778554778,
			"y": -150.52790525446778,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 158.0046844482422,
			"height": 35,
			"seed": 1985014210,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Steamworks",
			"rawText": "Steamworks",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steamworks",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "text",
			"version": 291,
			"versionNonce": 620958157,
			"isDeleted": false,
			"id": "XcxmvWyC",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1661.3554778554771,
			"y": 394.6993674728053,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 539.7611083984375,
			"height": 40,
			"seed": 2024509278,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Steam levels are subscribed/unsubscribed.\n- This automatically downloads/removes files in the steam directory",
			"rawText": "- Steam levels are subscribed/unsubscribed.\n- This automatically downloads/removes files in the steam directory",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Steam levels are subscribed/unsubscribed.\n- This automatically downloads/removes files in the steam directory",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "text",
			"version": 395,
			"versionNonce": 1841921411,
			"isDeleted": false,
			"id": "VL3yWYvN",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 915.900932400931,
			"y": -61.891541618103986,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 285.12060546875,
			"height": 180,
			"seed": 1373177026,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Gameplay and Editing ONLY \naffect files in LOCAL storage\n- Any games downloaded from steam\nmust be copied from the STEAM\ndirectory to LOCAL before they\ncan be played\n- Placeholder levels can be created\nwhile we wait for steam for UI\npurposes.",
			"rawText": "- Gameplay and Editing ONLY \naffect files in LOCAL storage\n- Any games downloaded from steam\nmust be copied from the STEAM\ndirectory to LOCAL before they\ncan be played\n- Placeholder levels can be created\nwhile we wait for steam for UI\npurposes.",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Gameplay and Editing ONLY \naffect files in LOCAL storage\n- Any games downloaded from steam\nmust be copied from the STEAM\ndirectory to LOCAL before they\ncan be played\n- Placeholder levels can be created\nwhile we wait for steam for UI\npurposes.",
			"lineHeight": 1.25,
			"baseline": 174
		},
		{
			"type": "text",
			"version": 199,
			"versionNonce": 7908678,
			"isDeleted": false,
			"id": "Zf9SMy2H",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2555.9009324009317,
			"y": -45.52790525446818,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 377.12078857421875,
			"height": 60,
			"seed": 1528983646,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695319510896,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Draft levels are saved to steam as backups\n- Draft levels are not public or searchable\n",
			"rawText": "- Draft levels are saved to steam as backups\n- Draft levels are not public or searchable\n",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Draft levels are saved to steam as backups\n- Draft levels are not public or searchable\n",
			"lineHeight": 1.25,
			"baseline": 54
		},
		{
			"type": "text",
			"version": 225,
			"versionNonce": 1667552547,
			"isDeleted": false,
			"id": "Xf9ics2m",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2575.9009324009317,
			"y": 209.01754929098655,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 403.6168212890625,
			"height": 40,
			"seed": 292977666,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- When levels are published they will be marked as \nPUBLIC in steam and will be searchable.",
			"rawText": "- When levels are published they will be marked as \nPUBLIC in steam and will be searchable.",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- When levels are published they will be marked as \nPUBLIC in steam and will be searchable.",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "text",
			"version": 78,
			"versionNonce": 299418253,
			"isDeleted": false,
			"id": "CwzoUDsj",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1731.8263403263422,
			"y": 1400.608458381896,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 125.82005310058594,
			"height": 45,
			"seed": 802906654,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 36,
			"fontFamily": 1,
			"text": "States",
			"rawText": "States",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "States",
			"lineHeight": 1.25,
			"baseline": 32
		},
		{
			"type": "text",
			"version": 103,
			"versionNonce": 653539523,
			"isDeleted": false,
			"id": "SbU8gdzO",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1719.0990675990693,
			"y": 1492.6539129273506,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 123.78851318359375,
			"height": 35,
			"seed": 40784350,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "My levels",
			"rawText": "My levels",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "My levels",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 106,
			"versionNonce": 471525613,
			"isDeleted": false,
			"id": "Oa9GY769x6-iiD9uA7hFO",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1746.7808857808875,
			"y": 1578.24482201826,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 219,
			"height": 237,
			"seed": 1644411102,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "stEk0gTK"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 70,
			"versionNonce": 1981163619,
			"isDeleted": false,
			"id": "stEk0gTK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1666.5608540426063,
			"y": 1684.24482201826,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 58.5599365234375,
			"height": 25,
			"seed": 1246979934,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Draft",
			"rawText": "Draft",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "Oa9GY769x6-iiD9uA7hFO",
			"originalText": "Draft",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 130,
			"versionNonce": 1007240013,
			"isDeleted": false,
			"id": "aoRkKPvGiSW1sIRnEYAB_",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1477.6899766899783,
			"y": 1585.517549290987,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 219,
			"height": 237,
			"seed": 42172802,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "agcBVTg1"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 103,
			"versionNonce": 1452876803,
			"isDeleted": false,
			"id": "agcBVTg1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1411.9099245049197,
			"y": 1691.517549290987,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 87.43989562988281,
			"height": 25,
			"seed": 1331429698,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Published",
			"rawText": "Published",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "aoRkKPvGiSW1sIRnEYAB_",
			"originalText": "Published",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "text",
			"version": 132,
			"versionNonce": 1442664877,
			"isDeleted": false,
			"id": "UGf7wqP6",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1715.5387787363206,
			"y": 1888.335731109169,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 166.54470825195312,
			"height": 35,
			"seed": 314239902,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Other levels",
			"rawText": "Other levels",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Other levels",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 145,
			"versionNonce": 276540323,
			"isDeleted": false,
			"id": "oYSw7ukDDawCetUMmsAhm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1743.1445221445242,
			"y": 1985.517549290988,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 219,
			"height": 237,
			"seed": 1683068766,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "NzmHgCAK"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 127,
			"versionNonce": 119575565,
			"isDeleted": false,
			"id": "NzmHgCAK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1690.1444687387625,
			"y": 2091.517549290988,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 112.99989318847656,
			"height": 25,
			"seed": 857930654,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Downloading",
			"rawText": "Downloading",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "oYSw7ukDDawCetUMmsAhm",
			"originalText": "Downloading",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 217,
			"versionNonce": 1743535939,
			"isDeleted": false,
			"id": "EHCBZdM_qXUTQn-d-uLHS",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1219.5081585081602,
			"y": 1983.6993674728055,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 219,
			"height": 237,
			"seed": 582866910,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "uCjtJMbs"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 205,
			"versionNonce": 191662701,
			"isDeleted": false,
			"id": "uCjtJMbs",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1139.3081234129454,
			"y": 2089.6993674728055,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 58.59992980957031,
			"height": 25,
			"seed": 1261644830,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Ready",
			"rawText": "Ready",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "EHCBZdM_qXUTQn-d-uLHS",
			"originalText": "Ready",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 175,
			"versionNonce": 390684387,
			"isDeleted": false,
			"id": "W7-TYwV86tTaIdZSWiKeN",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1484.9627039627057,
			"y": 1983.6993674728055,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 219,
			"height": 237,
			"seed": 1388917634,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "mADTDDEY"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 154,
			"versionNonce": 1672131789,
			"isDeleted": false,
			"id": "mADTDDEY",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1417.4126551345807,
			"y": 2089.6993674728055,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 83.89990234375,
			"height": 25,
			"seed": 807105346,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Updating",
			"rawText": "Updating",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "W7-TYwV86tTaIdZSWiKeN",
			"originalText": "Updating",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "text",
			"version": 107,
			"versionNonce": 153193091,
			"isDeleted": false,
			"id": "C3AANHdV",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1684.5536130536152,
			"y": 2160.381185654624,
			"strokeColor": "#e03131",
			"backgroundColor": "transparent",
			"width": 101.15988159179688,
			"height": 25,
			"seed": 511873630,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Unplayable",
			"rawText": "Unplayable",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Unplayable",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "text",
			"version": 114,
			"versionNonce": 997495597,
			"isDeleted": false,
			"id": "cDCLL0Lc",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1418.769917485877,
			"y": 2166.0630038364425,
			"strokeColor": "#e03131",
			"backgroundColor": "transparent",
			"width": 101.15988159179688,
			"height": 25,
			"seed": 386670174,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Unplayable",
			"rawText": "Unplayable",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Unplayable",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 83,
			"versionNonce": 1379090979,
			"isDeleted": false,
			"id": "MgVysZllMC0ocbwBf_8Rj",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1593.1899766899794,
			"y": 3.5630038364410552,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 456.3636363636367,
			"height": 600.0000000000005,
			"seed": 2080619138,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 80,
			"versionNonce": 255042957,
			"isDeleted": false,
			"id": "HJ9hh8wD",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1569.553613053616,
			"y": -61.436996163559,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 256.9850769042969,
			"height": 35,
			"seed": 1306571970,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Steam Level Data",
			"rawText": "Steam Level Data",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Steam Level Data",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "text",
			"version": 63,
			"versionNonce": 1802307011,
			"isDeleted": false,
			"id": "Q4ff3bzB",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1544.0990675990702,
			"y": 66.74482201825924,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 119.43986511230469,
			"height": 50,
			"seed": 706314654,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "SteamId\nSteamState",
			"rawText": "SteamId\nSteamState",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "SteamId\nSteamState",
			"lineHeight": 1.25,
			"baseline": 43
		},
		{
			"type": "text",
			"version": 68,
			"versionNonce": 304180205,
			"isDeleted": false,
			"id": "Y5TzCF7i",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1587.7354312354335,
			"y": 701.7448220182595,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 263.2197265625,
			"height": 250,
			"seed": 206942594,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "internal enum ItemState\n  {\n    None = 0,\n    Subscribed = 1,\n    LegacyItem = 2,\n    Installed = 4,\n    NeedsUpdate = 8,\n    Downloading = 16,\n    DownloadPending = 32,\n  }",
			"rawText": "internal enum ItemState\n  {\n    None = 0,\n    Subscribed = 1,\n    LegacyItem = 2,\n    Installed = 4,\n    NeedsUpdate = 8,\n    Downloading = 16,\n    DownloadPending = 32,\n  }",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "internal enum ItemState\n  {\n    None = 0,\n    Subscribed = 1,\n    LegacyItem = 2,\n    Installed = 4,\n    NeedsUpdate = 8,\n    Downloading = 16,\n    DownloadPending = 32,\n  }",
			"lineHeight": 1.25,
			"baseline": 243
		},
		{
			"type": "text",
			"version": 195,
			"versionNonce": 736974179,
			"isDeleted": false,
			"id": "x6oOVPdg",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -1816.8263403263416,
			"y": 16.290276563713974,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 184.7198028564453,
			"height": 150,
			"seed": 1884524290,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "enum SteamState\n{\n  None = 0,\n  Ready = 1,\n  Downloading = 2,\n}",
			"rawText": "enum SteamState\n{\n  None = 0,\n  Ready = 1,\n  Downloading = 2,\n}",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "enum SteamState\n{\n  None = 0,\n  Ready = 1,\n  Downloading = 2,\n}",
			"lineHeight": 1.25,
			"baseline": 143
		},
		{
			"type": "text",
			"version": 83,
			"versionNonce": 1413965389,
			"isDeleted": false,
			"id": "OTbaxBBp",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1627.4413364413363,
			"y": 647.2498725233102,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314.74810791015625,
			"height": 45,
			"seed": 1492888130,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 36,
			"fontFamily": 1,
			"text": "Download Process",
			"rawText": "Download Process",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Download Process",
			"lineHeight": 1.25,
			"baseline": 32
		},
		{
			"type": "rectangle",
			"version": 246,
			"versionNonce": 50508035,
			"isDeleted": false,
			"id": "NcB1FPJVmuLM298tAWP_m",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1692.6614219114217,
			"y": 912.8182485916864,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 148,
			"height": 63,
			"seed": 1926412254,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "gLYkhcLm"
				},
				{
					"id": "ngQYG3tFkynxUPSiksqH8",
					"type": "arrow"
				},
				{
					"id": "9_a5934RRPJcx0DwmFIYg",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 228,
			"versionNonce": 679417005,
			"isDeleted": false,
			"id": "gLYkhcLm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1708.4014808103475,
			"y": 931.8182485916864,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 116.51988220214844,
			"height": 25,
			"seed": 1542799234,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Query.Items",
			"rawText": "Query.Items",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "NcB1FPJVmuLM298tAWP_m",
			"originalText": "Query.Items",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 69,
			"versionNonce": 1915459747,
			"isDeleted": false,
			"id": "eLILYTQGD4Wycbp7Pp72q",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1097.3344988344986,
			"y": 807.7413255147633,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 463.0769230769233,
			"height": 304.615384615385,
			"seed": 1685475458,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "ngQYG3tFkynxUPSiksqH8",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 72,
			"versionNonce": 1248982797,
			"isDeleted": false,
			"id": "VXEDU1Dv",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1238.8729603729603,
			"y": 829.2797870532249,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 190.9328155517578,
			"height": 35,
			"seed": 634220034,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Browser",
			"rawText": "Level Browser",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Level Browser",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 120,
			"versionNonce": 1192660035,
			"isDeleted": false,
			"id": "5ub0cXCX7LfY2mq-vuwdl",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1936.872960372961,
			"y": 805.5874793609169,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 463.0769230769233,
			"height": 304.615384615385,
			"seed": 555521758,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "9_a5934RRPJcx0DwmFIYg",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 123,
			"versionNonce": 1150913901,
			"isDeleted": false,
			"id": "a8Gh2lhr",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2078.4114219114226,
			"y": 827.1259408993787,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 190.9328155517578,
			"height": 35,
			"seed": 761400094,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Browser",
			"rawText": "Level Browser",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Level Browser",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 71,
			"versionNonce": 362941411,
			"isDeleted": false,
			"id": "x5P7yeHR_EccWVqgrZydH",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1953.9114219114226,
			"y": 883.8951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 427,
			"height": 48,
			"seed": 678605982,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "FYu6dFam"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 76,
			"versionNonce": 373249997,
			"isDeleted": false,
			"id": "FYu6dFam",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2022.9215842649382,
			"y": 895.3951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 288.97967529296875,
			"height": 25,
			"seed": 532462274,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "BrowserLevel (pending upload)",
			"rawText": "BrowserLevel (pending upload)",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "x5P7yeHR_EccWVqgrZydH",
			"originalText": "BrowserLevel (pending upload)",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 92,
			"versionNonce": 1910282115,
			"isDeleted": false,
			"id": "4vx6prtSUYZlnefiBOqzg",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1954.911421911423,
			"y": 947.8951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 427,
			"height": 48,
			"seed": 1100737026,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "Ifh8w40r"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 29,
			"versionNonce": 427611693,
			"isDeleted": false,
			"id": "Ifh8w40r",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2105.221480505173,
			"y": 959.3951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 126.3798828125,
			"height": 25,
			"seed": 1672437698,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "BrowserLevel",
			"rawText": "BrowserLevel",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "4vx6prtSUYZlnefiBOqzg",
			"originalText": "BrowserLevel",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 100,
			"versionNonce": 1778691875,
			"isDeleted": false,
			"id": "-drpvTBTutPjbvlWR9a9p",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1956.911421911423,
			"y": 1015.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 427,
			"height": 48,
			"seed": 665485214,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "fISBhBbc"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 29,
			"versionNonce": 691767437,
			"isDeleted": false,
			"id": "fISBhBbc",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2107.221480505173,
			"y": 1027.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 126.3798828125,
			"height": 25,
			"seed": 560539202,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "BrowserLevel",
			"rawText": "BrowserLevel",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "-drpvTBTutPjbvlWR9a9p",
			"originalText": "BrowserLevel",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 379,
			"versionNonce": 340544966,
			"isDeleted": false,
			"id": "ngQYG3tFkynxUPSiksqH8",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1400.411421911422,
			"y": 1055.6961592635594,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 284.0000000000002,
			"height": 113.219736782057,
			"seed": 777833502,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510742,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "xvdmvJCMozZKHwGcYoDMq",
				"gap": 11.666666666667197,
				"focus": 0.4890794112163748
			},
			"endBinding": {
				"elementId": "NcB1FPJVmuLM298tAWP_m",
				"gap": 8.249999999999545,
				"focus": 0.5677242011685868
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					284.0000000000002,
					-113.219736782057
				]
			]
		},
		{
			"type": "arrow",
			"version": 202,
			"versionNonce": 684513926,
			"isDeleted": false,
			"id": "9_a5934RRPJcx0DwmFIYg",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1846.4114219114222,
			"y": 945.7978049003591,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 74.4615384615388,
			"height": 3.7007373313758762,
			"seed": 1857330306,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510731,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "NcB1FPJVmuLM298tAWP_m",
				"gap": 5.750000000000455,
				"focus": 0.15473196936792877
			},
			"endBinding": {
				"elementId": "5ub0cXCX7LfY2mq-vuwdl",
				"gap": 16,
				"focus": 0.17153945666235354
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					74.4615384615388,
					-3.7007373313758762
				]
			]
		},
		{
			"type": "rectangle",
			"version": 82,
			"versionNonce": 1711768163,
			"isDeleted": false,
			"id": "PPdCgN5IExJVmcIE2p1p_",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1066.411421911422,
			"y": 1297.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 532.0000000000005,
			"height": 76,
			"seed": 1872821442,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "GYV0H4Ns_tRIuEsG_nuUh",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "rectangle",
			"version": 61,
			"versionNonce": 32672077,
			"isDeleted": false,
			"id": "kU2nVIUgF10ES-QZNQTs3",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1449.9114219114222,
			"y": 1309.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 119,
			"height": 48,
			"seed": 296335170,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "nYPnxWVt"
				},
				{
					"id": "GYV0H4Ns_tRIuEsG_nuUh",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 36,
			"versionNonce": 860831235,
			"isDeleted": false,
			"id": "nYPnxWVt",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1464.7814628049769,
			"y": 1321.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 89.25991821289062,
			"height": 25,
			"seed": 29503298,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Download",
			"rawText": "Download",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "kU2nVIUgF10ES-QZNQTs3",
			"originalText": "Download",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "text",
			"version": 57,
			"versionNonce": 1523439533,
			"isDeleted": false,
			"id": "gLfS0Xdi",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1220.4114219114222,
			"y": 1327.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 126.3798828125,
			"height": 25,
			"seed": 99939614,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "BrowserLevel",
			"rawText": "BrowserLevel",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "BrowserLevel",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 378,
			"versionNonce": 348153734,
			"isDeleted": false,
			"id": "GYV0H4Ns_tRIuEsG_nuUh",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1580.4114219114226,
			"y": 1339.2887165560496,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 115.99999999999955,
			"height": 2.5078985223915424,
			"seed": 1179756894,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510736,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "kU2nVIUgF10ES-QZNQTs3",
				"gap": 11.500000000000455,
				"focus": 0.1525935380364423
			},
			"endBinding": {
				"elementId": "XL4fbdP6DENA6sNh3LcOu",
				"gap": 14.000000000000227,
				"focus": -0.2075697940799068
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					115.99999999999955,
					2.5078985223915424
				]
			]
		},
		{
			"type": "rectangle",
			"version": 289,
			"versionNonce": 1810913805,
			"isDeleted": false,
			"id": "XL4fbdP6DENA6sNh3LcOu",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1710.4114219114224,
			"y": 1304.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 180,
			"height": 65,
			"seed": 1089150338,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "5ilTJVPx"
				},
				{
					"id": "GYV0H4Ns_tRIuEsG_nuUh",
					"type": "arrow"
				},
				{
					"id": "rDDQe9nuXgjqstiAcIipa",
					"type": "arrow"
				}
			],
			"updated": 1695301404504,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 286,
			"versionNonce": 1298389315,
			"isDeleted": false,
			"id": "5ilTJVPx",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1729.3914863035122,
			"y": 1324.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 142.0398712158203,
			"height": 25,
			"seed": 376446274,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404504,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Item.GetAsync",
			"rawText": "Item.GetAsync",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "XL4fbdP6DENA6sNh3LcOu",
			"originalText": "Item.GetAsync",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 108,
			"versionNonce": 1083541190,
			"isDeleted": false,
			"id": "rDDQe9nuXgjqstiAcIipa",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1904.4114219114224,
			"y": 1339.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 132,
			"height": 0,
			"seed": 1277687234,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510736,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "XL4fbdP6DENA6sNh3LcOu",
				"gap": 14,
				"focus": 0.09230769230769231
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					132,
					0
				]
			]
		},
		{
			"type": "rectangle",
			"version": 60,
			"versionNonce": 1589491939,
			"isDeleted": false,
			"id": "3jq-cURn1mTLrY317J3dV",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2074.4114219114226,
			"y": 1257.8951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 330,
			"height": 204.00000000000023,
			"seed": 1456702814,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "lgVoppa8-J3YKRvVNSpTT",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 67,
			"versionNonce": 1069611725,
			"isDeleted": false,
			"id": "SdRwdMB7",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2184.4114219114226,
			"y": 1284.8951716686095,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 119.86851501464844,
			"height": 35,
			"seed": 570634654,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Ugc.Item",
			"rawText": "Ugc.Item",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Ugc.Item",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "text",
			"version": 87,
			"versionNonce": 1425712259,
			"isDeleted": false,
			"id": "EqLaFbic",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2148.4114219114226,
			"y": 1343.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 183.10440063476562,
			"height": 20,
			"seed": 405177474,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Create a local draft",
			"rawText": "- Create a local draft",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Create a local draft",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "arrow",
			"version": 136,
			"versionNonce": 147986733,
			"isDeleted": false,
			"id": "lgVoppa8-J3YKRvVNSpTT",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2422.4114219114226,
			"y": 1351.5238610919607,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 100,
			"height": 0.214791005945699,
			"seed": 1345555614,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "3jq-cURn1mTLrY317J3dV",
				"focus": -0.08562775684979439,
				"gap": 18
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					100,
					0.214791005945699
				]
			]
		},
		{
			"type": "rectangle",
			"version": 120,
			"versionNonce": 936975395,
			"isDeleted": false,
			"id": "6wreEa0dsN1kfhZyR99Up",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2556.4114219114226,
			"y": 1253.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 302,
			"height": 196.0000000000001,
			"seed": 544476226,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 81,
			"versionNonce": 586181517,
			"isDeleted": false,
			"id": "AY7L2wDx",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2646.4114219114226,
			"y": 1291.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 126.22427368164062,
			"height": 40,
			"seed": 811189442,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "Subscribe() and \nDownloadAsync()",
			"rawText": "Subscribe() and \nDownloadAsync()",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Subscribe() and \nDownloadAsync()",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "text",
			"version": 78,
			"versionNonce": 323461059,
			"isDeleted": false,
			"id": "0Yiyu544",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2588.4114219114226,
			"y": 1395.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 247.5685272216797,
			"height": 20,
			"seed": 1909006174,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Send progress updates to UI",
			"rawText": "- Send progress updates to UI",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Send progress updates to UI",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "diamond",
			"version": 86,
			"versionNonce": 1582658029,
			"isDeleted": false,
			"id": "P1yrpQcWrJEMp1ZhOexMk",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1403.9114219114222,
			"y": 1543.3951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 191,
			"height": 175,
			"seed": 431839966,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "5DFEANYc"
				},
				{
					"id": "FQEdt-xkIB8SwWa2cvqZ0",
					"type": "arrow"
				},
				{
					"id": "rN3lVqz9V14VXD3MWR_I3",
					"type": "arrow"
				},
				{
					"id": "2P-eMxVecmUkzM5fCcv0a",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 84,
			"versionNonce": 1902423907,
			"isDeleted": false,
			"id": "5DFEANYc",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1463.9573508665003,
			"y": 1601.1451716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 71.40814208984375,
			"height": 60,
			"seed": 1865704158,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "Download\nComplete\nEvent",
			"rawText": "Download\nComplete\nEvent",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "P1yrpQcWrJEMp1ZhOexMk",
			"originalText": "Download\nComplete\nEvent",
			"lineHeight": 1.25,
			"baseline": 54
		},
		{
			"type": "arrow",
			"version": 112,
			"versionNonce": 1258601990,
			"isDeleted": false,
			"id": "FQEdt-xkIB8SwWa2cvqZ0",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1601.9962339476817,
			"y": 1631.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 120.41518796374044,
			"height": 0,
			"seed": 1952836638,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510737,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "P1yrpQcWrJEMp1ZhOexMk",
				"gap": 7.155037497394943,
				"focus": 0.011428571428571429
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					120.41518796374044,
					0
				]
			]
		},
		{
			"type": "rectangle",
			"version": 77,
			"versionNonce": 343853827,
			"isDeleted": false,
			"id": "sS7KzD8tWjrqAtqLsEZ5v",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1737.9114219114222,
			"y": 1607.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 211,
			"height": 56,
			"seed": 2143901122,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "wKyxyGok"
				},
				{
					"id": "QQ_kXI61vWUJ_m5f_2tZH",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 61,
			"versionNonce": 266967725,
			"isDeleted": false,
			"id": "wKyxyGok",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1752.7792350217737,
			"y": 1615.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 181.26437377929688,
			"height": 40,
			"seed": 1846741442,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "Copy files from STEAM\nto LOCAL",
			"rawText": "Copy files from STEAM\nto LOCAL",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "sS7KzD8tWjrqAtqLsEZ5v",
			"originalText": "Copy files from STEAM\nto LOCAL",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "arrow",
			"version": 115,
			"versionNonce": 1106197062,
			"isDeleted": false,
			"id": "QQ_kXI61vWUJ_m5f_2tZH",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1960.4114219114224,
			"y": 1637.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 122.00000000000023,
			"height": 0,
			"seed": 1106888770,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510738,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "sS7KzD8tWjrqAtqLsEZ5v",
				"gap": 11.500000000000227,
				"focus": 0.07142857142857142
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					122.00000000000023,
					0
				]
			]
		},
		{
			"type": "rectangle",
			"version": 75,
			"versionNonce": 781958413,
			"isDeleted": false,
			"id": "Tb-IEoNpoX4MMgjgJ30gZ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2106.4114219114226,
			"y": 1605.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 312,
			"height": 68,
			"seed": 1123557534,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "fIzLkx6n"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 61,
			"versionNonce": 1741956675,
			"isDeleted": false,
			"id": "fIzLkx6n",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2125.843123571579,
			"y": 1629.8951716686097,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 273.1365966796875,
			"height": 20,
			"seed": 717285598,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "Update local draft to local ready",
			"rawText": "Update local draft to local ready",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "Tb-IEoNpoX4MMgjgJ30gZ",
			"originalText": "Update local draft to local ready",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 95,
			"versionNonce": 1268421485,
			"isDeleted": false,
			"id": "48MZWCI1EvBRaG3-HJ-J3",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1044.7447552447547,
			"y": 1534.7285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 263,
			"height": 52,
			"seed": 613909250,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "EIwLKrvn"
				},
				{
					"id": "rN3lVqz9V14VXD3MWR_I3",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 73,
			"versionNonce": 1067214307,
			"isDeleted": false,
			"id": "EIwLKrvn",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1092.634838557743,
			"y": 1548.2285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 167.21983337402344,
			"height": 25,
			"seed": 1693274498,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Manual Download",
			"rawText": "Manual Download",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "48MZWCI1EvBRaG3-HJ-J3",
			"originalText": "Manual Download",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 95,
			"versionNonce": 1664064973,
			"isDeleted": false,
			"id": "-Uir7lkiuBOHjzANwW69_",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1049.7447552447547,
			"y": 1668.7285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 263,
			"height": 52,
			"seed": 384433858,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "BvlM3ZsK"
				},
				{
					"id": "2P-eMxVecmUkzM5fCcv0a",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 91,
			"versionNonce": 1601941891,
			"isDeleted": false,
			"id": "BvlM3ZsK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1089.2848629718055,
			"y": 1682.2285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 183.91978454589844,
			"height": 25,
			"seed": 612423298,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Steam Subscription",
			"rawText": "Steam Subscription",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "-Uir7lkiuBOHjzANwW69_",
			"originalText": "Steam Subscription",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 282,
			"versionNonce": 1731059910,
			"isDeleted": false,
			"id": "rN3lVqz9V14VXD3MWR_I3",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1323.2447552447547,
			"y": 1570.7285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 89.38987471188807,
			"height": 30.473820924507436,
			"seed": 1614553182,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510740,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "48MZWCI1EvBRaG3-HJ-J3",
				"gap": 15.5,
				"focus": -0.5663404460131549
			},
			"endBinding": {
				"elementId": "P1yrpQcWrJEMp1ZhOexMk",
				"gap": 16,
				"focus": 0.0012554112554135357
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					89.38987471188807,
					30.473820924507436
				]
			]
		},
		{
			"type": "arrow",
			"version": 273,
			"versionNonce": 1423238982,
			"isDeleted": false,
			"id": "2P-eMxVecmUkzM5fCcv0a",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1323.2447552447547,
			"y": 1696.728505001943,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 104.00000000000023,
			"height": 26,
			"seed": 1246663810,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510741,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "-Uir7lkiuBOHjzANwW69_",
				"gap": 10.5,
				"focus": 0.6369426751592391
			},
			"endBinding": {
				"elementId": "P1yrpQcWrJEMp1ZhOexMk",
				"gap": 13.606859817679066,
				"focus": -0.24904761904761707
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					104.00000000000023,
					-26
				]
			]
		},
		{
			"type": "rectangle",
			"version": 96,
			"versionNonce": 1062358669,
			"isDeleted": false,
			"id": "xvdmvJCMozZKHwGcYoDMq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1269.7447552447547,
			"y": 1036.7285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 119,
			"height": 48,
			"seed": 847333470,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "oK6f3Ani"
				},
				{
					"id": "ngQYG3tFkynxUPSiksqH8",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 76,
			"versionNonce": 1353280707,
			"isDeleted": false,
			"id": "oK6f3Ani",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1296.7647900347938,
			"y": 1048.2285050019427,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 64.95993041992188,
			"height": 25,
			"seed": 1647611038,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Search",
			"rawText": "Search",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "xvdmvJCMozZKHwGcYoDMq",
			"originalText": "Search",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "text",
			"version": 236,
			"versionNonce": 2055402733,
			"isDeleted": false,
			"id": "XDYTs1Lr",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1645.877122877121,
			"y": 1965.072882846321,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 271.4400634765625,
			"height": 45,
			"seed": 1884844610,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 36,
			"fontFamily": 1,
			"text": "Upload Process",
			"rawText": "Upload Process",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Upload Process",
			"lineHeight": 1.25,
			"baseline": 32
		},
		{
			"type": "rectangle",
			"version": 171,
			"versionNonce": 453303395,
			"isDeleted": false,
			"id": "zMLMhvtiFrV-EYpUAkJX4",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 997.5884670884652,
			"y": 2319.655244928683,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 463.0769230769233,
			"height": 304.615384615385,
			"seed": 365150594,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 183,
			"versionNonce": 697290573,
			"isDeleted": false,
			"id": "mcubIcrl",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1139.1269286269262,
			"y": 2341.1937064671447,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 203.02886962890625,
			"height": 35,
			"seed": 278151490,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Selection",
			"rawText": "Level Selection",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Level Selection",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 200,
			"versionNonce": 1402034179,
			"isDeleted": false,
			"id": "1U5x9NGNRbu07iA89gNvs",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1169.9987234987211,
			"y": 2548.6424244158625,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 119,
			"height": 48,
			"seed": 1151719234,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "OqZwcDmU"
				},
				{
					"id": "lkEiKFgQht9IIgYeWLnTZ",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 184,
			"versionNonce": 1792171437,
			"isDeleted": false,
			"id": "OqZwcDmU",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1196.8987631715727,
			"y": 2560.1424244158625,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 65.19992065429688,
			"height": 25,
			"seed": 1311612674,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Upload",
			"rawText": "Upload",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "1U5x9NGNRbu07iA89gNvs",
			"originalText": "Upload",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 627,
			"versionNonce": 206322374,
			"isDeleted": false,
			"id": "lkEiKFgQht9IIgYeWLnTZ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1299.8221223221226,
			"y": 2564.6031102179786,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 338.84925971901,
			"height": 121.79165889997876,
			"seed": 1755573442,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510745,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "1U5x9NGNRbu07iA89gNvs",
				"gap": 10.823398823401476,
				"focus": 0.37978367112149364
			},
			"endBinding": {
				"elementId": "xiNPhNsLlsjjMQ_qzAAPS",
				"gap": 12.195078352637076,
				"focus": 0.458904109589044
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					338.84925971901,
					-121.79165889997876
				]
			]
		},
		{
			"type": "diamond",
			"version": 184,
			"versionNonce": 1433673741,
			"isDeleted": false,
			"id": "xiNPhNsLlsjjMQ_qzAAPS",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1650.4584859584857,
			"y": 2380.9394086815964,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 125,
			"height": 130,
			"seed": 1303962462,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "lkEiKFgQht9IIgYeWLnTZ",
					"type": "arrow"
				},
				{
					"type": "text",
					"id": "HEH5pT8u"
				},
				{
					"id": "91U_GVU8p1wRbEVIGRivw",
					"type": "arrow"
				},
				{
					"id": "1zl5SzzYcFHrbW12xbYNH",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 156,
			"versionNonce": 57111363,
			"isDeleted": false,
			"id": "HEH5pT8u",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1688.984441158681,
			"y": 2435.9394086815964,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 48.448089599609375,
			"height": 20,
			"seed": 1664405918,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "isNew?",
			"rawText": "isNew?",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "xiNPhNsLlsjjMQ_qzAAPS",
			"originalText": "isNew?",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "arrow",
			"version": 351,
			"versionNonce": 1438970374,
			"isDeleted": false,
			"id": "91U_GVU8p1wRbEVIGRivw",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1767.0948495948492,
			"y": 2417.4394086815964,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 361.81818181818153,
			"height": 149.090909090909,
			"seed": 524904770,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "q0llEPmh"
				}
			],
			"updated": 1695319510745,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "xiNPhNsLlsjjMQ_qzAAPS",
				"gap": 13.724812779451163,
				"focus": -0.09527005657658995
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					361.81818181818153,
					-149.090909090909
				]
			]
		},
		{
			"type": "text",
			"version": 133,
			"versionNonce": 87954147,
			"isDeleted": false,
			"id": "q0llEPmh",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1821.9439582041355,
			"y": 2345.8484995906874,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 32.119964599609375,
			"height": 25,
			"seed": 759679326,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Yes",
			"rawText": "Yes",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "91U_GVU8p1wRbEVIGRivw",
			"originalText": "Yes",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 612,
			"versionNonce": 1372457990,
			"isDeleted": false,
			"id": "1zl5SzzYcFHrbW12xbYNH",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1762.7855166389704,
			"y": 2482.564392209825,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 158.99865792583614,
			"height": 51.767004906086186,
			"seed": 120033410,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "Ov3BpWPN"
				}
			],
			"updated": 1695319510748,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "xiNPhNsLlsjjMQ_qzAAPS",
				"gap": 16.25,
				"focus": 0.3138804515874926
			},
			"endBinding": {
				"elementId": "Ae-yFM7DsmxLoUrltIF5B",
				"gap": 11.67431139368,
				"focus": -0.5147611214934678
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					158.99865792583614,
					51.767004906086186
				]
			]
		},
		{
			"type": "text",
			"version": 155,
			"versionNonce": 1046462083,
			"isDeleted": false,
			"id": "Ov3BpWPN",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1831.622368284844,
			"y": 2496.3768516613654,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 23.959976196289062,
			"height": 25,
			"seed": 1655058498,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "No",
			"rawText": "No",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "1zl5SzzYcFHrbW12xbYNH",
			"originalText": "No",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 448,
			"versionNonce": 660840237,
			"isDeleted": false,
			"id": "0VGBY0ObYjhaKb3ct-Oqn",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2146.1857586857577,
			"y": 2243.1212268634144,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 250,
			"height": 49,
			"seed": 657560194,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "EWIUa50b"
				},
				{
					"id": "ZKQtfwHtyG5XU6-Q5ZjyA",
					"type": "arrow"
				},
				{
					"id": "04jRekTN8BiZIJcmt7KcE",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 492,
			"versionNonce": 696972835,
			"isDeleted": false,
			"id": "EWIUa50b",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2156.7858792301913,
			"y": 2255.1212268634144,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 228.7997589111328,
			"height": 25,
			"seed": 908063298,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Editor.NewCommunityFile",
			"rawText": "Editor.NewCommunityFile",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "0VGBY0ObYjhaKb3ct-Oqn",
			"originalText": "Editor.NewCommunityFile",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 446,
			"versionNonce": 1096888717,
			"isDeleted": false,
			"id": "Ae-yFM7DsmxLoUrltIF5B",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1933.4584859584866,
			"y": 2503.1212268634144,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 180,
			"height": 65,
			"seed": 1846664514,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "i0Wloaas"
				},
				{
					"id": "1zl5SzzYcFHrbW12xbYNH",
					"type": "arrow"
				},
				{
					"id": "cHN2HDFpAefNPehDkXcuy",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 441,
			"versionNonce": 1705916867,
			"isDeleted": false,
			"id": "i0Wloaas",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 1952.4385503505764,
			"y": 2523.1212268634144,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 142.0398712158203,
			"height": 25,
			"seed": 942204162,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Item.GetAsync",
			"rawText": "Item.GetAsync",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "Ae-yFM7DsmxLoUrltIF5B",
			"originalText": "Item.GetAsync",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 566,
			"versionNonce": 2027739398,
			"isDeleted": false,
			"id": "cHN2HDFpAefNPehDkXcuy",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2121.6403041403046,
			"y": 2539.2575904997775,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 101.81818181818107,
			"height": 1.8181818181819835,
			"seed": 2084812126,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510749,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "Ae-yFM7DsmxLoUrltIF5B",
				"gap": 8.181818181818016,
				"focus": 0.05521180390288119
			},
			"endBinding": {
				"elementId": "V_LBcRw0dYaG_UlqDnZBR",
				"gap": 13.636363636363058,
				"focus": -0.10756782484529447
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					101.81818181818107,
					1.8181818181819835
				]
			]
		},
		{
			"type": "rectangle",
			"version": 482,
			"versionNonce": 803735907,
			"isDeleted": false,
			"id": "V_LBcRw0dYaG_UlqDnZBR",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2237.0948495948487,
			"y": 2506.7575904997784,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 180,
			"height": 65,
			"seed": 848002526,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "RXak331P"
				},
				{
					"id": "cHN2HDFpAefNPehDkXcuy",
					"type": "arrow"
				},
				{
					"id": "jkCK-jQdu1flXUcVwosXF",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 479,
			"versionNonce": 696382029,
			"isDeleted": false,
			"id": "RXak331P",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2281.184899338501,
			"y": 2526.7575904997784,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 91.81990051269531,
			"height": 25,
			"seed": 509350430,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Item.Edit",
			"rawText": "Item.Edit",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "V_LBcRw0dYaG_UlqDnZBR",
			"originalText": "Item.Edit",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 169,
			"versionNonce": 495379715,
			"isDeleted": false,
			"id": "_wRWiIAipaIZ65XBSaMIW",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2505.2766677766676,
			"y": 2310.1666814088694,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 170.909090909091,
			"height": 167.27272727272725,
			"seed": 1559902046,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "ZKQtfwHtyG5XU6-Q5ZjyA",
					"type": "arrow"
				},
				{
					"id": "jkCK-jQdu1flXUcVwosXF",
					"type": "arrow"
				},
				{
					"id": "2Tu6jbreXNVoCZQxczBIm",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 153,
			"versionNonce": 243802285,
			"isDeleted": false,
			"id": "qwunMpiK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2530.7312132312127,
			"y": 2385.3939541361415,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 121.58427429199219,
			"height": 20,
			"seed": 533177758,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Make changes",
			"rawText": "- Make changes",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Make changes",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "arrow",
			"version": 458,
			"versionNonce": 1328594694,
			"isDeleted": false,
			"id": "ZKQtfwHtyG5XU6-Q5ZjyA",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2403.4584859584857,
			"y": 2273.8030450452325,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 85.81818181818198,
			"height": 91.66942148760245,
			"seed": 1409984734,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510746,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "0VGBY0ObYjhaKb3ct-Oqn",
				"gap": 7.272727272727934,
				"focus": -0.8550004576180363
			},
			"endBinding": {
				"elementId": "_wRWiIAipaIZ65XBSaMIW",
				"gap": 16,
				"focus": -0.45759508622725037
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					85.81818181818198,
					91.66942148760245
				]
			]
		},
		{
			"type": "arrow",
			"version": 461,
			"versionNonce": 1825755014,
			"isDeleted": false,
			"id": "jkCK-jQdu1flXUcVwosXF",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2421.6403041403046,
			"y": 2531.9848632270505,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 70.90909090909008,
			"height": 103.63636363636306,
			"seed": 1420850370,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510749,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "V_LBcRw0dYaG_UlqDnZBR",
				"gap": 4.545454545455868,
				"focus": 0.7980390067142793
			},
			"endBinding": {
				"elementId": "_wRWiIAipaIZ65XBSaMIW",
				"gap": 12.727272727272975,
				"focus": 0.5224681421864564
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					70.90909090909008,
					-103.63636363636306
				]
			]
		},
		{
			"type": "text",
			"version": 196,
			"versionNonce": 220053571,
			"isDeleted": false,
			"id": "FT0rAJ5K",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2547.1605920875254,
			"y": 2330.3939541361415,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 100.03988647460938,
			"height": 25,
			"seed": 1473641630,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "Ugc.Editor",
			"rawText": "Ugc.Editor",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Ugc.Editor",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 232,
			"versionNonce": 751802733,
			"isDeleted": false,
			"id": "sKbuv66ze7qObIt1XF_qa",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2761.5493950493947,
			"y": 2281.2575904997784,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 302,
			"height": 196.0000000000001,
			"seed": 897549086,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "2Tu6jbreXNVoCZQxczBIm",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 202,
			"versionNonce": 2091040739,
			"isDeleted": false,
			"id": "0IQPQpRo",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2790.5833132567377,
			"y": 2438.3484995906874,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 247.5685272216797,
			"height": 20,
			"seed": 1140814302,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Send progress updates to UI",
			"rawText": "- Send progress updates to UI",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Send progress updates to UI",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "text",
			"version": 256,
			"versionNonce": 601088973,
			"isDeleted": false,
			"id": "N8mq2Geb",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2855.4776690306617,
			"y": 2328.8030450452325,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 129.8198699951172,
			"height": 25,
			"seed": 1231207938,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "SubmitAsync()",
			"rawText": "SubmitAsync()",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "SubmitAsync()",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 345,
			"versionNonce": 514247555,
			"isDeleted": false,
			"id": "2Tu6jbreXNVoCZQxczBIm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2694.3675768675766,
			"y": 2388.3484995906874,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 49.09090909090901,
			"height": 1.8181818181819835,
			"seed": 1108199006,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404505,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "_wRWiIAipaIZ65XBSaMIW",
				"focus": -0.018619084561672085,
				"gap": 18.181818181818016
			},
			"endBinding": {
				"elementId": "sKbuv66ze7qObIt1XF_qa",
				"focus": -0.009750706926244124,
				"gap": 18.09090909090901
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					49.09090909090901,
					-1.8181818181819835
				]
			]
		},
		{
			"type": "arrow",
			"version": 490,
			"versionNonce": 2145727878,
			"isDeleted": false,
			"id": "04jRekTN8BiZIJcmt7KcE",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2401.3512098949814,
			"y": 2239.677191166452,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 97.01636697259528,
			"height": 71.06429075562528,
			"seed": 1265150338,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510747,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "0VGBY0ObYjhaKb3ct-Oqn",
				"gap": 6.208322484924793,
				"focus": 0.5807391688325012
			},
			"endBinding": {
				"elementId": "-8VCyzqXXyHpOqbvxtse5",
				"gap": 16,
				"focus": 0.6390901771336579
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					97.01636697259528,
					-71.06429075562528
				]
			]
		},
		{
			"type": "rectangle",
			"version": 209,
			"versionNonce": 1734894371,
			"isDeleted": false,
			"id": "-8VCyzqXXyHpOqbvxtse5",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2514.3675768675766,
			"y": 2081.0757723179604,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 407.27272727272697,
			"height": 121.81818181818176,
			"seed": 2119863262,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "04jRekTN8BiZIJcmt7KcE",
					"type": "arrow"
				}
			],
			"updated": 1695301404505,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 372,
			"versionNonce": 1655190042,
			"isDeleted": false,
			"id": "BlVKw4Sp",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2545.2766677766676,
			"y": 2101.7575904997784,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 361.82476806640625,
			"height": 100,
			"seed": 1952292190,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695319510902,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Create a draft search result to \nenhance level browser (tagged as pending)\n- This can be removed when the first search \nreturns a result for the uploaded level\n",
			"rawText": "- Create a draft search result to \nenhance level browser (tagged as pending)\n- This can be removed when the first search \nreturns a result for the uploaded level\n",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Create a draft search result to \nenhance level browser (tagged as pending)\n- This can be removed when the first search \nreturns a result for the uploaded level\n",
			"lineHeight": 1.25,
			"baseline": 94
		},
		{
			"type": "text",
			"version": 67,
			"versionNonce": 1062602435,
			"isDeleted": false,
			"id": "lA9g2of0",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": 2484.8221223221212,
			"y": 866.5303177725048,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 175.2323455810547,
			"height": 20,
			"seed": 305309598,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Because of queries ",
			"rawText": "- Because of queries ",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Because of queries ",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "text",
			"version": 107,
			"versionNonce": 235207405,
			"isDeleted": false,
			"id": "dZ99yE78",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6531.387290813764,
			"y": -652.3590428668551,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 481.53619384765625,
			"height": 45,
			"seed": 1045617901,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 36,
			"fontFamily": 1,
			"text": "Level Storage Architecture",
			"rawText": "Level Storage Architecture",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "Level Storage Architecture",
			"lineHeight": 1.25,
			"baseline": 32
		},
		{
			"type": "rectangle",
			"version": 557,
			"versionNonce": 2098083427,
			"isDeleted": false,
			"id": "wuc0XZEIyHF-2Ltn_I7xw",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5954.457001804528,
			"y": -127.31750983848008,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 262,
			"height": 99,
			"seed": 1205341123,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "R8RPNIwm"
				},
				{
					"id": "rRpuy5fwL1S58HnY4WwMd",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 493,
			"versionNonce": 1256265037,
			"isDeleted": false,
			"id": "R8RPNIwm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5909.296914219079,
			"y": -90.31750983848008,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 171.67982482910156,
			"height": 25,
			"seed": 285711203,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelPlayManager",
			"rawText": "LevelPlayManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "wuc0XZEIyHF-2Ltn_I7xw",
			"originalText": "LevelPlayManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 449,
			"versionNonce": 526348803,
			"isDeleted": false,
			"id": "knVDw1Tz7SQg3FzEZvjPf",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5982.878054436104,
			"y": 131.7702094597655,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 1684414445,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "dOt9MPle"
				},
				{
					"id": "rRpuy5fwL1S58HnY4WwMd",
					"type": "arrow"
				},
				{
					"id": "bA17AvC39OMrZu2VHTbdK",
					"type": "arrow"
				},
				{
					"id": "8V8fquMc8fpyOonITd0pu",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 369,
			"versionNonce": 504030125,
			"isDeleted": false,
			"id": "dOt9MPle",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5891.117991264718,
			"y": 167.2702094597655,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 130.47987365722656,
			"height": 25,
			"seed": 1674943053,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelManager",
			"rawText": "LevelManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "knVDw1Tz7SQg3FzEZvjPf",
			"originalText": "LevelManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 1085,
			"versionNonce": 1336138886,
			"isDeleted": false,
			"id": "rRpuy5fwL1S58HnY4WwMd",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5825.609272552154,
			"y": -12.598211592866022,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 9.88948086923483,
			"height": 126.54385964912271,
			"seed": 1625656035,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510751,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "wuc0XZEIyHF-2Ltn_I7xw",
				"gap": 15.719298245614056,
				"focus": 0.05375014636199797
			},
			"endBinding": {
				"elementId": "knVDw1Tz7SQg3FzEZvjPf",
				"gap": 17.824561403508824,
				"focus": 0.09519362902960514
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					9.88948086923483,
					126.54385964912271
				]
			]
		},
		{
			"type": "arrow",
			"version": 599,
			"versionNonce": 2033336326,
			"isDeleted": false,
			"id": "bA17AvC39OMrZu2VHTbdK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5821.901070240768,
			"y": 242.08599893344967,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 2.25027835072342,
			"height": 146.52631578947359,
			"seed": 955368195,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510753,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "knVDw1Tz7SQg3FzEZvjPf",
				"gap": 14.315789473684106,
				"focus": -0.03127987800358524
			},
			"endBinding": {
				"elementId": "z-2gkwTeQjDglr72zPRgq",
				"gap": 12.657894736842024,
				"focus": 0.13450224661444746
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-2.25027835072342,
					146.52631578947359
				]
			]
		},
		{
			"type": "rectangle",
			"version": 285,
			"versionNonce": 1912016195,
			"isDeleted": false,
			"id": "z-2gkwTeQjDglr72zPRgq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5923.439457944877,
			"y": 401.2702094597653,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 286725549,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "BRtrEi6K"
				},
				{
					"id": "bA17AvC39OMrZu2VHTbdK",
					"type": "arrow"
				},
				{
					"id": "DpgIQlN-JEdyXw_ziK7Af",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 249,
			"versionNonce": 381525101,
			"isDeleted": false,
			"id": "BRtrEi6K",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5852.8634920635295,
			"y": 413.7702094597653,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 32.84806823730469,
			"height": 20,
			"seed": 2138729763,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "load",
			"rawText": "load",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "z-2gkwTeQjDglr72zPRgq",
			"originalText": "load",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 95,
			"versionNonce": 1925868771,
			"isDeleted": false,
			"id": "26Mpm9PT76NGOk6-wnoS5",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6990.644308306078,
			"y": -450.08419370500945,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 722,
			"height": 72,
			"seed": 99253805,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "8LZeNhei"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 33,
			"versionNonce": 1191806669,
			"isDeleted": false,
			"id": "8LZeNhei",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6716.3466703666245,
			"y": -431.58419370500945,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 173.40472412109375,
			"height": 35,
			"seed": 264279597,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Editing",
			"rawText": "Level Editing",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "26Mpm9PT76NGOk6-wnoS5",
			"originalText": "Level Editing",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 110,
			"versionNonce": 760273027,
			"isDeleted": false,
			"id": "2gmQxRNyC4v8fx2txdFrm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6173.977641639409,
			"y": -452.75086037167614,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 722,
			"height": 72,
			"seed": 2048766541,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "ZKJm7aAq"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 57,
			"versionNonce": 791225645,
			"isDeleted": false,
			"id": "ZKJm7aAq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5900.141994056401,
			"y": -434.25086037167614,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174.32870483398438,
			"height": 35,
			"seed": 1437327533,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Playing",
			"rawText": "Level Playing",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "2gmQxRNyC4v8fx2txdFrm",
			"originalText": "Level Playing",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 140,
			"versionNonce": 256430115,
			"isDeleted": false,
			"id": "wdAVCDbK7PrIWn5X7kdvQ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5400.644308306076,
			"y": -450.08419370500934,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 722,
			"height": 72,
			"seed": 1766817901,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "axTKRCen"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 97,
			"versionNonce": 42334093,
			"isDeleted": false,
			"id": "axTKRCen",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5138.050718523361,
			"y": -431.58419370500934,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 196.8128204345703,
			"height": 35,
			"seed": 901704397,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Browsing",
			"rawText": "Level Browsing",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "wdAVCDbK7PrIWn5X7kdvQ",
			"originalText": "Level Browsing",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "rectangle",
			"version": 260,
			"versionNonce": 1252053955,
			"isDeleted": false,
			"id": "fkpUowQCOq4z4z0j3kzvT",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7779.310974972743,
			"y": -450.08419370500934,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 722,
			"height": 72,
			"seed": 1915588995,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "RjwU5yQ1"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 242,
			"versionNonce": 1264156141,
			"isDeleted": false,
			"id": "RjwU5yQ1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7519.825409787196,
			"y": -431.58419370500934,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 203.02886962890625,
			"height": 35,
			"seed": 1780902179,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "Level Selection",
			"rawText": "Level Selection",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "fkpUowQCOq4z4z0j3kzvT",
			"originalText": "Level Selection",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "text",
			"version": 251,
			"versionNonce": 1333224291,
			"isDeleted": false,
			"id": "Wn19ZoUq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7759.644308306075,
			"y": -343.91752703834277,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 172.00033569335938,
			"height": 140,
			"seed": 1527676749,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- See a list of levels\n- Delete levels\n- Publish\n- Go to browse levels\n- Go to leaderboard\n- Go to play\n- Go to edit",
			"rawText": "- See a list of levels\n- Delete levels\n- Publish\n- Go to browse levels\n- Go to leaderboard\n- Go to play\n- Go to edit",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- See a list of levels\n- Delete levels\n- Publish\n- Go to browse levels\n- Go to leaderboard\n- Go to play\n- Go to edit",
			"lineHeight": 1.25,
			"baseline": 134
		},
		{
			"type": "text",
			"version": 126,
			"versionNonce": 279787597,
			"isDeleted": false,
			"id": "qt5x7Wqf",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6934.310974972742,
			"y": -340.7508603716761,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 171.82437133789062,
			"height": 80,
			"seed": 1350955427,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Create new levels\n- Edit old levels\n- Save and Play test\n- Save and exit",
			"rawText": "- Create new levels\n- Edit old levels\n- Save and Play test\n- Save and exit",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Create new levels\n- Edit old levels\n- Save and Play test\n- Save and exit",
			"lineHeight": 1.25,
			"baseline": 74
		},
		{
			"type": "text",
			"version": 109,
			"versionNonce": 549393542,
			"isDeleted": false,
			"id": "PQ8HWgSh",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5323.644308306074,
			"y": -346.0841937050096,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 155.9523162841797,
			"height": 80,
			"seed": 415453667,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695319510903,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Searching levels\n- Staring levels\n- Downloading levels\n",
			"rawText": "- Searching levels\n- Staring levels\n- Downloading levels\n",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Searching levels\n- Staring levels\n- Downloading levels\n",
			"lineHeight": 1.25,
			"baseline": 74
		},
		{
			"type": "text",
			"version": 99,
			"versionNonce": 1012573869,
			"isDeleted": false,
			"id": "Bva8kykb",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6084.977641639407,
			"y": -347.417527038343,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 161.98435974121094,
			"height": 60,
			"seed": 1461071053,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Go back to levels\n- Go back to editing\n- Submitting scores",
			"rawText": "- Go back to levels\n- Go back to editing\n- Submitting scores",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Go back to levels\n- Go back to editing\n- Submitting scores",
			"lineHeight": 1.25,
			"baseline": 54
		},
		{
			"type": "rectangle",
			"version": 348,
			"versionNonce": 1225402019,
			"isDeleted": false,
			"id": "kySAjqpNWj-vCYmgcZqlq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6842.732259828239,
			"y": -126.63011269829667,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 651347043,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "J-_EWAw3hTCae0khLUUAg",
					"type": "arrow"
				},
				{
					"type": "text",
					"id": "RIvbbOXY"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 82,
			"versionNonce": 1091258637,
			"isDeleted": false,
			"id": "RIvbbOXY",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6781.082159120231,
			"y": -91.13011269829667,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 190.69979858398438,
			"height": 25,
			"seed": 724023405,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "EditorLevelManager",
			"rawText": "EditorLevelManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "kySAjqpNWj-vCYmgcZqlq",
			"originalText": "EditorLevelManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 418,
			"versionNonce": 519264835,
			"isDeleted": false,
			"id": "CC6k6Xkx1L79qjWOMS9GH",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6836.310742748823,
			"y": 151.29738524235916,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 1725430499,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "AXYJFgnq"
				},
				{
					"id": "J-_EWAw3hTCae0khLUUAg",
					"type": "arrow"
				},
				{
					"id": "Cxazv3QN_9lqzGxzDZd2L",
					"type": "arrow"
				},
				{
					"id": "gD8nAD6OVUmyfMfOGSKTL",
					"type": "arrow"
				},
				{
					"id": "tJx2KlAPM9jE9Udk3p88p",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 338,
			"versionNonce": 3710829,
			"isDeleted": false,
			"id": "AXYJFgnq",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6744.550679577436,
			"y": 186.79738524235916,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 130.47987365722656,
			"height": 25,
			"seed": 612148867,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelManager",
			"rawText": "LevelManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "CC6k6Xkx1L79qjWOMS9GH",
			"originalText": "LevelManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 745,
			"versionNonce": 1350566790,
			"isDeleted": false,
			"id": "J-_EWAw3hTCae0khLUUAg",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6683.003824555381,
			"y": -22.948228792728628,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 9.099947598264407,
			"height": 152.49122807017545,
			"seed": 197844515,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510758,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "kySAjqpNWj-vCYmgcZqlq",
				"gap": 7.681883905568036,
				"focus": -0.0378524771653463
			},
			"endBinding": {
				"elementId": "CC6k6Xkx1L79qjWOMS9GH",
				"gap": 21.75438596491233,
				"focus": -0.10606261694053115
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-9.099947598264407,
					152.49122807017545
				]
			]
		},
		{
			"type": "diamond",
			"version": 374,
			"versionNonce": 1881942477,
			"isDeleted": false,
			"id": "Z_OmrF3X-m17F7sCjU3x4",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6852.801970819001,
			"y": -23.649983178693333,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 142,
			"height": 174,
			"seed": 558756195,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "GztYoZXo"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 90,
			"versionNonce": 392029571,
			"isDeleted": false,
			"id": "GztYoZXo",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6811.86603484488,
			"y": 33.35001682130667,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 60.12812805175781,
			"height": 60,
			"seed": 653217891,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "create/\nload/\nsave",
			"rawText": "create/\nload/\nsave",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "Z_OmrF3X-m17F7sCjU3x4",
			"originalText": "create/\nload/\nsave",
			"lineHeight": 1.25,
			"baseline": 54
		},
		{
			"type": "arrow",
			"version": 479,
			"versionNonce": 2067766662,
			"isDeleted": false,
			"id": "Cxazv3QN_9lqzGxzDZd2L",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6751.445566317321,
			"y": 263.29738524235916,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 175.54127834617884,
			"height": 146.17543859649112,
			"seed": 936683491,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510760,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "CC6k6Xkx1L79qjWOMS9GH",
				"gap": 16,
				"focus": -0.02200129993373752
			},
			"endBinding": {
				"elementId": "MDLST_Meh1mb9rmayRK4Y",
				"gap": 9.942982456140214,
				"focus": -0.3125454227054885
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-175.54127834617884,
					146.17543859649112
				]
			]
		},
		{
			"type": "arrow",
			"version": 412,
			"versionNonce": 934179846,
			"isDeleted": false,
			"id": "gD8nAD6OVUmyfMfOGSKTL",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6670.29125218142,
			"y": 263.29738524235916,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 4.2625412635225075,
			"height": 139.43859649122788,
			"seed": 1801171843,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510761,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "CC6k6Xkx1L79qjWOMS9GH",
				"gap": 16,
				"focus": -0.0445710485878078
			},
			"endBinding": {
				"elementId": "7JZtGwqhHYPZNtXK3rAP_",
				"gap": 15.346491228070192,
				"focus": 0.09244012121457792
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					4.2625412635225075,
					139.43859649122788
				]
			]
		},
		{
			"type": "arrow",
			"version": 521,
			"versionNonce": 1464418566,
			"isDeleted": false,
			"id": "tJx2KlAPM9jE9Udk3p88p",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6560.8768547124755,
			"y": 259.01668348797324,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 171.74475491857356,
			"height": 146.52631578947353,
			"seed": 490752803,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510762,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "CC6k6Xkx1L79qjWOMS9GH",
				"gap": 11.719298245614027,
				"focus": -0.22712233656579622
			},
			"endBinding": {
				"elementId": "T8suBcwmAsjH4xjzNbOtz",
				"gap": 7.206140350877206,
				"focus": 0.41160235230628595
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					171.74475491857356,
					146.52631578947353
				]
			]
		},
		{
			"type": "text",
			"version": 175,
			"versionNonce": 1955504323,
			"isDeleted": false,
			"id": "PYDMiYnC",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6672.977641639411,
			"y": 35.249139628323974,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 278.5926208496094,
			"height": 40,
			"seed": 1566229773,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- create or load happens at start\n- save is triggered by UI buttons",
			"rawText": "- create or load happens at start\n- save is triggered by UI buttons",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- create or load happens at start\n- save is triggered by UI buttons",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "rectangle",
			"version": 293,
			"versionNonce": 1505427693,
			"isDeleted": false,
			"id": "MDLST_Meh1mb9rmayRK4Y",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7017.310974972744,
			"y": 419.4158062949905,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 1298194925,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "70f3AtW9"
				},
				{
					"id": "Cxazv3QN_9lqzGxzDZd2L",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 260,
			"versionNonce": 1948487779,
			"isDeleted": false,
			"id": "70f3AtW9",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6956.375031369228,
			"y": 431.9158062949905,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 52.12811279296875,
			"height": 20,
			"seed": 606608461,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "create",
			"rawText": "create",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "MDLST_Meh1mb9rmayRK4Y",
			"originalText": "create",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 152,
			"versionNonce": 2112248653,
			"isDeleted": false,
			"id": "7JZtGwqhHYPZNtXK3rAP_",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6759.977641639412,
			"y": 418.08247296165723,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 959541571,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "YOqfI4HL"
				},
				{
					"id": "gD8nAD6OVUmyfMfOGSKTL",
					"type": "arrow"
				},
				{
					"id": "NyVOIW_Wsv4zS3ZDbS1hz",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 126,
			"versionNonce": 2126089219,
			"isDeleted": false,
			"id": "YOqfI4HL",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6689.401675758064,
			"y": 430.58247296165723,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 32.84806823730469,
			"height": 20,
			"seed": 756568291,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "load",
			"rawText": "load",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "7JZtGwqhHYPZNtXK3rAP_",
			"originalText": "load",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 160,
			"versionNonce": 209152429,
			"isDeleted": false,
			"id": "T8suBcwmAsjH4xjzNbOtz",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6487.977641639412,
			"y": 412.749139628324,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 1912939821,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "jEV82e12"
				},
				{
					"id": "tJx2KlAPM9jE9Udk3p88p",
					"type": "arrow"
				},
				{
					"id": "TqjWwyvc_C3wWWjfZKjVM",
					"type": "arrow"
				},
				{
					"id": "Gv4fQofHcdfczT442J7wh",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 126,
			"versionNonce": 676271011,
			"isDeleted": false,
			"id": "jEV82e12",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6419.217677650154,
			"y": 425.249139628324,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 36.480072021484375,
			"height": 20,
			"seed": 165803917,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "save",
			"rawText": "save",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "T8suBcwmAsjH4xjzNbOtz",
			"originalText": "save",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 497,
			"versionNonce": 379038733,
			"isDeleted": false,
			"id": "3dqnVZsHjzYRtUaSoigS4",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7800.732259828239,
			"y": -108.94048653498612,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 475090669,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "tv2KOFrswE-QAqMLTf8dw",
					"type": "arrow"
				},
				{
					"type": "text",
					"id": "Q5sqt91K"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 252,
			"versionNonce": 17999683,
			"isDeleted": false,
			"id": "Q5sqt91K",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7751.692152101188,
			"y": -73.44048653498612,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 215.91978454589844,
			"height": 25,
			"seed": 1954736461,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelSelectionManager",
			"rawText": "LevelSelectionManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "3dqnVZsHjzYRtUaSoigS4",
			"originalText": "LevelSelectionManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 603,
			"versionNonce": 1279304301,
			"isDeleted": false,
			"id": "wnNeO--4BUrm-yAM_OP-F",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7790.310742748823,
			"y": 164.9870114056697,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 1548892077,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "cev9qd73"
				},
				{
					"id": "tv2KOFrswE-QAqMLTf8dw",
					"type": "arrow"
				},
				{
					"id": "JMwxERY5vgux3zOFTpSWW",
					"type": "arrow"
				},
				{
					"id": "eYm8SaWpOny8oatbdxsm9",
					"type": "arrow"
				},
				{
					"id": "nh3Sdy5tGZOlJqqkI_Ag1",
					"type": "arrow"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 521,
			"versionNonce": 258131683,
			"isDeleted": false,
			"id": "cev9qd73",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7698.550679577436,
			"y": 200.4870114056697,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 130.47987365722656,
			"height": 25,
			"seed": 2112403981,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelManager",
			"rawText": "LevelManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "wnNeO--4BUrm-yAM_OP-F",
			"originalText": "LevelManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 1234,
			"versionNonce": 1590909702,
			"isDeleted": false,
			"id": "tv2KOFrswE-QAqMLTf8dw",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7640.245070270666,
			"y": -5.258602629418078,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 6.769565129704461,
			"height": 148.49122807017545,
			"seed": 37744749,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510765,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "3dqnVZsHjzYRtUaSoigS4",
				"gap": 7.681883905568043,
				"focus": -0.0378524771653385
			},
			"endBinding": {
				"elementId": "wnNeO--4BUrm-yAM_OP-F",
				"gap": 21.75438596491233,
				"focus": -0.10606261694052865
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-6.769565129704461,
					148.49122807017545
				]
			]
		},
		{
			"type": "diamond",
			"version": 557,
			"versionNonce": 164491907,
			"isDeleted": false,
			"id": "a8pQoIUHEQwhQBANlIjgg",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7806.801970819,
			"y": -9.960357015382783,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 142,
			"height": 174,
			"seed": 924932813,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "rKwnMUbe"
				}
			],
			"updated": 1695301404506,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 290,
			"versionNonce": 941796141,
			"isDeleted": false,
			"id": "rKwnMUbe",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7765.682029107575,
			"y": 57.03964298461722,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 59.76011657714844,
			"height": 40,
			"seed": 1965455661,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404506,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "loadAll/\ndelete",
			"rawText": "loadAll/\ndelete",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "a8pQoIUHEQwhQBANlIjgg",
			"originalText": "loadAll/\ndelete",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "arrow",
			"version": 1058,
			"versionNonce": 1342119366,
			"isDeleted": false,
			"id": "JMwxERY5vgux3zOFTpSWW",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7673.807121090605,
			"y": 276.9870114056697,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 94.61895263727274,
			"height": 135.5087719298246,
			"seed": 1169738637,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510767,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "wnNeO--4BUrm-yAM_OP-F",
				"gap": 16,
				"focus": -0.022001299933822365
			},
			"endBinding": {
				"elementId": "r1mFl4TYWU4K3kd9wFG4r",
				"gap": 9.942982456140044,
				"focus": -0.3125454227054899
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-94.61895263727274,
					135.5087719298246
				]
			]
		},
		{
			"type": "arrow",
			"version": 1005,
			"versionNonce": 806816454,
			"isDeleted": false,
			"id": "eYm8SaWpOny8oatbdxsm9",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7592.668794517913,
			"y": 276.9870114056697,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 67.53942568760885,
			"height": 132.77192982456165,
			"seed": 1225904621,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510768,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "wnNeO--4BUrm-yAM_OP-F",
				"gap": 16,
				"focus": -0.044571048589878794
			},
			"endBinding": {
				"elementId": "pxnFcBqIQlLlqbWbRU320",
				"gap": 15.346491228069851,
				"focus": 0.0924401212145596
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					67.53942568760885,
					132.77192982456165
				]
			]
		},
		{
			"type": "rectangle",
			"version": 488,
			"versionNonce": 706702787,
			"isDeleted": false,
			"id": "r1mFl4TYWU4K3kd9wFG4r",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7845.977641639412,
			"y": 422.43876579163435,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 741342669,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "Tav1bY9s"
				},
				{
					"id": "JMwxERY5vgux3zOFTpSWW",
					"type": "arrow"
				},
				{
					"id": "ZJbYUkz0WFsN-pQX5sC4i",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 461,
			"versionNonce": 472482797,
			"isDeleted": false,
			"id": "Tav1bY9s",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7784.857692298591,
			"y": 434.93876579163435,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 51.760101318359375,
			"height": 20,
			"seed": 912833581,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "loadAll",
			"rawText": "loadAll",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "r1mFl4TYWU4K3kd9wFG4r",
			"originalText": "loadAll",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 353,
			"versionNonce": 940752227,
			"isDeleted": false,
			"id": "pxnFcBqIQlLlqbWbRU320",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7601.9776416394125,
			"y": 425.1054324583012,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 1270525581,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "4k4su5lV"
				},
				{
					"id": "eYm8SaWpOny8oatbdxsm9",
					"type": "arrow"
				},
				{
					"id": "rWa0UF4Dw1593SXvxz8jI",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 329,
			"versionNonce": 361528909,
			"isDeleted": false,
			"id": "4k4su5lV",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7539.281688270272,
			"y": 437.6054324583012,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 48.60809326171875,
			"height": 20,
			"seed": 450843885,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "delete",
			"rawText": "delete",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "pxnFcBqIQlLlqbWbRU320",
			"originalText": "delete",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 129,
			"versionNonce": 909842691,
			"isDeleted": false,
			"id": "Yc1_qhBcrADpqZClJpjJ6",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6816.977641639408,
			"y": 623.082472961657,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 1852278829,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "9JM0CEmH"
				},
				{
					"id": "NyVOIW_Wsv4zS3ZDbS1hz",
					"type": "arrow"
				},
				{
					"id": "TqjWwyvc_C3wWWjfZKjVM",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 75,
			"versionNonce": 828452013,
			"isDeleted": false,
			"id": "9JM0CEmH",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6738.113818702396,
			"y": 657.082472961657,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 90.27235412597656,
			"height": 35,
			"seed": 612458957,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "LOCAL",
			"rawText": "LOCAL",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "Yc1_qhBcrADpqZClJpjJ6",
			"originalText": "LOCAL",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 255,
			"versionNonce": 1381005254,
			"isDeleted": false,
			"id": "NyVOIW_Wsv4zS3ZDbS1hz",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6689.552482759387,
			"y": 465.91580629499015,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 4.838135889811383,
			"height": 141.16666666666686,
			"seed": 465965037,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510769,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "7JZtGwqhHYPZNtXK3rAP_",
				"gap": 2.8333333333329165,
				"focus": 0.17894954705382865
			},
			"endBinding": {
				"elementId": "Yc1_qhBcrADpqZClJpjJ6",
				"gap": 16,
				"focus": -0.029629629629637555
			},
			"lastCommittedPoint": null,
			"startArrowhead": "arrow",
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-4.838135889811383,
					141.16666666666686
				]
			]
		},
		{
			"type": "arrow",
			"version": 165,
			"versionNonce": 729660998,
			"isDeleted": false,
			"id": "TqjWwyvc_C3wWWjfZKjVM",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6422.310974972741,
			"y": 467.2491396283235,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 226.72006472492012,
			"height": 139.83333333333348,
			"seed": 1966519021,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510769,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "T8suBcwmAsjH4xjzNbOtz",
				"gap": 9.499999999999545,
				"focus": -0.24740863047268996
			},
			"endBinding": {
				"elementId": "Yc1_qhBcrADpqZClJpjJ6",
				"gap": 16,
				"focus": -0.3156392560533452
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-226.72006472492012,
					139.83333333333348
				]
			]
		},
		{
			"type": "rectangle",
			"version": 158,
			"versionNonce": 89453635,
			"isDeleted": false,
			"id": "58z3MPre6BUoL2NIgDd_D",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6520.977641639409,
			"y": 622.41580629499,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 459258147,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "CMF1d6kK"
				},
				{
					"id": "Gv4fQofHcdfczT442J7wh",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 119,
			"versionNonce": 1574282605,
			"isDeleted": false,
			"id": "CMF1d6kK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6457.485904273686,
			"y": 656.41580629499,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 121.01652526855469,
			"height": 35,
			"seed": 1710398659,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "REMOTE",
			"rawText": "REMOTE",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "58z3MPre6BUoL2NIgDd_D",
			"originalText": "REMOTE",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 161,
			"versionNonce": 1508598982,
			"isDeleted": false,
			"id": "Gv4fQofHcdfczT442J7wh",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6399.644308306077,
			"y": 461.9158062949903,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 1.3761904761904589,
			"height": 144.49999999999972,
			"seed": 576809005,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510770,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "T8suBcwmAsjH4xjzNbOtz",
				"gap": 4.166666666666288,
				"focus": -0.012376012376051319
			},
			"endBinding": {
				"elementId": "58z3MPre6BUoL2NIgDd_D",
				"gap": 16,
				"focus": -0.0052021573652591266
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					1.3761904761904589,
					144.49999999999972
				]
			]
		},
		{
			"type": "text",
			"version": 105,
			"versionNonce": 1614311373,
			"isDeleted": false,
			"id": "qaak78un",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7024.977641639409,
			"y": 492.0824729616569,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 228.17648315429688,
			"height": 40,
			"seed": 1365337859,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- create only updates state\n- no storage happens",
			"rawText": "- create only updates state\n- no storage happens",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- create only updates state\n- no storage happens",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "rectangle",
			"version": 271,
			"versionNonce": 832634755,
			"isDeleted": false,
			"id": "fkpvgxeWtB1xe-sN1tI3a",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7764.977641639409,
			"y": 574.41580629499,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 249134477,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "FNwXlr2K"
				},
				{
					"id": "ZJbYUkz0WFsN-pQX5sC4i",
					"type": "arrow"
				},
				{
					"id": "rWa0UF4Dw1593SXvxz8jI",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 215,
			"versionNonce": 1955786285,
			"isDeleted": false,
			"id": "FNwXlr2K",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7686.113818702397,
			"y": 608.41580629499,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 90.27235412597656,
			"height": 35,
			"seed": 1330398189,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "LOCAL",
			"rawText": "LOCAL",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "fkpvgxeWtB1xe-sN1tI3a",
			"originalText": "LOCAL",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 611,
			"versionNonce": 285305670,
			"isDeleted": false,
			"id": "ZJbYUkz0WFsN-pQX5sC4i",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7755.4109780829,
			"y": 478.06052291481456,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 103.16186410687442,
			"height": 82.5,
			"seed": 704537773,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510771,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "r1mFl4TYWU4K3kd9wFG4r",
				"gap": 10.621757123180203,
				"focus": 0.3287472549070297
			},
			"endBinding": {
				"elementId": "fkpvgxeWtB1xe-sN1tI3a",
				"gap": 13.855283380175479,
				"focus": 0.37395211494211933
			},
			"lastCommittedPoint": null,
			"startArrowhead": "arrow",
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					103.16186410687442,
					82.5
				]
			]
		},
		{
			"type": "arrow",
			"version": 457,
			"versionNonce": 1244123590,
			"isDeleted": false,
			"id": "rWa0UF4Dw1593SXvxz8jI",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7534.310974972741,
			"y": 477.9158062949902,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 83.2758620689674,
			"height": 80.49999999999983,
			"seed": 1892593411,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510771,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "pxnFcBqIQlLlqbWbRU320",
				"gap": 7.810373836688996,
				"focus": -0.10901955860979652
			},
			"endBinding": {
				"elementId": "fkpvgxeWtB1xe-sN1tI3a",
				"gap": 16,
				"focus": -0.2619464436231683
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-83.2758620689674,
					80.49999999999983
				]
			]
		},
		{
			"type": "text",
			"version": 411,
			"versionNonce": 664192621,
			"isDeleted": false,
			"id": "ovxk8LuF",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -8145.784658293944,
			"y": 568.7625617163847,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 352.99273681640625,
			"height": 120,
			"seed": 1596820131,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695311306834,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- We only care about local storage \nfor loading.\n- When deleting we only delete local\ncopies\n- We never want to delete remote storage\nso we have backups in case mistakes happen",
			"rawText": "- We only care about local storage \nfor loading.\n- When deleting we only delete local\ncopies\n- We never want to delete remote storage\nso we have backups in case mistakes happen",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- We only care about local storage \nfor loading.\n- When deleting we only delete local\ncopies\n- We never want to delete remote storage\nso we have backups in case mistakes happen",
			"lineHeight": 1.25,
			"baseline": 114
		},
		{
			"type": "diamond",
			"version": 525,
			"versionNonce": 1973749485,
			"isDeleted": false,
			"id": "f03GZqCiCgAYX0ApddDGC",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5943.977641639404,
			"y": 0.24913962832368952,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 91,
			"height": 104,
			"seed": 356538701,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "2sywwB2O"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 255,
			"versionNonce": 1749346915,
			"isDeleted": false,
			"id": "2sywwB2O",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5914.651675758057,
			"y": 42.24913962832369,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 32.84806823730469,
			"height": 20,
			"seed": 1675732909,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "load",
			"rawText": "load",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "f03GZqCiCgAYX0ApddDGC",
			"originalText": "load",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 197,
			"versionNonce": 1520410957,
			"isDeleted": false,
			"id": "OXiqxmy4xy82EjL3P_QSm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5960.977641639403,
			"y": 618.892236582637,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 1217116109,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "ZYXxaffL"
				},
				{
					"id": "DpgIQlN-JEdyXw_ziK7Af",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 143,
			"versionNonce": 1503634947,
			"isDeleted": false,
			"id": "ZYXxaffL",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5882.113818702392,
			"y": 652.892236582637,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 90.27235412597656,
			"height": 35,
			"seed": 54343213,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "LOCAL",
			"rawText": "LOCAL",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "OXiqxmy4xy82EjL3P_QSm",
			"originalText": "LOCAL",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 436,
			"versionNonce": 1153865798,
			"isDeleted": false,
			"id": "DpgIQlN-JEdyXw_ziK7Af",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5833.552482759369,
			"y": 461.7255699159702,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 4.838135889820478,
			"height": 141.1666666666668,
			"seed": 1005578381,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510773,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "z-2gkwTeQjDglr72zPRgq",
				"gap": 15.455360456204914,
				"focus": -0.04771273377067269
			},
			"endBinding": {
				"elementId": "OXiqxmy4xy82EjL3P_QSm",
				"gap": 16,
				"focus": -0.02962962962963728
			},
			"lastCommittedPoint": null,
			"startArrowhead": "arrow",
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-4.838135889820478,
					141.1666666666668
				]
			]
		},
		{
			"type": "image",
			"version": 66,
			"versionNonce": 862055843,
			"isDeleted": false,
			"id": "Uy45dKhUnkFzwxWi2UNt-",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5310.144308306069,
			"y": 1931.9951713743544,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1381,
			"height": 598,
			"seed": 108687725,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "732937a81208c78c02abe60dcb84a81250376d13",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "image",
			"version": 42,
			"versionNonce": 506950157,
			"isDeleted": false,
			"id": "POsSaRJMS92bBBxsbq2xs",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6915.644308306069,
			"y": 2829.9951713743544,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1440,
			"height": 802,
			"seed": 209732909,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "091d4d4eaeefcb84a8f20c73d6ab3d7ff8516663",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "arrow",
			"version": 202,
			"versionNonce": 414859587,
			"isDeleted": false,
			"id": "kQ670SkBm2E6faozwnXcN",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5434.3109749727355,
			"y": 2836.5444534888857,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 221.3406107363926,
			"height": 338.8826154478643,
			"seed": 1449836877,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "S0wetVaCHxjAKCUvXAxjG",
				"focus": 0.5961487281842236,
				"gap": 9.504328044229538
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					221.3406107363926,
					-338.8826154478643
				]
			]
		},
		{
			"type": "image",
			"version": 94,
			"versionNonce": 402106477,
			"isDeleted": false,
			"id": "4GI4DGsragCTbWdEILqZ8",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5685.644308306069,
			"y": 3928.995171374354,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1268,
			"height": 700,
			"seed": 1618693325,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [
				{
					"id": "VEPTYLDW5Dq9eKTSqH4Ga",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "82a3cc7bb3b55fde0f8169bb6e43fcdceff8c3a1",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "image",
			"version": 82,
			"versionNonce": 1124540643,
			"isDeleted": false,
			"id": "Q-kNC1-vGrXGsc357WI_0",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7145.644308306069,
			"y": 3944.995171374354,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1268,
			"height": 700,
			"seed": 1783587501,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [
				{
					"id": "j5TCj6VGcL_kJhW6jjJRT",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "39935dd69c02dcc500df361f403d49a88d08bc84",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "image",
			"version": 242,
			"versionNonce": 757207757,
			"isDeleted": false,
			"id": "EZve3kxiKeif-vgXZeY3w",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6750.9776416394025,
			"y": 2241.9951713743544,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 946.0000000000001,
			"height": 422,
			"seed": 713971459,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "99ea706983684000aea34779dd857be12a73129b",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "ellipse",
			"version": 55,
			"versionNonce": 1003949187,
			"isDeleted": false,
			"id": "S0wetVaCHxjAKCUvXAxjG",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5601.199863861626,
			"y": 2802.550726929909,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 168.33333333333303,
			"height": 145,
			"seed": 970772995,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "kQ670SkBm2E6faozwnXcN",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 62,
			"versionNonce": 1515204909,
			"isDeleted": false,
			"id": "G6OakRswFu0EeaGLJty-F",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6186.199863861626,
			"y": 3282.550726929909,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 265,
			"height": 96.66666666666652,
			"seed": 2070754531,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "Xjl7PLancqId7jEYvlVG4",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 66,
			"versionNonce": 1458078755,
			"isDeleted": false,
			"id": "9MDKcOdjFSfuG_T_cV5UV",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6176.199863861626,
			"y": 3379.2173935965757,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 251.66666666666606,
			"height": 86.66666666666697,
			"seed": 1174769571,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "VEPTYLDW5Dq9eKTSqH4Ga",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 46,
			"versionNonce": 1290503053,
			"isDeleted": false,
			"id": "_wtkbWRcH4U-xYW8Tdu1L",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6542.866530528293,
			"y": 3069.2173935965757,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 65,
			"height": 70,
			"seed": 1901285603,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 72,
			"versionNonce": 1814693827,
			"isDeleted": false,
			"id": "uhFey_zl3ret5-xkVOeRf",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6536.199863861626,
			"y": 3284.2173935965757,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 295,
			"height": 98.33333333333348,
			"seed": 1291822403,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "frPy9dZSsZuypJrntWrtu",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 69,
			"versionNonce": 1227721197,
			"isDeleted": false,
			"id": "LcSvYzbIq__ywu8mhpW81",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6526.199863861626,
			"y": 3367.550726929909,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 273.33333333333303,
			"height": 116.66666666666652,
			"seed": 639418435,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "j5TCj6VGcL_kJhW6jjJRT",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "ellipse",
			"version": 48,
			"versionNonce": 1371150179,
			"isDeleted": false,
			"id": "4KppE-Zm8oWUEZRZLxvjn",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6866.199863861626,
			"y": 3367.550726929909,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 258.33333333333303,
			"height": 110,
			"seed": 17908835,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "5WeOhlf-7HpBgXu3yblkC",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 51,
			"versionNonce": 1344259149,
			"isDeleted": false,
			"id": "vWWFmZaekQyNVWg20TpNa",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6497.866530528304,
			"y": 3074.217393596577,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 186.66666666666697,
			"height": 425,
			"seed": 1673032931,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": null,
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					186.66666666666697,
					-425
				]
			]
		},
		{
			"type": "arrow",
			"version": 129,
			"versionNonce": 367473411,
			"isDeleted": false,
			"id": "5WeOhlf-7HpBgXu3yblkC",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6874.562974963386,
			"y": 3414.692998133212,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 494.1368888982561,
			"height": 268.2873666268829,
			"seed": 1505244845,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "4KppE-Zm8oWUEZRZLxvjn",
				"focus": 0.9251766698000526,
				"gap": 9.398473720816753
			},
			"endBinding": {
				"elementId": "UYvmg52f_UzAP5pQVxTlS",
				"focus": 0.4352462235991267,
				"gap": 19.337662337659822
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-494.1368888982561,
					268.2873666268829
				]
			]
		},
		{
			"type": "arrow",
			"version": 133,
			"versionNonce": 94089901,
			"isDeleted": false,
			"id": "frPy9dZSsZuypJrntWrtu",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6547.921761537647,
			"y": 3326.7624329743685,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 819.1114356573271,
			"height": 144.23891287848983,
			"seed": 722009763,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "uhFey_zl3ret5-xkVOeRf",
				"focus": 0.6226952790520255,
				"gap": 12.643493445129934
			},
			"endBinding": {
				"elementId": "UYvmg52f_UzAP5pQVxTlS",
				"focus": -0.30703918500650973,
				"gap": 21.00432900432679
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-819.1114356573271,
					144.23891287848983
				]
			]
		},
		{
			"type": "arrow",
			"version": 84,
			"versionNonce": 2033179299,
			"isDeleted": false,
			"id": "j5TCj6VGcL_kJhW6jjJRT",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -6431.537526199301,
			"y": 3493.539904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 55,
			"height": 450,
			"seed": 242057325,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "LcSvYzbIq__ywu8mhpW81",
				"focus": 0.2465083137498535,
				"gap": 12.036861186966377
			},
			"endBinding": {
				"elementId": "Q-kNC1-vGrXGsc357WI_0",
				"focus": -0.026373297437182416,
				"gap": 1.4552669552649604
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-55,
					450
				]
			]
		},
		{
			"type": "image",
			"version": 97,
			"versionNonce": 1447744781,
			"isDeleted": false,
			"id": "UYvmg52f_UzAP5pQVxTlS",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -8655.037526199301,
			"y": 3371.039904419089,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1267,
			"height": 720,
			"seed": 455842317,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [
				{
					"id": "5WeOhlf-7HpBgXu3yblkC",
					"type": "arrow"
				},
				{
					"id": "frPy9dZSsZuypJrntWrtu",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "f23dd068c2fdd958ead25bb9644f0609bae1ce4b",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "arrow",
			"version": 60,
			"versionNonce": 675450435,
			"isDeleted": false,
			"id": "VEPTYLDW5Dq9eKTSqH4Ga",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5974.037526199301,
			"y": 3476.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 420,
			"height": 435,
			"seed": 483499587,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "9MDKcOdjFSfuG_T_cV5UV",
				"focus": -0.18614656389319445,
				"gap": 18.4650756294527
			},
			"endBinding": {
				"elementId": "4GI4DGsragCTbWdEILqZ8",
				"focus": -0.15137500334053444,
				"gap": 17.95526695526496
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					420,
					435
				]
			]
		},
		{
			"type": "image",
			"version": 118,
			"versionNonce": 1571203949,
			"isDeleted": false,
			"id": "mU_YBwCe_N9RiiKUXyZW-",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -3674.537526199301,
			"y": 3557.539904419089,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 946.0000000000001,
			"height": 422,
			"seed": 1140332301,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [
				{
					"id": "So4l_KBR-J9ss1OvKr_AL",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "2b0acdc7924745614881e8638e0a7dd1438f665b",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "image",
			"version": 83,
			"versionNonce": 905825763,
			"isDeleted": false,
			"id": "kvsh24TQ4v2CGkwQft0WW",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5080.537526199301,
			"y": 2958.039904419089,
			"strokeColor": "transparent",
			"backgroundColor": "transparent",
			"width": 1043,
			"height": 541,
			"seed": 328767885,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"status": "pending",
			"fileId": "61a8ddd77a4eef6d7abdf8d8faf8bb29682e07aa",
			"scale": [
				1,
				1
			]
		},
		{
			"type": "arrow",
			"version": 78,
			"versionNonce": 200848845,
			"isDeleted": false,
			"id": "Xjl7PLancqId7jEYvlVG4",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5904.037526199301,
			"y": 3321.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 840,
			"height": 80,
			"seed": 1853176419,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "G6OakRswFu0EeaGLJty-F",
				"focus": 0.08827010583521656,
				"gap": 18.51723083720097
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					840,
					-80
				]
			]
		},
		{
			"type": "arrow",
			"version": 62,
			"versionNonce": 1433409923,
			"isDeleted": false,
			"id": "So4l_KBR-J9ss1OvKr_AL",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4094.037526199301,
			"y": 3381.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 422.5,
			"height": 160,
			"seed": 835034019,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": null,
			"endBinding": {
				"elementId": "mU_YBwCe_N9RiiKUXyZW-",
				"focus": 0.12691298478712573,
				"gap": 16.5
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					422.5,
					160
				]
			]
		},
		{
			"type": "ellipse",
			"version": 54,
			"versionNonce": 207254573,
			"isDeleted": false,
			"id": "BUjeABAENDTbNLUCAULng",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -8174.037526199301,
			"y": 3991.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 290,
			"height": 97.5,
			"seed": 113814637,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "BeBoWrqL8D77aBwkNbTwY",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 64,
			"versionNonce": 1406660899,
			"isDeleted": false,
			"id": "BeBoWrqL8D77aBwkNbTwY",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7879.037526199301,
			"y": 4043.539904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 937.5,
			"height": 407.5,
			"seed": 1164032877,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "BUjeABAENDTbNLUCAULng",
				"focus": -0.7712085753835025,
				"gap": 5.32459827748221
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					937.5,
					407.5
				]
			]
		},
		{
			"type": "ellipse",
			"version": 46,
			"versionNonce": 1790273165,
			"isDeleted": false,
			"id": "SC1ijjmuNOsJKUOJHPi0t",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5216.537526199301,
			"y": 4343.539904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 330,
			"height": 85,
			"seed": 1533410125,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "HObX3wobYFBOUXMqZ0a6z",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 102,
			"versionNonce": 1386468547,
			"isDeleted": false,
			"id": "HObX3wobYFBOUXMqZ0a6z",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "dashed",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4936.537526199301,
			"y": 4338.539904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 85,
			"height": 912.5,
			"seed": 1342998349,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "SC1ijjmuNOsJKUOJHPi0t",
				"focus": 0.6699607765222539,
				"gap": 16.54388838357935
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					85,
					-912.5
				]
			]
		},
		{
			"type": "ellipse",
			"version": 46,
			"versionNonce": 2042825965,
			"isDeleted": false,
			"id": "fEGAZ6o7I51HqA3WnznsF",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4819.037526199301,
			"y": 2381.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 367.5,
			"height": 145,
			"seed": 1889605901,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"id": "D71wlxQSXbcU0SJKTrk2Z",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 73,
			"versionNonce": 1098007651,
			"isDeleted": false,
			"id": "D71wlxQSXbcU0SJKTrk2Z",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "dashed",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4781.537526199301,
			"y": 2526.039904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 772.5,
			"height": 560,
			"seed": 715171085,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695301404507,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "fEGAZ6o7I51HqA3WnznsF",
				"focus": 0.2210227779699347,
				"gap": 25.864508761147732
			},
			"endBinding": {
				"elementId": "IDO-r7LeIroCmsr9U0ayq",
				"focus": -0.34755287836265314,
				"gap": 2.5
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-772.5,
					560
				]
			]
		},
		{
			"type": "rectangle",
			"version": 67,
			"versionNonce": 2030888781,
			"isDeleted": false,
			"id": "IDO-r7LeIroCmsr9U0ayq",
			"fillStyle": "hachure",
			"strokeWidth": 4,
			"strokeStyle": "dashed",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5816.537526199301,
			"y": 3083.539904419089,
			"strokeColor": "#f08c00",
			"backgroundColor": "transparent",
			"width": 260,
			"height": 402.5,
			"seed": 982867683,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"id": "D71wlxQSXbcU0SJKTrk2Z",
					"type": "arrow"
				}
			],
			"updated": 1695301404507,
			"link": null,
			"locked": false
		},
		{
			"type": "arrow",
			"version": 84,
			"versionNonce": 805025606,
			"isDeleted": false,
			"id": "nh3Sdy5tGZOlJqqkI_Ag1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7483.044019705794,
			"y": 270.63730701649126,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 112,
			"height": 132,
			"seed": 1187572557,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510765,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "wnNeO--4BUrm-yAM_OP-F",
				"gap": 9.650295610821559,
				"focus": -0.5125813920792256
			},
			"endBinding": null,
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					112,
					132
				]
			]
		},
		{
			"type": "rectangle",
			"version": 82,
			"versionNonce": 1523459725,
			"isDeleted": false,
			"id": "STxrDKKiOcPEOkQ-pwZMd",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7380.710686372459,
			"y": 423.9706403498245,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 142,
			"height": 49,
			"seed": 1092891203,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "NqBV9F8d"
				},
				{
					"id": "9HHoKdoMu5YDV_fQtg76l",
					"type": "arrow"
				}
			],
			"updated": 1695311314962,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 50,
			"versionNonce": 1350616995,
			"isDeleted": false,
			"id": "NqBV9F8d",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7334.414742158592,
			"y": 438.4706403498245,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 49.408111572265625,
			"height": 20,
			"seed": 812992195,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "publish",
			"rawText": "publish",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "STxrDKKiOcPEOkQ-pwZMd",
			"originalText": "publish",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "text",
			"version": 253,
			"versionNonce": 500096730,
			"isDeleted": false,
			"id": "6xdRekUM",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7353.710686372461,
			"y": 230.63730701649126,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 323.60064697265625,
			"height": 100,
			"seed": 151976109,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695319510904,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- Publish should instantly update state\nand return to level selection\n- We can show a spinner while the upload\nis happening in the background.\n",
			"rawText": "- Publish should instantly update state\nand return to level selection\n- We can show a spinner while the upload\nis happening in the background.\n",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- Publish should instantly update state\nand return to level selection\n- We can show a spinner while the upload\nis happening in the background.\n",
			"lineHeight": 1.25,
			"baseline": 94
		},
		{
			"type": "text",
			"version": 41,
			"versionNonce": 552459075,
			"isDeleted": false,
			"id": "gHf8xANK",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7351.044019705794,
			"y": 322.6373070164915,
			"strokeColor": "#e03131",
			"backgroundColor": "transparent",
			"width": 288.8966064453125,
			"height": 40,
			"seed": 2020494509,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- We probably cant leave the scene \nuntil the upload is done?",
			"rawText": "- We probably cant leave the scene \nuntil the upload is done?",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- We probably cant leave the scene \nuntil the upload is done?",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "diamond",
			"version": 588,
			"versionNonce": 300068461,
			"isDeleted": false,
			"id": "675-t8a1ZhjuQvhBGyRrm",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5797.5711329434425,
			"y": 5.913224081879093,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 134,
			"height": 100,
			"seed": 1750829229,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "tBhWt0JA"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 349,
			"versionNonce": 1974958819,
			"isDeleted": false,
			"id": "tBhWt0JA",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5758.859195992759,
			"y": 35.91322408187909,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 56.57612609863281,
			"height": 40,
			"seed": 1050667789,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "submit \nscore",
			"rawText": "submit score",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "675-t8a1ZhjuQvhBGyRrm",
			"originalText": "submit score",
			"lineHeight": 1.25,
			"baseline": 34
		},
		{
			"type": "arrow",
			"version": 148,
			"versionNonce": 1510819142,
			"isDeleted": false,
			"id": "8V8fquMc8fpyOonITd0pu",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5734.815218086247,
			"y": 239.08744330782304,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 99.36842105263167,
			"height": 147.78947368421052,
			"seed": 96671139,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510775,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "knVDw1Tz7SQg3FzEZvjPf",
				"gap": 11.317233848057526,
				"focus": -0.27040247415757185
			},
			"endBinding": {
				"elementId": "JR5Bf9jmFmiP1eixoJhIe",
				"gap": 8.701754385964819,
				"focus": -0.024779566086177814
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					99.36842105263167,
					147.78947368421052
				]
			]
		},
		{
			"type": "rectangle",
			"version": 115,
			"versionNonce": 728508035,
			"isDeleted": false,
			"id": "JR5Bf9jmFmiP1eixoJhIe",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5681.95556896344,
			"y": 395.5786713779984,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 142,
			"height": 49,
			"seed": 1421240643,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "9e1yz22K"
				},
				{
					"id": "8V8fquMc8fpyOonITd0pu",
					"type": "arrow"
				},
				{
					"id": "ThuEVkrZhfuVSzaNYEgtR",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 88,
			"versionNonce": 766621485,
			"isDeleted": false,
			"id": "9e1yz22K",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5635.243624383362,
			"y": 410.0786713779984,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 48.57611083984375,
			"height": 20,
			"seed": 2101233891,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "submit",
			"rawText": "submit",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "JR5Bf9jmFmiP1eixoJhIe",
			"originalText": "submit",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 234,
			"versionNonce": 147480099,
			"isDeleted": false,
			"id": "kqIE49NRUneecQa6tjCN1",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5679.22902768028,
			"y": 615.8869082924052,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 487055821,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "VOBrMJlS"
				},
				{
					"id": "ThuEVkrZhfuVSzaNYEgtR",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 185,
			"versionNonce": 103565709,
			"isDeleted": false,
			"id": "VOBrMJlS",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5615.737290314557,
			"y": 649.8869082924052,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 121.01652526855469,
			"height": 35,
			"seed": 178528301,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "REMOTE",
			"rawText": "REMOTE",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "kqIE49NRUneecQa6tjCN1",
			"originalText": "REMOTE",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 102,
			"versionNonce": 1142256198,
			"isDeleted": false,
			"id": "ThuEVkrZhfuVSzaNYEgtR",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5599.439553996069,
			"y": 449.4921714502999,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 25.41882876204636,
			"height": 150.39473684210532,
			"seed": 166589507,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510777,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "JR5Bf9jmFmiP1eixoJhIe",
				"gap": 4.913500072301474,
				"focus": -0.08709935554184335
			},
			"endBinding": {
				"elementId": "kqIE49NRUneecQa6tjCN1",
				"gap": 16,
				"focus": -0.05563686333217812
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					25.41882876204636,
					150.39473684210532
				]
			]
		},
		{
			"type": "rectangle",
			"version": 620,
			"versionNonce": 1785040877,
			"isDeleted": false,
			"id": "pRYYqV8XRYQyUm8k1wKDl",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5234.061280996272,
			"y": -118.90707740525346,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 262,
			"height": 99,
			"seed": 1882858733,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "KZrUdAZZ"
				},
				{
					"id": "BNvHG8rE8tpsK1G6kjTCD",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 562,
			"versionNonce": 74281315,
			"isDeleted": false,
			"id": "KZrUdAZZ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5202.4011857814285,
			"y": -81.90707740525346,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 198.6798095703125,
			"height": 25,
			"seed": 1088487245,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelBrowseManager",
			"rawText": "LevelBrowseManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "pRYYqV8XRYQyUm8k1wKDl",
			"originalText": "LevelBrowseManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "rectangle",
			"version": 512,
			"versionNonce": 1483147853,
			"isDeleted": false,
			"id": "NlkACs9K3T5DmpGntYOIx",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5262.482333627849,
			"y": 140.18064189299207,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 314,
			"height": 96,
			"seed": 1618841005,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "xGhfipqI"
				},
				{
					"id": "BNvHG8rE8tpsK1G6kjTCD",
					"type": "arrow"
				},
				{
					"id": "PoZb6ZUqtfZcYY7siEitX",
					"type": "arrow"
				},
				{
					"id": "-7e2DQPpPBYjVIhwkxXVM",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 432,
			"versionNonce": 220155139,
			"isDeleted": false,
			"id": "xGhfipqI",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5170.722270456462,
			"y": 175.68064189299207,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 130.47987365722656,
			"height": 25,
			"seed": 1312378893,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 20,
			"fontFamily": 1,
			"text": "LevelManager",
			"rawText": "LevelManager",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "NlkACs9K3T5DmpGntYOIx",
			"originalText": "LevelManager",
			"lineHeight": 1.25,
			"baseline": 18
		},
		{
			"type": "arrow",
			"version": 1272,
			"versionNonce": 1444880198,
			"isDeleted": false,
			"id": "BNvHG8rE8tpsK1G6kjTCD",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5105.213551743898,
			"y": -4.1877791596393905,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 9.88948086923483,
			"height": 126.54385964912264,
			"seed": 1745733229,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510778,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "pRYYqV8XRYQyUm8k1wKDl",
				"gap": 15.719298245614084,
				"focus": 0.05375014636200133
			},
			"endBinding": {
				"elementId": "NlkACs9K3T5DmpGntYOIx",
				"gap": 17.824561403508824,
				"focus": 0.09519362902961356
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					9.88948086923483,
					126.54385964912264
				]
			]
		},
		{
			"type": "arrow",
			"version": 788,
			"versionNonce": 1317827270,
			"isDeleted": false,
			"id": "PoZb6ZUqtfZcYY7siEitX",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5101.505349432512,
			"y": 250.4964313666763,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 2.250278350725239,
			"height": 146.5263157894737,
			"seed": 281309389,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510780,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "NlkACs9K3T5DmpGntYOIx",
				"gap": 14.315789473684163,
				"focus": -0.03127987800358763
			},
			"endBinding": {
				"elementId": "_0-QnlVrjwFRxYZGQNnef",
				"gap": 12.657894736841968,
				"focus": 0.1345022466144552
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-2.250278350725239,
					146.5263157894737
				]
			]
		},
		{
			"type": "rectangle",
			"version": 350,
			"versionNonce": 2113444621,
			"isDeleted": false,
			"id": "_0-QnlVrjwFRxYZGQNnef",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5203.043737136623,
			"y": 409.68064189299196,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 174,
			"height": 45,
			"seed": 368496429,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "1Nh9xxhw"
				},
				{
					"id": "PoZb6ZUqtfZcYY7siEitX",
					"type": "arrow"
				},
				{
					"id": "naDnVPns7crekhHOOaO7L",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 315,
			"versionNonce": 1874602051,
			"isDeleted": false,
			"id": "1Nh9xxhw",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5141.923787795803,
			"y": 422.18064189299196,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 51.760101318359375,
			"height": 20,
			"seed": 1443783053,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "loadAll",
			"rawText": "loadAll",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "_0-QnlVrjwFRxYZGQNnef",
			"originalText": "loadAll",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "diamond",
			"version": 604,
			"versionNonce": 397128045,
			"isDeleted": false,
			"id": "yt7T07V6MqZmOqg_crK_O",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5271.5819208311505,
			"y": 8.659572061550307,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 139,
			"height": 104,
			"seed": 642033645,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [
				{
					"type": "text",
					"id": "TSWpnrCO"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 349,
			"versionNonce": 1328818147,
			"isDeleted": false,
			"id": "TSWpnrCO",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5227.71197149033,
			"y": 50.65957206155031,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 51.760101318359375,
			"height": 20,
			"seed": 2023957069,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "loadAll",
			"rawText": "loadAll",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "yt7T07V6MqZmOqg_crK_O",
			"originalText": "loadAll",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 262,
			"versionNonce": 191691725,
			"isDeleted": false,
			"id": "_EOZxpNEB_wyseYZh-J0k",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5240.581920831149,
			"y": 627.3026690158638,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 1107513517,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "DqMO0rfD"
				},
				{
					"id": "l-MuUCDSv4BRacf0ISOlB",
					"type": "arrow"
				},
				{
					"id": "naDnVPns7crekhHOOaO7L",
					"type": "arrow"
				},
				{
					"id": "gLViQTHVDk3Pic_hg1J4q",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 214,
			"versionNonce": 1791701891,
			"isDeleted": false,
			"id": "DqMO0rfD",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5161.718097894137,
			"y": 661.3026690158638,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 90.27235412597656,
			"height": 35,
			"seed": 537834253,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "LOCAL",
			"rawText": "LOCAL",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "_EOZxpNEB_wyseYZh-J0k",
			"originalText": "LOCAL",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 654,
			"versionNonce": 1867122566,
			"isDeleted": false,
			"id": "l-MuUCDSv4BRacf0ISOlB",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5059.823428617782,
			"y": 471.4693356825302,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 161.8285307768465,
			"height": 135.8333333333336,
			"seed": 1061079405,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510784,
			"link": null,
			"locked": false,
			"startBinding": null,
			"endBinding": {
				"elementId": "S7Sv5KwQdDeiC1LKh3GuJ",
				"gap": 16.994671709768227,
				"focus": 0.0994913070564497
			},
			"lastCommittedPoint": null,
			"startArrowhead": "arrow",
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					161.8285307768465,
					135.8333333333336
				]
			]
		},
		{
			"type": "arrow",
			"version": 336,
			"versionNonce": 511233222,
			"isDeleted": false,
			"id": "-7e2DQPpPBYjVIhwkxXVM",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5014.4194972779915,
			"y": 247.4978757410496,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 99.36842105263077,
			"height": 147.78947368421052,
			"seed": 479872141,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510782,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "NlkACs9K3T5DmpGntYOIx",
				"gap": 11.317233848057526,
				"focus": -0.27040247415757424
			},
			"endBinding": {
				"elementId": "HkhTYefeRVtapEtywXhQr",
				"gap": 8.701754385964819,
				"focus": -0.024779566086180177
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					99.36842105263077,
					147.78947368421052
				]
			]
		},
		{
			"type": "rectangle",
			"version": 178,
			"versionNonce": 1649421453,
			"isDeleted": false,
			"id": "HkhTYefeRVtapEtywXhQr",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4961.559848155185,
			"y": 403.98910381122494,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 142,
			"height": 49,
			"seed": 391916269,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "zpe0E8nX"
				},
				{
					"id": "-7e2DQPpPBYjVIhwkxXVM",
					"type": "arrow"
				},
				{
					"id": "iYKKKbb3bIAHJQxlc_zQG",
					"type": "arrow"
				},
				{
					"id": "gLViQTHVDk3Pic_hg1J4q",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 158,
			"versionNonce": 1266708163,
			"isDeleted": false,
			"id": "zpe0E8nX",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4924.575915660068,
			"y": 418.48910381122494,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 68.03213500976562,
			"height": 20,
			"seed": 30557517,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "download",
			"rawText": "download",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "HkhTYefeRVtapEtywXhQr",
			"originalText": "download",
			"lineHeight": 1.25,
			"baseline": 14
		},
		{
			"type": "rectangle",
			"version": 299,
			"versionNonce": 2115310317,
			"isDeleted": false,
			"id": "S7Sv5KwQdDeiC1LKh3GuJ",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4958.833306872025,
			"y": 624.297340725632,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 1062605741,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "XW3zKp1U"
				},
				{
					"id": "iYKKKbb3bIAHJQxlc_zQG",
					"type": "arrow"
				},
				{
					"id": "l-MuUCDSv4BRacf0ISOlB",
					"type": "arrow"
				},
				{
					"id": "gLViQTHVDk3Pic_hg1J4q",
					"type": "arrow"
				}
			],
			"updated": 1695301404508,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 248,
			"versionNonce": 1739300451,
			"isDeleted": false,
			"id": "XW3zKp1U",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4895.341569506302,
			"y": 658.297340725632,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 121.01652526855469,
			"height": 35,
			"seed": 202446349,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695301404508,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "REMOTE",
			"rawText": "REMOTE",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "S7Sv5KwQdDeiC1LKh3GuJ",
			"originalText": "REMOTE",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 290,
			"versionNonce": 707133510,
			"isDeleted": false,
			"id": "iYKKKbb3bIAHJQxlc_zQG",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4879.043833187815,
			"y": 457.90260388352664,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 25.41882876204636,
			"height": 150.3947368421055,
			"seed": 1660734573,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510784,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "HkhTYefeRVtapEtywXhQr",
				"gap": 4.913500072301645,
				"focus": -0.08709935554182945
			},
			"endBinding": {
				"elementId": "S7Sv5KwQdDeiC1LKh3GuJ",
				"gap": 15.999999999999886,
				"focus": -0.05563686333217911
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					25.41882876204636,
					150.3947368421055
				]
			]
		},
		{
			"type": "arrow",
			"version": 767,
			"versionNonce": 1993465798,
			"isDeleted": false,
			"id": "naDnVPns7crekhHOOaO7L",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -5114.960204812385,
			"y": 464.90265551899955,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 2.1714692231544177,
			"height": 149.1666666666668,
			"seed": 329675437,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510781,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "_0-QnlVrjwFRxYZGQNnef",
				"gap": 10.222013626007595,
				"focus": -0.01786237945504369
			},
			"endBinding": {
				"elementId": "_EOZxpNEB_wyseYZh-J0k",
				"gap": 13.233346830197434,
				"focus": -0.011960735967820959
			},
			"lastCommittedPoint": null,
			"startArrowhead": "arrow",
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-2.1714692231544177,
					149.1666666666668
				]
			]
		},
		{
			"type": "arrow",
			"version": 340,
			"versionNonce": 1220622790,
			"isDeleted": false,
			"id": "gLViQTHVDk3Pic_hg1J4q",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4927.6601312908115,
			"y": 456.28083491921717,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 110.84296978663588,
			"height": 155.02183409664661,
			"seed": 1294084483,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510783,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "HkhTYefeRVtapEtywXhQr",
				"gap": 3.2917311079922342,
				"focus": 0.19463608936827195
			},
			"endBinding": {
				"elementId": "_EOZxpNEB_wyseYZh-J0k",
				"gap": 16,
				"focus": 0.18539164356664797
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					-110.84296978663588,
					155.02183409664661
				]
			]
		},
		{
			"type": "text",
			"version": 347,
			"versionNonce": 305566662,
			"isDeleted": false,
			"id": "PBTdJPXB",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -4713.324486916607,
			"y": 391.03112913863833,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 426.4168701171875,
			"height": 160,
			"seed": 592875907,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695319510905,
			"link": null,
			"locked": false,
			"fontSize": 16,
			"fontFamily": 1,
			"text": "- When loading levels we want to get remote \nlevels, but also show if we have downloaded it\nalready so we need to combine remote and local\nstate\n- When downloading we want to instantly create\na local save stub with limited information which\ncan be updated with level data once it is downloaded\n",
			"rawText": "- When loading levels we want to get remote \nlevels, but also show if we have downloaded it\nalready so we need to combine remote and local\nstate\n- When downloading we want to instantly create\na local save stub with limited information which\ncan be updated with level data once it is downloaded\n",
			"textAlign": "left",
			"verticalAlign": "top",
			"containerId": null,
			"originalText": "- When loading levels we want to get remote \nlevels, but also show if we have downloaded it\nalready so we need to combine remote and local\nstate\n- When downloading we want to instantly create\na local save stub with limited information which\ncan be updated with level data once it is downloaded\n",
			"lineHeight": 1.25,
			"baseline": 154
		},
		{
			"type": "rectangle",
			"version": 252,
			"versionNonce": 982061219,
			"isDeleted": false,
			"id": "oeH3K5DR3Dh1RB9UE37fV",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7428.99208902524,
			"y": 578.674669425527,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 248,
			"height": 103,
			"seed": 1372952291,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 3
			},
			"boundElements": [
				{
					"type": "text",
					"id": "64ecc5f0"
				},
				{
					"id": "9HHoKdoMu5YDV_fQtg76l",
					"type": "arrow"
				}
			],
			"updated": 1695311317354,
			"link": null,
			"locked": false
		},
		{
			"type": "text",
			"version": 213,
			"versionNonce": 352650307,
			"isDeleted": false,
			"id": "64ecc5f0",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7365.500351659517,
			"y": 612.674669425527,
			"strokeColor": "#1e1e1e",
			"backgroundColor": "transparent",
			"width": 121.01652526855469,
			"height": 35,
			"seed": 359056003,
			"groupIds": [],
			"frameId": null,
			"roundness": null,
			"boundElements": [],
			"updated": 1695311317354,
			"link": null,
			"locked": false,
			"fontSize": 28,
			"fontFamily": 1,
			"text": "REMOTE",
			"rawText": "REMOTE",
			"textAlign": "center",
			"verticalAlign": "middle",
			"containerId": "oeH3K5DR3Dh1RB9UE37fV",
			"originalText": "REMOTE",
			"lineHeight": 1.25,
			"baseline": 25
		},
		{
			"type": "arrow",
			"version": 347,
			"versionNonce": 128343558,
			"isDeleted": false,
			"id": "9HHoKdoMu5YDV_fQtg76l",
			"fillStyle": "hachure",
			"strokeWidth": 1,
			"strokeStyle": "solid",
			"roughness": 1,
			"opacity": 100,
			"angle": 0,
			"x": -7307.523644513538,
			"y": 486.13104219538366,
			"strokeColor": "#1971c2",
			"backgroundColor": "transparent",
			"width": 1.000603513754868,
			"height": 76.54362723014333,
			"seed": 286358051,
			"groupIds": [],
			"frameId": null,
			"roundness": {
				"type": 2
			},
			"boundElements": [],
			"updated": 1695319510785,
			"link": null,
			"locked": false,
			"startBinding": {
				"elementId": "STxrDKKiOcPEOkQ-pwZMd",
				"gap": 13.160401845559136,
				"focus": -0.023770615631038522
			},
			"endBinding": {
				"elementId": "oeH3K5DR3Dh1RB9UE37fV",
				"gap": 16,
				"focus": -0.005202157365259505
			},
			"lastCommittedPoint": null,
			"startArrowhead": null,
			"endArrowhead": "arrow",
			"points": [
				[
					0,
					0
				],
				[
					1.000603513754868,
					76.54362723014333
				]
			]
		}
	],
	"appState": {
		"theme": "light",
		"viewBackgroundColor": "#ffffff",
		"currentItemStrokeColor": "#1971c2",
		"currentItemBackgroundColor": "transparent",
		"currentItemFillStyle": "hachure",
		"currentItemStrokeWidth": 1,
		"currentItemStrokeStyle": "solid",
		"currentItemRoughness": 1,
		"currentItemOpacity": 100,
		"currentItemFontFamily": 1,
		"currentItemFontSize": 16,
		"currentItemTextAlign": "left",
		"currentItemStartArrowhead": null,
		"currentItemEndArrowhead": "arrow",
		"scrollX": 3479.077398908292,
		"scrollY": 576.5777690795838,
		"zoom": {
			"value": 0.49999999999999994
		},
		"currentItemRoundness": "round",
		"gridSize": null,
		"gridColor": {
			"Bold": "#C9C9C9FF",
			"Regular": "#EDEDEDFF"
		},
		"currentStrokeOptions": null,
		"previousGridSize": null,
		"frameRendering": {
			"enabled": true,
			"clip": true,
			"name": true,
			"outline": true
		}
	},
	"files": {}
}
```
%%