# timeoutimagefind

## Syntax

```G1ANT
♥timeoutimagefind = ⟦timespan⟧
```

## Description

Determines the timeout value (in ms) for the [image.find](G1ANT.Addon/G1ANT.Addon.Images/G1ANT.Addon.Images/Commands/ImageFindCommand.md) and [waifor.image](G1ANT.Addon/G1ANT.Addon.Images/G1ANT.Addon.Images/Commands/WaitforImageCommand.md) commands; the default value is 20000 (20 seconds).

## Example

```G1ANT
♥timeoutimagefind = 200

♥image = ♥environment⟦USERPROFILE⟧\Desktop\image.png
file.download https://jeremykun.files.wordpress.com/2012/01/img49.png filename ♥image
program ♥image
image.find ♥image result ♥point relative false errormessage ‴No such image‴
dialog ♥point
```

The `image.find` command checks for an image presence on screen. In the example above, the command has only 200ms for processing the screen content instead of the default 20 seconds, so despite the image is displayed, an error message “*No such image*” appears, because the timeout expires.
