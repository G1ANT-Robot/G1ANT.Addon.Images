# image.find

**Syntax:**

```G1ANT
image.find  image1 ‴‴
```

**Description:**

Command `image.find` allows to find provided image in another image (or part of the screen/entire screen).

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`image1`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  | path of the picture to be found|
|`image2`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | path of the picture where image1 will be searched- if not specified, image1 will be searched on the screen |
|`screensearcharea`| [rectangle](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/rectangle.md) | no |  | argument narrowing search area, specified can speed up the search, format: ‴x0⫽y0⫽x1⫽y1‴ (x0,y0 – coordinates of a top left corner; x1,y1 – coordinates of a right bottom corner of the area)  |
|`relative`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true| argument specifying whether the search is to be done relatively to the foreground window |
|`threshold`| [float](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/float.md) | no | 0 | tolerance treshold- by default 0, which means the image has to match in 100% |
|`centerresult`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | if specified, result point will be pointing at the middle of the found area |
|`offsetx`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | | value that will be added to the result's X coordinate |
|`offsety`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no |  | value that will be added to the result's Y coordinate |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable where X,Y coordinates (rectangle center) will be stored |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutimagefind](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md) | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Images.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Images](https://github.com/G1ANT-Robot/G1ANT.Addon.Images)

**Example 1:**

Here we will attempt to find G1ANT.png image at current focused screen.

```G1ANT
image.find image1 ‴C:\G1ant.png‴ image2 ‴‴ screensearcharea ‴‴ relative false
dialog ♥result
```

**Example 2:**

Here we will attempt to find G1antLogo.png image within another picture.

```G1ANT
image.find image1 ‴C:\G1antLogo.png‴ image2 ‴C:\temp\G1antFull.png‴ relative false
dialog ♥result
```

**Example 3:**

```G1ANT
selenium.open type ‴firefox‴ url ‴duckduckgo.com‴
image.find image1 ‴C:\Users\diana\Downloads\Screenshot_33.png‴ image2 ‴‴ relative false
dialog ♥result
```
