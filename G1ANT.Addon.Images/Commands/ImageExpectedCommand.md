# image.expected

**Syntax:**

```G1ANT
image.expected  image1 ‴‴
```

**Description:**

Command `image.expected` allows to confirm if image1 is exactly the same as image2.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`image1`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  | path to the picture to be found |
|`image2`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | path to the picture in which image1 will be searched. If not specified, image1 will be searched on the screen|
|`screensearcharea`| [rectangle](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/rectangle.md) | no |  | argument narrowing search area, specified can speed up the search, format: ‴x0⫽y0⫽x1⫽y1‴ (x0,y0 – coordinates of a top left corner; x1,y1 – coordinates of a right bottom corner of the area) |
|`relative`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true| argument specifying whether the search is to be done relatively to the foreground window |
|`threshold`| [float](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/float.md) | no | 0 | tolerance treshold- by default 0, which means the image has to match in 100% |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable where execution status will be stored |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutimageexpected](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md) | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Images.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Images](https://github.com/G1ANT-Robot/G1ANT.Addon.Images)

**Example 1:**

Here we will verify whether the rectangle displayed on the current screen is exactly the same as G1antLogo.png.

```G1ANT
image.expected image1 ‴C:\G1antLogo.png‴ image2 ‴‴ screensearcharea ‴446⫽77⫽549⫽111‴ relative true
dialog ♥result
```

**Example 2:**

Here we will verify whether the rectangle of larger image is exactly the same as G1antLogo.png.

```G1ANT
image.expected image1 ‴C:\G1antLogo.png‴ image2 ‴C:\temp\G1antFull.png‴ screensearcharea ‴446⫽77⫽549⫽111‴ relative ‴false‴
dialog ♥result
```

**Example 3:**

In this example we are checking if image1 from the specified path equals opened ‴duckduckgo.com‴.

```G1ANT
selenium.open type ‴firefox‴ url ‴duckduckgo.com‴
image.expected image1 ‴C:\Users\♥environment⟦USERNAME⟧\Downloads\Screenshot_33.png‴ image2 ‴‴ threshold 0 screensearcharea ‴542⫽268⫽783⫽466‴ result ♥result
dialog ♥result
```
