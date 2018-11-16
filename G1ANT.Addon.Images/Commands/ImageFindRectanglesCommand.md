# image.findrectangles

**Syntax:**

```G1ANT
image.find  image1 ‴‴  image2 ‴‴  rectangle ‴‴  relative ‴‴  result ♥variablename 
timeout 5000  if true  errorjump ➜labelname  errormessage ‴‴
```

**Description:**

Command `image.findrectangles` allows to find provided image in another image (or part of the screen/entire screen).
Required argument: path.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`image1`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes | ‴ ‴ | path of the picture to be found|
|`image2`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no | ‴ ‴ | path of the picture where image1 will be searched- if not specified, image1 will be searched on the screen |
|`screensearcharea`| [rectangle](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/rectangle.md) | no | ‴ ‴ | argument narrowing search area, specified can speed up the search, format: ‴x0⫽y0⫽x1⫽y1‴ (x0,y0 – coordinates of a top left corner; x1,y1 – coordinates of a right bottom corner of the area) |
|`relative`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true| argument specifying whether the search is to be done relatively to the foreground window |
|`threshold`| "float":{TOPIC-LINK+decimal}| no | number 0-1 | tolerance treshold- by default 0, which means the image has to match in 100% |
|`centerresult`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | if specified, result point will be pointing at the middle of the found area |
|`offsetx`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | number | value that will be added to the result's X coordinate |
|`offsety`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | number | value that will be added to the result's Y coordinate |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | ♥variablename | name of variable where X,Y coordinates (rectangle center) will be stored |
|`timeout`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | "timeoutimagefind":{TOPIC-LINK+special-variables} | specifies maximum number of milliseconds to wait for picture to be found|
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if "if" condition is true |
|`errorjump`| [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | ➜labelname | name of the label to jump to if given timeout expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no | ‴ ‴ | message that will be shown in case error occurs and no `errorjump` argument is specified |
