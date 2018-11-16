# waitfor.image

**Syntax:**

```G1ANT
waitfor.image image ‴‴
```

**Description:**

Command `waitfor.image` allows to wait for specified image within another image or current screen view.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`image`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  |specifies path to an awaited image |
|`relative`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | argument specifying whether the search is to be done relatively to the foreground window |
|`threshold`| [float](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/float.md) | no | 0 | tolerance threshold- by default 0, which means the image has to match in 100% (possible: from 0 to 1) |
|`screensearcharea`| [rectangle](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/rectangle.md) | no |   | argument narrowing the search area, specifying can speed up the search |
|`centerresult`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md)  | no | true | if specified, result point will be pointing at the middle of the found area |
|`offsetx`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md)  | no | | value to be added to the result's X coordinate |
|`offsety`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md)  | no |  | value to be added to the result's Y coordinate |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable where execution status will be stored |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutimagefind](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md) | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Images.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Images](https://github.com/G1ANT-Robot/G1ANT.Addon.Images)

**Example 1**:

```G1ANT
waitfor.image image1 ‴image1.bmp‴
```

**Example 2:**

```G1ANT
selenium.open type ‴firefox‴ url ‴duckduckgo.com‴
waitfor.image image ‴C:\Users\diana\Pictures\Screenshot_33.png‴ relative false threshold 0 centerresult true result ♥point timeout 5000
dialog ♥point
```

