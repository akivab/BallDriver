look at solid.cs for the simplest example of an effect.

The effects is a subclasses of FullBallEffect which includes some helper functions, and colors.

Some effects create children effects and can render the child effect instead of a pixel. Look at checker.cs for an example.
checker.cs also uses the polar coordinate array which is available but not passed to buildFrame.


