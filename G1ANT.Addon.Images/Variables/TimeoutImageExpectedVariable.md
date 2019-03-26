# timeoutimageexpected

## Syntax

```G1ANT
♥timeoutimageexpected = ⟦timespan⟧
```

## Description

Determines the timeout value (in ms) for the [image.expected](G1ANT.Addon/G1ANT.Addon.Images/G1ANT.Addon.Images/Commands/ImageExpectedCommand.md) command; the default value is 20000 (20 seconds).

## Example

```G1ANT
♥timeoutimageexpected = 200

♥image = ♥environment⟦USERPROFILE⟧\Desktop\image.png
file.download https://jeremykun.files.wordpress.com/2012/01/img49.png filename ♥image
program ♥image
image.expected ♥image result ♥isPresent relative false errormessage ‴No such image‴
dialog ♥isPresent
```

The `image.expected` command checks for an image presence on screen. In the example above, the command has only 200ms for processing the screen content instead of the default 20 seconds, so despite the image is displayed, an error message “*No such image*” appears, because the timeout expires.
